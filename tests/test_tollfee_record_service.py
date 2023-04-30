from datetime import datetime

from src.services.tollfee_record_sevice import calculate_toll_fee

hourly_rates = {0: 8.0, 1: 8.0, 2: 8.0, 3: 8.0, 4: 8.0, 5: 8.0, 6: 12.0, 7: 18.0, 8: 18.0, 9: 16.0, 10: 15.0,
                11: 16.0, 12: 18.0, 13: 18.0, 14: 15.0, 15: 15.0, 16: 10.0, 17: 12.0, 18: 18.0, 19: 18.0, 20: 8.0,
                21: 8.0, 22: 8.0, 23: 8.0}


def test_calculate_toll_fee_no_days():
    start_time = datetime(2022, 1, 1, 8, 0, 0)
    end_time = datetime(2022, 1, 1, 10, 0, 0)
    assert calculate_toll_fee(start_time, end_time, hourly_rates) == 34.0


def test_calculate_toll_fee_one_day():
    start_time = datetime(2022, 1, 1, 8, 0, 0)
    end_time = datetime(2022, 1, 2, 10, 0, 0)
    assert calculate_toll_fee(start_time, end_time, hourly_rates) == 94.0


def test_calculate_toll_fee_multiple_days():
    start_time = datetime(2022, 1, 1, 8, 0, 0)
    end_time = datetime(2022, 1, 5, 10, 0, 0)
    assert calculate_toll_fee(start_time, end_time, hourly_rates) == 274.0


def test_calculate_toll_fee_rounding():
    start_time = datetime(2022, 1, 1, 8, 0, 0)
    end_time = datetime(2022, 1, 1, 11, 30, 0)
    assert calculate_toll_fee(start_time, end_time, hourly_rates) == 60.0


def test_calculate_toll_fee_rate_max():
    start_time = datetime(2022, 1, 1, 8, 0, 0)
    end_time = datetime(2022, 1, 1, 9, 30, 0)
    assert calculate_toll_fee(start_time, end_time, hourly_rates) == 34.0
