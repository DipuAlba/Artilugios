## `Crypto`

Crypto library to encrypt/decrypt string texts  Based on Brett code: https://stackoverflow.com/a/2791259/2126607
```csharp
public class SeDipuAlba.Artilugios.Crypto

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | DecryptStringAES(`String` cipherText, `String` sharedSecret, `String` salt) | Decrypts a string that was encrypted using EncryptStringAES() and the same shared secret. | 
| `String` | DecryptStringAES(`String` cipherText, `String` sharedSecret, `Byte[]` salt) | Decrypts a string that was encrypted using EncryptStringAES() and the same shared secret. | 
| `String` | EncryptStringAES(`String` plainText, `String` sharedSecret, `String` salt) | Encrypts a string using AES with a shared secret. The encrypted string can be decrypted using DecryptStringAES(). | 
| `String` | EncryptStringAES(`String` plainText, `String` sharedSecret, `Byte[]` salt) | Encrypts a string using AES with a shared secret. The encrypted string can be decrypted using DecryptStringAES(). | 


## `Guard`

Validation class
```csharp
public static class SeDipuAlba.Artilugios.Guard

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | IsBetweenExclusive(`Int32` value, `Int32` min, `Int32` max, `String` paramName) | Ensures that the value of a parameter is between exclusive bounds. | 


## `HashedPassword`

Password generator for WsSegPass SEDIPUALB@  https://pre.sedipualba.es/hashedpassword/
```csharp
public class SeDipuAlba.Artilugios.HashedPassword

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Generate(`Nullable<DateTime>` time = null) | Generate password | 


## `StringMask`

Hides parts of a string with a specified character.  Based on Assil code: https://stackoverflow.com/a/45056499/2126607
```csharp
public class SeDipuAlba.Artilugios.StringMask

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `StringMask` | ShowFirst(`Int32` number) | Shows the first [number] of characters and masks the rest. | 
| `StringMask` | ShowLast(`Int32` number) | Shows the last [number] of characters and masks the rest. | 
| `String` | ToString() | Returns a `System.String` that represents this instance. | 


