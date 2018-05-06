export interface FunFunInterface<TOne, TTwo> {
  ValueOfTwo: TTwo;
  MaybeItsOne: Maybe<TOne>;
  TestMethod(a: number, b: Enumerable<number>): string;
}