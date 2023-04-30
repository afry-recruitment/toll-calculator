import json

from flask import Response

from src.utils.constant import Constant


def get_error_response(error_message, response_data=None, body_status=None, http_status=None):
    response_body = {
        'status': Constant.RES_BODY_FAILURE_STATUS if not body_status else body_status,
        'message': error_message,
    }
    if response_data:
        for data in response_data:
            response_body.update(data)
    return Response(
        json.dumps(response_body),
        status=Constant.INTERNAL_SERVER_ERROR if not http_status else http_status,
        mimetype=Constant.MIME_TYPE
    )


def get_success_response(http_status, message=None, response_data=None):
    response_body = {
        'status': Constant.RES_BODY_SUCCESS_STATUS,
        'message': Constant.RES_BODY_SUCCESS_MSG,
    }
    if message:
        response_body.update({"message": message})
    if response_data:
        for data in response_data:
            response_body.update(data)
    return Response(json.dumps(response_body),
                    status=http_status,
                    mimetype=Constant.MIME_TYPE)
