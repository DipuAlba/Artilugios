# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

`SeDipuAlba.Artilugios` ("Artilugios" = gadgets) is an internal-use NuGet utility
library for SEDIPUALB@ (Diputación Provincial de Albacete). It is a grab-bag of
independent, mostly-static helper classes — there is no shared runtime, DI graph,
or domain model tying them together. Each file is self-contained; adding a utility
means adding a class, not wiring into a framework.

Published at https://www.nuget.org/packages/SeDipuAlba.Artilugios — changes ship to
other internal projects, so treat public method signatures as a stable API surface.

## Solution layout

Three projects in `SeDipuAlba.Artilugios.sln`:

- **`SeDipuAlba.Artilugios`** — the library. Targets **`netstandard2.0`**.
  `SeDipuAlba.Artilugios` namespace at the root; `SeDipuAlba.Artilugios.Extensions`
  for the `this`-parameter extension classes under `Extensions/`.
- **`SeDipuAlba.Artilugios.Tests`** — NUnit 3 tests. Targets `net462`.
- **`SeDipuAlba.Artilugios.Benchmarks`** — BenchmarkDotNet console app, run manually.
  Targets `net6.0`.

## Commands

```bash
dotnet build                              # build the solution
dotnet test                               # run all tests (34 at last count)
dotnet test --filter "FullyQualifiedName~SpanishIdTests"   # one test class
dotnet test --filter "Name=ValidateNif_ValidNif_ReturnsPersonalNif"  # one test
```

Benchmarks are opt-in and hand-edited: `Program.cs` hardcodes which
`BenchmarkRunner.Run<T>()` executes (the other is commented out). Uncomment the one
you want, then:

```bash
dotnet run --project SeDipuAlba.Artilugios.Benchmarks --configuration Release
```

Packaging (run from the `SeDipuAlba.Artilugios` project directory):

```bash
dotnet pack SeDipuAlba.Artilugios.csproj --configuration Release --output .
```

`generarnuget.cmd` wraps this: it archives the previous `.nupkg` into `oldnupkg/`
before packing. `GeneratePackageOnBuild` is off, so a plain build never produces a package.

## Things that will bite you

- **`netstandard2.0` constrains the library.** No modern BCL conveniences — e.g.
  `Crypto.cs` uses `RijndaelManaged` / `Rfc2898DeriveBytes` (defaults to SHA-1, no
  modern overloads). Don't reach for newer-runtime APIs in the library project; the
  test project (`net462`) and benchmarks (`net6.0`) are not so constrained.
- **Version numbers are auto-generated from the build date** via MSBuild expressions
  in the `.csproj` (`1.0.<days-since-2000>.<seconds-of-day/1.32>`). Never hand-edit
  `Version`/`FileVersion`/`AssemblyVersion`; every build mints a new version.
- **`SeDipuAlba.Artilugios.nuspec` is dead weight.** The project is SDK-style and
  `dotnet pack` regenerates the nuspec from `.csproj` properties, ignoring the
  checked-in file. Its `<icon>logo-sedipualba.png</icon>` and `projectUrl` never reach
  the package (`logo-sedipualba.png` is not packed at all). To change package metadata,
  edit the `.csproj`; to ship the icon you'd need `PackageIcon` there.
- **`GenerateDocumentationFile` is on**, so every undocumented public type or member
  raises `CS1591`. There is existing noise (all of `SpanishId`'s public surface), but
  don't add more — give new public members XML docs.
- **`dotnet test` also builds the benchmarks project**, which emits `NETSDK1138`
  (net6.0 out of support). That warning is pre-existing noise, not something you broke.
- **The GitHub Actions workflow (`dotnet-desktop.yml`) is partly stale** — its
  `Solution_Name`/`Test_Project_Path` env vars reference a `DipuAlba.Artilugios.*`
  name that no longer matches. The actual build step is just `dotnet test`, which
  works regardless. If you touch the workflow, fix or ignore those unused vars.
  Note it only triggers on `main`, so pushes to `develop` are not CI-checked.

## Spanish-domain logic (the non-obvious part)

`SpanishId.ValidateNif` is the core domain routine: it validates Spanish tax IDs and
returns a `NifType` (`PersonalNif`, `Nie`, `LegalEntityNif`, `Invalid`). Two distinct
control-digit algorithms live here, tried in order:

1. **mod-23 letter lookup** for personal NIF/NIE — `"TRWAGMYFPDXBNJZSQVHLCKE"[n % 23]`,
   with the leading `X`/`Y`/`Z` of a NIE substituted for `0`/`1`/`2` before the modulo.
   `K`/`L`/`M` prefixes are also personal (and `M` is reported as a NIE).
2. **Luhn-like digit sum** for legal-entity NIFs (`A`–`W` prefix), mapping to the
   `JABCDEFGHI` letter table. The prefix letter decides whether the control character
   is that letter or the digit; `C`-prefixed IDs accept either, so
   `GetControlDigitForLegalEntityNif` takes the supplied control char and may echo it back.

Input must be **exactly 9 chars and uppercase** — `ValidateNif` does no normalization
and lowercase is rejected by design. `StringMask.MaskSpanishNif` does `Trim()` +
`ToUpperInvariant()` first, so lowercase input works through the masking API but not
through `ValidateNif` directly.

`StringMask.MaskSpanishNif` implements AEPD (Spanish data-protection agency) data
minimization; `docs/orientaciones-da7.pdf` is the source guidance — consult it before
changing masking behavior. The rules:

- Valid NIF/NIE/legal-entity ID → keep the **4th through 7th digit**, counting digits
  only and masking every non-digit (so `12345678Z` → `***4567**`, `X1234567L` →
  `****4567*`).
- Invalid input with ≥7 digits → same digit-position rule.
- Invalid input with <7 digits → keep the last 4 characters.
- Null/whitespace → `ArgumentException`.

`StringMaskTests` pins all of this; lean on it and keep the AEPD rationale intact.
The instance API (`new StringMask(s, 'X').ShowFirst(n).ShowLast(n)`) is unrelated to
the NIF logic — it is a general chainable masker whose bounds are exclusive
(`Guard.IsBetweenExclusive`), so `ShowFirst(0)` and `ShowFirst(length)` both throw.

## Conventions

- Code, identifiers, and XML doc comments are in **English**; some inline comments are
  in Spanish. Match the surrounding language of the file you edit.
- Recent commit messages are in **Spanish**, often prefixed with the touched file
  (`SeDipuAlba.Artilugios.csproj: Añade licencia EUPL-1.2`). Older history is English.
- Utilities that adapt third-party code cite the source (StackOverflow) in the class
  XML doc — preserve those attributions.
- Tests are NUnit 3. Fixtures carry no `[TestFixture]` attribute (NUnit infers them
  from the `[Test]` methods; `CryptoTests` is even `internal` and still runs). Name new
  tests in the existing `Method_Scenario_ExpectedResult` style. Older fixtures (`Tests.cs`) use terser names.
- Work happens on `develop`; `main` is the release branch (and the only one CI covers).
