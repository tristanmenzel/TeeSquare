// Auto-generated Code - Do Not Edit

import { types, Instance } from 'mobx-state-tree';
export enum Audience {
  Children = 0,
  Teenagers = 1,
  YoungAdults = 2,
  Adults = 3
}
export var BookProps = {
  title: types.string,
  author: Name,
  isAvailable: types.boolean,
  firstPublished: types.Date,
  lastRevisedOn: types.maybe(types.Date),
  reviewedPositively: types.maybe(types.boolean),
  recommendedAudience: types.maybe(types.frozen<Audience>()),
}
export var Book = types.model('Book', {
  ...BookProps
});

export type BookInstance = Instance<typeof Book>;

export var LibraryProps = {
  name: types.string,
  location: Location,
  squareMeters: types.integer,
  levels: types.maybe(types.integer),
  allBooks: types.array(Book),
  topBorrowed: types.array(Book),
}
export var Library = types.model('Library', {
  ...LibraryProps
});

export type LibraryInstance = Instance<typeof Library>;

export var LocationProps = {
  latitude: types.number,
  longitude: types.number,
}
export var Location = types.model('Location', {
  ...LocationProps
});

export type LocationInstance = Instance<typeof Location>;

export var NameProps = {
  firstName: types.string,
  title: types.frozen<Title>(),
  lastName: types.string,
}
export var Name = types.model('Name', {
  ...NameProps
});

export type NameInstance = Instance<typeof Name>;

export enum Title {
  Unknown = 0,
  Mr = 1,
  Mrs = 2,
  Miss = 3,
  Doctor = 4,
  Sir = 5,
  Madam = 6
}
