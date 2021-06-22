# TeeSquare

## TeeSquare Base Library

A fluent API for writing formatted typescript code as well as a reflective writer for emitting typescript equivalents of dotnet types.

For documentation, see the unit tests

[Code Writer](https://github.com/tristanmenzel/TeeSquare/blob/master/src/TeeSquare.Tests/CodeWriterTests/CodeWriterTests.cs)
 - Low level writer which provides some assistance with indenting

[Type Script Writer](https://github.com/tristanmenzel/TeeSquare/blob/master/src/TeeSquare.Tests/TypeScriptWriterTest/TypeScriptWriterTests.cs)
 - Fluent abstraction for building types
 
[Reflective Writer](https://github.com/tristanmenzel/TeeSquare/blob/master/src/TeeSquare.Tests/Reflection/ReflectiveWriterTests.cs)
 - Reflects dotnet types and outputs typescript versions
 
## TeeSquare WebApi

Additional utilities for reflecting webapi routes and outputting a `RequestFactory` with methods for each api that is discovered

[RouteReflector](https://github.com/tristanmenzel/TeeSquare/tree/master/src/TeeSquare.WebApi.Tests)

## TeeSquare MobX

Extensions of the base library for outputting MobX State Tree models

[MobX](https://github.com/tristanmenzel/TeeSquare/blob/master/src/TeeSquare.Mobx.Tests/MobxModelTests.cs)

## TeeSquare UnionTypes

Adds ability for TeeSquare to generate union types from dotnet types when decorated with the provided attribute

 - Union types should be expressed with a common base type in c#
 - The base type should be decorated with the `[UnionType]` attribute and include the types of every possible value in the constructor
 - Types from a union should have a discriminator property which can be defined with a `public const` field. Eg. `public const string Kind = "Banana";` 

[UnionTypes](https://github.com/tristanmenzel/TeeSquare/blob/master/src/TeeSquare.UnionTypes.Tests/UnionTypesTests.cs)



 


