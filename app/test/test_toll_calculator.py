from datetime import datetime
import pytest

from app.toll_calculator import get_toll_fee


@pytest.mark.parametrize(
    "dates,expected",
    [
        ([datetime(2024, 5, 13, 6, 0, 0)], 8),
        ([datetime(2024, 5, 13, 6, 29, 0)], 8),
        ([datetime(2024, 5, 13, 6, 30, 0)], 13),
        ([datetime(2024, 5, 13, 6, 59, 0)], 13),
        ([datetime(2024, 5, 13, 7, 0, 0)], 18),
        ([datetime(2024, 5, 13, 7, 59, 0)], 18),
        ([datetime(2024, 5, 13, 8, 0, 0)], 13),
        ([datetime(2024, 5, 13, 8, 29, 0)], 13),
        ([datetime(2024, 5, 13, 8, 30, 0)], 8),
        ([datetime(2024, 5, 13, 9, 0, 0)], 0),
        ([datetime(2024, 5, 13, 9, 29, 0)], 0),
        ([datetime(2024, 5, 13, 9, 30, 0)], 8),
        ([datetime(2024, 5, 13, 9, 59, 0)], 8),
        ([datetime(2024, 5, 13, 10, 0, 0)], 0),
        ([datetime(2024, 5, 13, 10, 29, 0)], 0),
        ([datetime(2024, 5, 13, 10, 30, 0)], 8),
        ([datetime(2024, 5, 13, 10, 59, 0)], 8),
        ([datetime(2024, 5, 13, 11, 0, 0)], 0),
        ([datetime(2024, 5, 13, 11, 29, 0)], 0),
        ([datetime(2024, 5, 13, 11, 30, 0)], 8),
        ([datetime(2024, 5, 13, 11, 59, 0)], 8),
        ([datetime(2024, 5, 13, 12, 0, 0)], 0),
        ([datetime(2024, 5, 13, 12, 29, 0)], 0),
        ([datetime(2024, 5, 13, 12, 30, 0)], 8),
        ([datetime(2024, 5, 13, 12, 59, 0)], 8),
        ([datetime(2024, 5, 13, 13, 0, 0)], 0),
        ([datetime(2024, 5, 13, 13, 29, 0)], 0),
        ([datetime(2024, 5, 13, 13, 30, 0)], 8),
        ([datetime(2024, 5, 13, 13, 59, 0)], 8),
        ([datetime(2024, 5, 13, 14, 0, 0)], 0),
        ([datetime(2024, 5, 13, 14, 29, 0)], 0),
        ([datetime(2024, 5, 13, 14, 30, 0)], 8),
        ([datetime(2024, 5, 13, 14, 59, 0)], 8),
        ([datetime(2024, 5, 13, 15, 0, 0)], 13),
        ([datetime(2024, 5, 13, 15, 30, 0)], 18),
        ([datetime(2024, 5, 13, 16, 59, 0)], 18),
        ([datetime(2024, 5, 13, 17, 0, 0)], 13),
        ([datetime(2024, 5, 13, 17, 59, 0)], 13),
        ([datetime(2024, 5, 13, 18, 0, 0)], 8),
        ([datetime(2024, 5, 13, 18, 29, 0)], 8),
        ([datetime(2024, 5, 13, 18, 30, 0)], 0),
        ([datetime(2024, 5, 14, 5, 59, 0)], 0),
    ],
)
def test_fees_by_time_of_day(dates, expected):
    assert get_toll_fee("Car", dates) == expected


def test_sum_of_fees():
    assert (
        get_toll_fee(
            "Car",
            [
                datetime(2024, 5, 13, 14, 59, 0),
                datetime(2024, 5, 13, 15, 0, 0),
                datetime(2024, 5, 13, 16, 59, 0),
            ],
        )
        == 39
    )


def test_sum_of_fees_multiple_days():
    assert (
        get_toll_fee(
            "Car",
            [
                datetime(2024, 5, 13, 14, 59, 0),
                datetime(2024, 5, 13, 15, 0, 0),
                datetime(2024, 5, 13, 16, 59, 0),
                datetime(2024, 5, 14, 6, 30, 0),
                datetime(2024, 5, 14, 7, 59, 0),
                datetime(2024, 5, 14, 14, 59, 0),
                datetime(2024, 5, 14, 15, 0, 0),
                datetime(2024, 5, 14, 16, 59, 0),
            ],
        )
        == 99
    )


def test_maximum_fee_is_60sek():
    assert (
        get_toll_fee(
            "Car",
            [
                datetime(2024, 5, 13, 6, 30, 0),
                datetime(2024, 5, 13, 7, 59, 0),
                datetime(2024, 5, 13, 14, 59, 0),
                datetime(2024, 5, 13, 15, 0, 0),
                datetime(2024, 5, 13, 16, 59, 0),
            ],
        )
        == 60
    )


def test_only_one_highest_fee_per_hour():
    assert (
        get_toll_fee(
            "Car", [datetime(2024, 5, 13, 15, 0, 0), datetime(2024, 5, 13, 15, 30, 0)]
        )
        == 18
    )


@pytest.mark.parametrize(
    "vehicle",
    [
        "Motorbike",
        "Tractor",
        "Emergency",
        "Diplomat",
        "Foreign",
        "Military",
    ],
)
def test_fee_free_vehicle(vehicle):
    assert get_toll_fee(vehicle, [datetime(2024, 5, 13, 6, 0, 0)]) == 0


@pytest.mark.parametrize(
    "dates",
    [
        [datetime(2024, 5, 11, 17, 0, 0)],  # weekend
        [datetime(2024, 5, 12, 17, 0, 0)],  # weekend
        [datetime(2024, 5, 9, 17, 0, 0)],  # holiday
    ],
)
def test_fee_free_day(dates):
    assert get_toll_fee("Car", dates) == 0
