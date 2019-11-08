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
