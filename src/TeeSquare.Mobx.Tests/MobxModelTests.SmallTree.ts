// Auto-generated Code - Do Not Edit

import { types, Instance } from 'mobx-state-tree';
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
