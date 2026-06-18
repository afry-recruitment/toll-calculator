export const TOLLABLE_VEHICLES = ['Car', 'Van', 'Truck'] as const
export const TOLL_FREE_VEHICLES = ['Motorbike', 'Tractor', 'Emergency', 'Diplomat', 'Foreign', 'Military'] as const

export type TollableVehicle = typeof TOLLABLE_VEHICLES[number]
export type TollFreeVehicle = typeof TOLL_FREE_VEHICLES[number]
export type Vehicle = TollableVehicle | TollFreeVehicle
