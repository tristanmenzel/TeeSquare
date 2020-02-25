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
export const NameBaseModelProps = {
  firstName: types.string,
  title: types.frozen<Title>(),
  lastName: types.string,
}
export const NameBaseModel = types.model('NameBaseModel', {
  ...NameBaseModelProps
});

export enum Audience {
  Children = 0,
  Teenagers = 1,
  YoungAdults = 2,
  Adults = 3
}
export const BookBaseModelProps = {
  title: types.string,
  author: NameBaseModel,
  isAvailable: types.boolean,
  firstPublished: types.Date,
  lastRevisedOn: types.maybe(types.Date),
  reviewedPositively: types.maybe(types.boolean),
  recommendedAudience: types.maybe(types.frozen<Audience>()),
}
export const BookBaseModel = types.model('BookBaseModel', {
  ...BookBaseModelProps
});

export const LocationBaseModelProps = {
  latitude: types.number,
  longitude: types.number,
}
export const LocationBaseModel = types.model('LocationBaseModel', {
  ...LocationBaseModelProps
});

export const LibraryBaseModelProps = {
  name: types.string,
  location: LocationBaseModel,
  squareMeters: types.integer,
  levels: types.maybe(types.integer),
  allBooks: types.array(BookBaseModel),
  topBorrowed: types.array(BookBaseModel),
}
export const LibraryBaseModel = types.model('LibraryBaseModel', {
  ...LibraryBaseModelProps
});

