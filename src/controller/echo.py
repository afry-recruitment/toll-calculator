import logging
from datetime import datetime

from flask_restx import Namespace, Resource

from src.utils.constant import Constant
from src.utils.responses import get_success_response

echo_ns = Namespace('Echo', description='End point for checking the health of the API')


@echo_ns.route('/echo')
class Echo(Resource):
    def get(self):
        logging.debug('Echo...')
        return get_success_response(http_status=Constant.SUCCESS,
                                    response_data=[{"message": "hello from toll calculator",
                                                    "timestamp": str(datetime.now())}])
