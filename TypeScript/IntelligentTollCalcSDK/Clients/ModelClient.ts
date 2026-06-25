import { VehicleType } from "../Core/tollConfig";

// Interface for a model client that classifies a vehicle model string
// (e.g. "Volvo XC90") into a general VehicleType used to decide toll exemption.
export interface IModelClient {
    classifyVehicleType(vehicleModel: string): Promise<VehicleType>;
}
