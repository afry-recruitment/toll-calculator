"""Tests for the toll calculator.

The date in the fee-schedule tests is arbitrary: fee_for_time only
reads the time of day. The date tests further down use specific dates
on purpose.
"""


from datetime import datetime

from toll_calculator import (
    VehicleType,
    calculate_daily_fee,
    fee_for_time,
    is_toll_free_date,
)


# --- Fee schedule ---


def test_opening_interval_costs_8():
    assert fee_for_time(datetime(2026, 8, 31, 6, 0)) == 8


def test_morning_light_traffic_0630_costs_13():
    assert fee_for_time(datetime(2026, 8, 31, 6, 30)) == 13


def test_morning_rush_0700_costs_18():
    assert fee_for_time(datetime(2026, 8, 31, 7, 0)) == 18


def test_morning_rush_0730_costs_18():
    assert fee_for_time(datetime(2026, 8, 31, 7, 30)) == 18



def test_late_morning_0800_costs_13():
    assert fee_for_time(datetime(2026, 8, 31, 8, 0)) == 13


def test_midday_traffic_0830_costs_8():
    assert fee_for_time(datetime(2026, 8, 31, 8, 30)) == 8


def test_afternoon_light_traffic_1500_costs_13():
    assert fee_for_time(datetime(2026, 8, 31, 15, 0)) == 13


def test_afternoon_rush_1530_costs_18():
    assert fee_for_time(datetime(2026, 8, 31, 15, 30)) == 18


def test_evening_light_traffic_1700_costs_13():
    assert fee_for_time(datetime(2026, 8, 31, 17, 0)) == 13


def test_evening_closing_1800_costs_8():
    assert fee_for_time(datetime(2026, 8, 31, 18, 0)) == 8


def test_no_fee_after_closing_1830():
    assert fee_for_time(datetime(2026, 8, 31, 18, 30)) == 0


def test_no_fee_at_night_2200():
    assert fee_for_time(datetime(2026, 8, 31, 22, 0)) == 0


def test_0915_is_charged_not_free():
    assert fee_for_time(datetime(2026, 8, 31, 9, 15)) == 8


def test_seconds_within_interval_are_charged():
    assert fee_for_time(datetime(2026, 8, 31, 6, 29, 45)) == 8


# --- Toll-free dates ---


def test_saturday_is_toll_free():
    assert is_toll_free_date(datetime(2026, 8, 29, 12, 0)) is True


def test_monday_is_not_toll_free():
    assert is_toll_free_date(datetime(2026, 8, 31, 12, 0)) is False


def test_langfredagen_is_toll_free():
    assert is_toll_free_date(datetime(2026, 4, 3, 12, 0)) is True


def test_juldagen_is_toll_free():
    assert is_toll_free_date(datetime(2026, 12, 25, 12, 0)) is True


# --- Daily fee ---


def test_single_passage_is_charged_once():
    passages = [datetime(2026, 8, 31, 7, 30)]
    assert calculate_daily_fee(VehicleType.CAR, passages) == 18


def test_two_passages_within_an_hour_charge_the_highest():
    passages = [
        datetime(2026, 8, 31, 6, 15),
        datetime(2026, 8, 31, 6, 45),
    ]
    assert calculate_daily_fee(VehicleType.CAR, passages) == 13


def test_passages_in_separate_hours_are_added():
    passages = [
        datetime(2026, 8, 31, 6, 15),
        datetime(2026, 8, 31, 9, 0),
    ]
    assert calculate_daily_fee(VehicleType.CAR, passages) == 16


def test_hour_window_starts_at_first_passage():
    passages = [
        datetime(2026, 8, 31, 6, 15),
        datetime(2026, 8, 31, 6, 45),
        datetime(2026, 8, 31, 7, 10),
        datetime(2026, 8, 31, 9, 0),
    ]
    assert calculate_daily_fee(VehicleType.CAR, passages) == 26


def test_daily_fee_is_capped_at_60():
    passages = [
        datetime(2026, 8, 31, 6, 30),
        datetime(2026, 8, 31, 7, 30),
        datetime(2026, 8, 31, 15, 30),
        datetime(2026, 8, 31, 17, 0),
    ]
    assert calculate_daily_fee(VehicleType.CAR, passages) == 60


def test_unsorted_passages_give_the_same_result():
    passages = [
        datetime(2026, 8, 31, 9, 0),
        datetime(2026, 8, 31, 6, 15),
    ]
    assert calculate_daily_fee(VehicleType.CAR, passages) == 16


def test_toll_free_vehicle_pays_nothing():
    passages = [datetime(2026, 8, 31, 7, 30)]
    assert calculate_daily_fee(VehicleType.MOTORBIKE, passages) == 0


def test_no_passages_costs_nothing():
    assert calculate_daily_fee(VehicleType.CAR, []) == 0


def test_weekend_costs_nothing():
    passages = [datetime(2026, 8, 29, 7, 30)]
    assert calculate_daily_fee(VehicleType.CAR, passages) == 0






