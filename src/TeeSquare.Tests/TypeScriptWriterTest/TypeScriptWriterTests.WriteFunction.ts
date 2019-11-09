export const HelloWorld = <T1 extends number>(a: T1): number => {
  console.log('Hey');
  return 2 * a;
};
export const HelloWorldArrows = (a: number): Array<number> => {
  console.log('Hey');
  return [2, a];
};
export const LogSomething = (a: number, b: Array<number>, c: string): void => {
  console.log(a, b, c);
};
