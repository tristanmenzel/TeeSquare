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

export type NameInstance = Instance<typeof NameModel>;

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

export type BookInstance = Instance<typeof BookModel>;

export const LocationModel = types.model('LocationModel',  {
  latitude: types.number,
  longitude: types.number,
});

export type LocationInstance = Instance<typeof LocationModel>;

export const LibraryModel = types.model('LibraryModel',  {
  name: types.string,
  location: LocationModel,
  squareMeters: types.integer,
  levels: types.maybe(types.integer),
  allBooks: types.array(BookModel),
  topBorrowed: types.array(BookModel),
});

export type LibraryInstance = Instance<typeof LibraryModel>;
