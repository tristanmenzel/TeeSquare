// Auto-generated Code - Do Not Edit

import { types, Instance } from 'mobx-state-tree';
export var LocationProps = {
  latitude: types.number,
  longitude: types.number,
}
export var Location = types.model('Location', {
  ...LocationProps
});

export type LocationInstance = Instance<typeof Location>;

