import { types, Instance } from 'mobx-state-tree';

export enum Audience {
  Children = 0,
  Teenagers = 1,
  YoungAdults = 2,
  Adults = 3
}
export enum Title {
  Unknown = 0,
  Mr = 1,
  Mrs = 2,
  Miss = 3,
  Doctor = 4,
  Sir = 5,
  Madam = 6
}
export const BookModel = types.model('BookModel',  {
  title: types.string,
  author: NameModel,
  isAvailable: types.boolean,
  firstPublished: types.string,
  lastRevisedOn: types.string,
  reviewedPositively: types.boolean,
  recommendedAudience: types.enumeration<Audience>("Audience", [Object.values(Audience)]),
});

export type Book = Instance<typeof BookModel>;
export const NameModel = types.model('NameModel',  {
  firstName: types.string,
  title: types.enumeration<Title>("Title", [Object.values(Title)]),
  lastName: types.string,
});

export type Name = Instance<typeof NameModel>;
