import logging

from flask_restx import Namespace, Resource
from flask import jsonify

tollfee_ns = Namespace('Toll Fee', description='End points related to the toll fees')


@tollfee_ns.route('/toll-fee')
class TollFee(Resource):
    def get(self):
        logging.debug('Get toll fee...')
        return jsonify({"message": "get toll fee"})
