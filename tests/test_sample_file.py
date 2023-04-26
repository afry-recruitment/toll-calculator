# test_my_module.py
import pytest

from tests.sample_file import add_numbers


def test_add_numbers():
    assert add_numbers(2, 3) == 5
    assert add_numbers(-1, 1) == 0
    assert add_numbers(0, 0) == 0


def test_add_numbers_raises_error():
    with pytest.raises(TypeError):
        add_numbers("2", 3)
