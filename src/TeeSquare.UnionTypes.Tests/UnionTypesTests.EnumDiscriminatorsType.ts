// Auto-generated Code - Do Not Edit

export type Field = NumericField | StringField;
export enum FieldType {
  String = 0,
  Number = 1
}
export interface NumericField {
  type: FieldType.Number;
  value: number;
}
export interface StringField {
  type: FieldType.String;
  value: string;
}
