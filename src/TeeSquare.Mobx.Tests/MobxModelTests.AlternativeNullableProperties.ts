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
export const NameProps = {
  firstName: types.string,
  title: types.frozen<Title>(),
  lastName: types.string,
}
export const Name = types.model('Name', {
  ...NameProps
});

export type NameInstance = Instance<typeof Name>;

export enum Audience {
  Children = 0,
  Teenagers = 1,
  YoungAdults = 2,
  Adults = 3
}
export const BookProps = {
  title: types.string,
  author: Name,
  isAvailable: types.boolean,
  firstPublished: types.Date,
  lastRevisedOn: types.maybeNull(types.Date),
  reviewedPositively: types.maybeNull(types.boolean),
  recommendedAudience: types.maybeNull(types.frozen<Audience>()),
}
export const Book = types.model('Book', {
  ...BookProps
});

export type BookInstance = Instance<typeof Book>;

