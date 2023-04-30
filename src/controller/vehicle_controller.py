import logging

from flask import request
from flask_restx import Namespace, Resource, fields

from src.services.holiday_service import is_today_a_holiday_or_weekend
from src.services.tollfee_record_sevice import has_daily_price_quota_exceed, save, get_tollfee
from src.services.tollfee_record_sevice import has_open_toll_records
from src.services.vehicle_service import get_all_vehicle
from src.utils.constant import Constant
from src.utils.responses import get_error_response
from src.utils.responses import get_success_response

vehicle_ns = Namespace('Vehicle', description='End points related to Vehicle')


@vehicle_ns.route('/vehicle-type')
class VehicleType(Resource):
    def get(self):
        logging.info('Fetching all the config vehicle types')
        vehicles = get_all_vehicle()
        return get_success_response(http_status=Constant.SUCCESS, response_data=[{"data": vehicles}])


@vehicle_ns.route('/vehicle-in')
class VehicleIN(Resource):
    vehicle_in_request_schema = vehicle_ns.model('Vehicle In Entity Request', {
        'license_no': fields.String(required=True),
        'vehicle_type': fields.String(required=True),
        'vehicle_code': fields.String(required=True),
        'is_free': fields.Boolean(required=True)
    })

    @vehicle_ns.expect(vehicle_in_request_schema, validate=True)
    def post(self):

        try:
            payload = request.json
            logging.info('Vehicle In with %s', payload)
            license_no = payload['license_no']

            is_holiday = is_today_a_holiday_or_weekend()
            has_open_toll_records(license_no)

            if is_holiday or payload['is_free']:
                save(payload)
                logging.info('Vehicle %s is allows to park fee of charge', license_no)
                return get_success_response(http_status=Constant.SUCCESS_ACCEPTED,
                                            message="Vehicle is allows to park fee of charge")
            else:
                exceed = has_daily_price_quota_exceed(license_no)
                if exceed:
                    logging.error('Vehicle %s is not allows to park, Daily parking quota has exceed', license_no)
                    raise ValueError("Vehicle is not allows to park, Daily parking quota has exceed")
                else:
                    save(payload)
                    logging.info('Vehicle %s is allows to park', license_no)
                    return get_success_response(http_status=Constant.SUCCESS_ACCEPTED,
                                                message="Vehicle is allows to park")
        except ValueError as e:
            logging.error(e)
            return get_error_response(error_message=e.args, http_status=Constant.UNPROCESSABLE_ENTITY)
        except Exception as e:
            logging.error(e)
            return get_error_response(error_message=e.args)


@vehicle_ns.route('/vehicle-exit')
class VehicleExit(Resource):
    vehicle_exit_request_schema = vehicle_ns.model('Vehicle Exit Entity Request', {
        'license_no': fields.String(required=True),
    })

    @vehicle_ns.expect(vehicle_exit_request_schema, validate=True)
    def post(self):

        try:
            payload = request.json
            logging.info('Vehicle Exit with %s', payload)
            license_no = payload['license_no']

            fee = get_tollfee(license_no)
            logging.info('Vehicle %s Toll fee is :', license_no, fee)
            return get_success_response(http_status=Constant.SUCCESS, response_data=[{"toll_fee": fee}])
        except ValueError as e:
            logging.error(e)
            return get_error_response(error_message=e.args, http_status=Constant.UNPROCESSABLE_ENTITY)
        except Exception as e:
            logging.error(e)
            return get_error_response(error_message=e.args)
