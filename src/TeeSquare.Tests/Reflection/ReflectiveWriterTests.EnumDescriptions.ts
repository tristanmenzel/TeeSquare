// Auto-generated Code - Do Not Edit

export enum DescribedEnum {
  Zero = 0,
  One = 1,
  NegativeOne = -1
}
export const AllDescribedEnum: DescribedEnum[] = [
  DescribedEnum.Zero,
  DescribedEnum.One,
  DescribedEnum.NegativeOne
];
export const DescribedEnumDesc: { [key: number]: string } = {
  0: `Zero`,
  1: `Positive One`,
  '-1': `Negative One`
};
export const GetDescribedEnumDescription = (value: DescribedEnum): string => {
  return DescribedEnumDesc[value];
};
