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
  title: types.enumeration<Title>("Title", [Object.values(Title)]),
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
  firstPublished: types.string,
  lastRevisedOn: types.maybe(types.string),
  reviewedPositively: types.maybe(types.boolean),
  recommendedAudience: types.maybe(types.enumeration<Audience>("Audience", [Object.values(Audience)])),
});

export type Book = Instance<typeof BookModel>;
export const LocationModel = types.model('LocationModel',  {
  latitude: types.number,
  longitude: types.number,
});

export type Location = Instance<typeof LocationModel>;
export const LibraryModel = types.model('LibraryModel',  {
  name: types.string,
  location: LocationModel,
  squareMeters: types.number,
  levels: types.maybe(types.integer),
  allBooks: types.array(BookModel),
  topBorrowed: types.array(BookModel),
});

export type Library = Instance<typeof LibraryModel>;
