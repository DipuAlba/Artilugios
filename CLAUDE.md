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
- **`SeDipuAlba.Artilugios.Tests`** — NUnit 3 tests. Targets `net462`.
- **`SeDipuAlba.Artilugios.Benchmarks`** — BenchmarkDotNet console app, run manually.

## Commands

```bash
dotnet build                              # build the solution
dotnet test                               # run all tests
dotnet test --filter "FullyQualifiedName~SpanishIdTests"   # one test class
dotnet test --filter "Name=ValidateNif_ValidNif_ReturnsPersonalNif"  # one test
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
  test project (`net462`) and benchmarks are not so constrained.
- **Version numbers are auto-generated from the build date** via MSBuild expressions
  in the `.csproj` (`1.0.<days-since-2000>.<seconds-of-day/1.32>`). Never hand-edit
  `Version`/`FileVersion`/`AssemblyVersion`; every build mints a new version.
- **The GitHub Actions workflow (`dotnet-desktop.yml`) is partly stale** — its
  `Solution_Name`/`Test_Project_Path` env vars reference a `DipuAlba.Artilugios.*`
  name that no longer matches. The actual build step is just `dotnet test`, which
  works regardless. If you touch the workflow, fix or ignore those unused vars.

## Spanish-domain logic (the non-obvious part)

`SpanishId.ValidateNif` is the core domain routine: it validates Spanish tax IDs and
returns a `NifType` (`PersonalNif`, `Nie`, `LegalEntityNif`, `Invalid`). Two distinct
control-digit algorithms live here — mod-23 letter lookup for personal NIF/NIE, and a
Luhn-like digit sum for legal-entity NIFs. Input must be **9 chars, uppercase**;
lowercase is rejected by design.

`StringMask.MaskSpanishNif` builds on it for AEPD-compliant (Spanish data-protection)
masking — keeps digit positions 4–7 visible for valid IDs, with documented fallbacks
for invalid input. Recent commits centered on getting these masking rules right, so
when changing masking behavior, lean on `StringMaskTests` and keep the AEPD rationale intact.

## Conventions

- Code, identifiers, and XML doc comments are in **English**; some inline comments are
  in Spanish. Match the surrounding language of the file you edit.
- Utilities that adapt third-party code cite the source (StackOverflow) in the class
  XML doc — preserve those attributions.
- Tests are NUnit (`[TestFixture]` / `[Test]`); name new tests in the existing
  `Method_Scenario_ExpectedResult` style.
