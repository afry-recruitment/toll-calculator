from src.model.tollfee import TollFee


def get_tollfee_by_vehicle_type(vehicle_type_code='DEFAULT'):
    toll_fee = TollFee.objects(vehicle_type_code=vehicle_type_code).exclude('_id')
    result = {}
    for fee in toll_fee:
        fee_dict = {
            fee.hour_in_24h: fee.fee
        }
        result.update(fee_dict)

    return result
