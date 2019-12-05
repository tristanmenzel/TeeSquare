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
export const NameModelBase = types.model('NameModelBase',  {
  firstName: types.string,
  title: types.frozen<Title>(),
  lastName: types.string,
});

export enum Audience {
  Children = 0,
  Teenagers = 1,
  YoungAdults = 2,
  Adults = 3
}
export const BookModelBase = types.model('BookModelBase',  {
  title: types.string,
  author: NameModelBase,
  isAvailable: types.boolean,
  firstPublished: types.Date,
  lastRevisedOn: types.maybe(types.Date),
  reviewedPositively: types.maybe(types.boolean),
  recommendedAudience: types.maybe(types.frozen<Audience>()),
});

export const LocationModelBase = types.model('LocationModelBase',  {
  latitude: types.number,
  longitude: types.number,
});

export const LibraryModelBase = types.model('LibraryModelBase',  {
  name: types.string,
  location: LocationModelBase,
  squareMeters: types.integer,
  levels: types.maybe(types.integer),
  allBooks: types.array(BookModelBase),
  topBorrowed: types.array(BookModelBase),
});
