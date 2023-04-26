import logging

from flask_restx import Namespace, Resource
from flask import jsonify

echo_ns = Namespace('Echo', description='End point for checking the API')


@echo_ns.route('/echo')
class Echo(Resource):
    def get(self):
        logging.debug('Echo...')
        return jsonify({"message": "hello"})
