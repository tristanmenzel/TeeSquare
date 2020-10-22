// Auto-generated Code - Do Not Edit

import { types, Instance } from 'mobx-state-tree';
export enum Audience {
  Children = 0,
  Teenagers = 1,
  YoungAdults = 2,
  Adults = 3
}
export var BookBaseModelProps = {
  title: types.string,
  author: NameBaseModel,
  isAvailable: types.boolean,
  firstPublished: types.Date,
  lastRevisedOn: types.maybe(types.Date),
  reviewedPositively: types.maybe(types.boolean),
  recommendedAudience: types.maybe(types.frozen<Audience>()),
}
export var BookBaseModel = types.model('BookBaseModel', {
  ...BookBaseModelProps
});

export var LibraryBaseModelProps = {
  name: types.string,
  location: LocationBaseModel,
  squareMeters: types.integer,
  levels: types.maybe(types.integer),
  allBooks: types.array(BookBaseModel),
  topBorrowed: types.array(BookBaseModel),
}
export var LibraryBaseModel = types.model('LibraryBaseModel', {
  ...LibraryBaseModelProps
});

export var LocationBaseModelProps = {
  latitude: types.number,
  longitude: types.number,
}
export var LocationBaseModel = types.model('LocationBaseModel', {
  ...LocationBaseModelProps
});

export var NameBaseModelProps = {
  firstName: types.string,
  title: types.frozen<Title>(),
  lastName: types.string,
}
export var NameBaseModel = types.model('NameBaseModel', {
  ...NameBaseModelProps
});

export enum Title {
  Unknown = 0,
  Mr = 1,
  Mrs = 2,
  Miss = 3,
  Doctor = 4,
  Sir = 5,
  Madam = 6
}
