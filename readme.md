# TeeSquare

A fluent API for writing formatted typescript code. 

## Writing an interface

```cs
using (var writer = new TypeScriptWriter(File.Open("demo.ts", FileMode.OpenOrCreate)))
{
    writer.WriteInterface("FunFunInterface", "TOne", "TTwo")
        .With(i =>
        {
            i.Method("TestMethod")
                .WithReturnType("string")
                .WithParams(p =>
                {
                    p.Param("a", "number");
                    p.Param("b", "Enumerable", "number");
                });
            i.Property("ValueOfTwo", "TTwo");
            i.Property("MaybeItsOne", "Maybe", "TOne");
        });
}
```
**Output**
```ts
export interface FunFunInterface<TOne, TTwo> {
  ValueOfTwo: TTwo;
  MaybeItsOne: Maybe<TOne>;
  TestMethod( a: number, b: Enumerable<number>): string;
}
```

## Writing a class

```cs
using (var writer = new TypeScriptWriter(File.Open("demo.ts", FileMode.OpenOrCreate)))
{
    writer.WriteClass("AppleService")
        .With(c =>
        {
            c.Property("banana", "string");
            c.Method("getApplePie")
                .WithReturnType("string")
                .WithParams(p =>
                {
                    p.Param("numApples", "number");
                    p.Param("typeOfApple", "string");
                })
                .WithBody(x => { x.WriteLine("return \"No apples here\";"); });
            c.Method("haveFun")
                .WithParams(p => { p.Param("amountOfFun", "number"); })
                .Static()
                .WithBody(x => x.WriteLine("console.log(\"Having so much fun\", amountOfFun);"));
        });
}
```

**Output**
```ts
export class AppleService {
  banana: string;
  getApplePie(numApples: number, typeOfApple: string): string {
    return "No apples here";
  }
  static haveFun(amountOfFun: number): void {
    console.log("Having so much fun", amountOfFun);
  }
}
```

## Writing an enum

You can optional provide descriptions for enum values and they will be written as an object map such that you can access the value by going `MyEnumDesc[MyEnum.SomeValue]`

```cs
using (var writer = new TypeScriptWriter(File.Open("demo.ts", FileMode.OpenOrCreate)))
{
    writer.WriteEnum("Fruits")
        .WithValues(e =>
        {
            e.Value("Apple", 0);
            e.Value("Banana", 1);
            e.Value("Cantelope", 2);
        });

    writer.WriteEnum("Things")
        .WithValues(e =>
        {
            e.Value("ThingOne", "ValueOne", "Description of thing one");
            e.Value("ThingTwo", "ValueTwo", "Description of thing two");
            e.Value("ThingThree", "ValueThree", "Description of thing three");
        });
}
```
**Output**
```ts
export enum Fruits {
  Apple = 0,
  Banana = 1,
  Cantelope = 2
}
export enum Things {
  ThingOne = "ValueOne",
  ThingTwo = "ValueTwo",
  ThingThree = "ValueThree"
}
export const ThingsDesc = {
  "ValueOne": `Description of thing one`,
  "ValueTwo": `Description of thing two`,
  "ValueThree": `Description of thing three`
}
```


