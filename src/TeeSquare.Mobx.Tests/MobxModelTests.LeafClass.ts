// Auto-generated Code - Do Not Edit

import { types, Instance } from 'mobx-state-tree';
export const LocationProps = {
  latitude: types.number,
  longitude: types.number,
}
export const Location = types.model('Location', {
  ...LocationProps
});

export type LocationInstance = Instance<typeof Location>;

