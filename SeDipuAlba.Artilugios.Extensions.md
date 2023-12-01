## `ExceptionExtensions`

Extensions for handling exceptions
```csharp
public static class SeDipuAlba.Artilugios.Extensions.ExceptionExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `IEnumerable<Exception>` | GetInnerExceptions(this `Exception` ex) | Returns all internal exceptions of an exception in a list | 
| `String` | GetInnerExceptionsConcatMessage(this `Exception` ex, `String` joinSymbol =  => ) | Returns exception messages concatenated with a symbol | 
| `IEnumerable<String>` | GetInnerExceptionsMessages(this `Exception` ex) | Returns exception messages in a list | 


## `TextExtensions`

Extensions for handling string texts
```csharp
public static class SeDipuAlba.Artilugios.Extensions.TextExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | RemoveDiacritics(this `String` text) | Removes diacritics from texts. Example: "En España está un pingüino" becomes "En Espana esta un pinguino" | 
| `String` | RemoveSymbols(this `String` txt, `String` alphabet = ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ) | Devuelve sólo los caracteres indicados en el alfabeto | 


## `UriExtensions`

Extensions for Uri class
```csharp
public static class SeDipuAlba.Artilugios.Extensions.UriExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Uri` | AddParameter(this `Uri` url, `String` paramName, `String` paramValue, `Boolean` urlEncode = True) | Adds the specified parameter to the Query String.  Based on Brinkie code: https://stackoverflow.com/a/19679135/2126607 | 


