import json
import logging

from flask import Response, request
from flask_restx import Namespace, Resource

from src.services.holiday_service import is_today_a_holiday_or_weekend
from src.services.tollfee_sevice import has_daily_price_quota_exceed, save, get_tollfee
from src.services.vehicle_service import get_all_vehicle
from src.utils.constant import Constant

vehicle_ns = Namespace('Vehicle', description='End points related to Vehicle')


@vehicle_ns.route('/vehicle-type')
class VehicleType(Resource):
    def get(self):
        logging.info('Fetching All the Vehicle Types')
        vehicles = get_all_vehicle()
        payload = {
            "status": 0,
            "data": vehicles
        }
        return Response(response=json.dumps(payload), status=Constant.SUCCESS,
                        mimetype=Constant.MIME_TYPE)


@vehicle_ns.route('/vehicle-in')
class VehicleIN(Resource):
    def post(self):
        logging.info('Fetching All the Vehicle Types')
        payload = request.json
        license_no = payload['license_no']

        is_holiday = is_today_a_holiday_or_weekend()

        if is_holiday or payload['is_free']:
            save(payload)
            response = {
                "status": 0,
                "message": "Vehicle is allows to park"
            }
            return Response(response=json.dumps(response), status=Constant.SUCCESS_ACCEPTED,
                            mimetype=Constant.MIME_TYPE)
        else:
            exceed = has_daily_price_quota_exceed(license_no)
            if exceed:
                response = {
                    "status": -1,
                    "message": "Vehicle is not allows to park, Daily parking quota has exceed"
                }
                return Response(response=json.dumps(response), status=Constant.UNPROCESSABLE_ENTITY,
                                mimetype=Constant.MIME_TYPE)
            else:
                save(payload)
                response = {
                    "status": 0,
                    "message": "Vehicle is allows to park"
                }
                return Response(response=json.dumps(response), status=Constant.SUCCESS_ACCEPTED,
                                mimetype=Constant.MIME_TYPE)


@vehicle_ns.route('/vehicle-exit')
class VehicleExit(Resource):
    def post(self):

        try:
            payload = request.json
            license_no = payload['license_no']

            fee = get_tollfee(license_no)
            payload = {
                "status": 0,
                "data": {
                    "fee": fee
                }
            }
            return Response(response=json.dumps(payload), status=Constant.SUCCESS,
                            mimetype=Constant.MIME_TYPE)
        except Exception as e:
            payload = {
                "status": -1,
                "message": e.args
            }
            return Response(response=json.dumps(payload), status=Constant.UNPROCESSABLE_ENTITY,
                            mimetype=Constant.MIME_TYPE)
