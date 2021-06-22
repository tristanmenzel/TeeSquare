// Auto-generated Code - Do Not Edit

export interface Circle {
  radius: number;
  kind: 'Circle';
}
export interface Rectangle {
  width: number;
  height: number;
  kind: 'Rectangle';
}
export type Shape = Circle | Square | Rectangle;
export interface Square {
  side: number;
  kind: 'Square';
}
