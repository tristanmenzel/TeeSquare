// Auto-generated Code - Do Not Edit

import { types, Instance } from 'mobx-state-tree';

export enum Title {
  Unknown = 0,
  Mr = 1,
  Mrs = 2,
  Miss = 3,
  Doctor = 4,
  Sir = 5,
  Madam = 6
}
export const NameModel = types.model('NameModel',  {
  firstName: types.string,
  title: types.frozen<Title>(),
  lastName: types.string,
});

export type Name = Instance<typeof NameModel>;
export enum Audience {
  Children = 0,
  Teenagers = 1,
  YoungAdults = 2,
  Adults = 3
}
export const BookModel = types.model('BookModel',  {
  title: types.string,
  author: NameModel,
  isAvailable: types.boolean,
  firstPublished: types.Date,
  lastRevisedOn: types.maybe(types.Date),
  reviewedPositively: types.maybe(types.boolean),
  recommendedAudience: types.maybe(types.frozen<Audience>()),
});

export type Book = Instance<typeof BookModel>;
