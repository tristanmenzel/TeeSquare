export enum Audience {
  Children = 0,
  Teenagers = 1,
  YoungAdults = 2,
  Adults = 3
}
export const AllAudience: Audience[] = [
  Audience.Children,
  Audience.Teenagers,
  Audience.YoungAdults,
  Audience.Adults
];
export const AudienceDesc: { [key: number]: string } = {
  0: `Children`,
  1: `Teenagers`,
  2: `Young Adults`,
  3: `Adults`
};
export const GetAudienceDescription = (value: Audience): string => {
  return AudienceDesc[value];
};
