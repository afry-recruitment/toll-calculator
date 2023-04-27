from src.model.vehicle import Vehicle


def get_all_vehicle():
    vehicles = Vehicle.objects.exclude('_id')
    result = []
    for vehicle in vehicles:
        vehicle_dict = {
            "vehicle_type": vehicle.vehicle_type,
            "vehicle_code": vehicle.vehicle_code,
            "is_free": vehicle.is_free,
            "description": vehicle.description
        }
        result.append(vehicle_dict)

    return result
