class Constant:
    MIME_TYPE = 'application/json'

    # Responce Status
    BAD_REQUEST = 400
    UNAUTHORIZED = 401
    INTERNAL_SERVER_ERROR = 500
    SUCCESS = 200
    UNPROCESSABLE_ENTITY = 422

    SUCCESS_CREATE = 201
    SUCCESS_ACCEPTED = 202

    # Response message common
    RES_BODY_SUCCESS_MSG = 'success'
    RES_BODY_SUCCESS_STATUS = 0
    RES_BODY_FAILURE_STATUS = -1

    MAX_TOLL_FEE_PRE_DAY = 60
