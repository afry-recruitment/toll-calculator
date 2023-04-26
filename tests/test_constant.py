from src.utils.constant import Constant


def test_mime_type():
    assert Constant.MIME_TYPE == 'application/json'


def test_bad_request():
    assert Constant.BAD_REQUEST == 400


def test_unauthorized():
    assert Constant.UNAUTHORIZED == 401


def test_internal_server_error():
    assert Constant.INTERNAL_SERVER_ERROR == 500


def test_success():
    assert Constant.SUCCESS == 200


def test_unprocessable_entity():
    assert Constant.UNPROCESSABLE_ENTITY == 422


def test_success_create():
    assert Constant.SUCCESS_CREATE == 201


def test_success_accepted():
    assert Constant.SUCCESS_ACCEPTED == 202
