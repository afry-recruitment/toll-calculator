import pytest
from datetime import datetime as dt
from unittest.mock import patch, mock_open
import json

from toll_calculator import TollCalculator
from car import Car
from motorbike import Motorbike


class TestIsTollFreeVehicle:
    def setup_method(self):
        self.calc = TollCalculator()
    
    def test_motorbike_is_toll_free(self):
        assert self.calc.is_toll_free_vehicle(Motorbike()) is True

    def test_car_is_not_toll_free(self):
        assert self.calc.is_toll_free_vehicle(Car()) is False

    def test_none_returns_false(self):
        assert self.calc.is_toll_free_vehicle(None) is False


class TestIsTollFreeDate:
    def setup_method(self):
        self.calc = TollCalculator()

    def test_sunday_is_toll_free(self):
        assert self.calc.is_toll_free_date(dt(2026, 6, 14)) is True

    def test_weekday_is_not_toll_free(self):
        assert self.calc.is_toll_free_date(dt(2026, 6, 15)) is False
    
    def test_swedish_holiday_is_toll_free(self):
        # Kristi himmelsfärd
        assert self.calc.is_toll_free_date(dt(2026, 5, 14)) is True
    
    def test_christmas_day_is_toll_free(self):
        assert self.calc.is_toll_free_date(dt(2026, 12, 25)) is True


class TestGetTollFeeForPass:
    def setup_method(self):
        self.calc = TollCalculator()
        self.car = Car()
    
    def test_0600_to_0629_returns_8(self):
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 6, 0)) == 8
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 6, 29)) == 8

    def test_0630_to_0659_returns_13(self):
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 6, 30)) == 13
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 6, 59)) == 13
        
    def test_0700_to_0759_returns_18(self):
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 7, 0)) == 18
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 7, 59)) == 18
        
    def test_0800_to_0829_returns_13(self):
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 8, 0)) == 13
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 8, 29)) == 13

    def test_0830_to_1459_returns_8(self):
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 8, 30)) == 8
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 14, 59)) == 8
 
    def test_1500_to_1529_returns_13(self):
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 15, 0)) == 13
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 15, 29)) == 13
 
    def test_1530_to_1659_returns_18(self):
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 15, 30)) == 18
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 16, 59)) == 18
 
    def test_1700_to_1759_returns_13(self):
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 17, 0)) == 13
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 17, 59)) == 13
 
    def test_1800_to_1829_returns_8(self):
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 18, 0)) == 8
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 18, 29)) == 8
 
    def test_after_1830_returns_0(self):
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 18, 30)) == 0
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2026, 6, 15, 23, 59)) == 0
 
    def test_toll_free_vehicle_returns_0(self):
        assert self.calc.get_toll_fee_for_pass(Motorbike(), dt(2026, 6, 15, 7, 0)) == 0
 
    def test_weekend_returns_0(self):
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2025, 3, 15, 7, 0)) == 0
 
    def test_holiday_returns_0(self):
        assert self.calc.get_toll_fee_for_pass(self.car, dt(2025, 12, 25, 7, 0)) == 0


class TestGetTollFee:
    def setup_method(self):
        self.calc = TollCalculator()
        self.car = Car()
        self.motorbike = Motorbike()
    
    def test_empty_dates_return_0(self):
        assert self.calc.get_toll_fee(self.car, []) == 0
    
    def test_toll_free_vehicle_returns_0(self):
        dates = [
            dt(2026, 6, 15, 7, 0), 
            dt(2026, 6, 15, 8, 0)
        ]
        assert self.calc.get_toll_fee(self.motorbike, dates) == 0
    
    def test_single_pass_returns_correct_fee(self):
        dates = [dt(2026, 6, 15, 7, 0)]
        assert self.calc.get_toll_fee(self.car, dates) == 18
    
    def test_two_passes_same_hour_returns_correct_fee(self):
        # Should only charge once
        dates = [
            dt(2026, 6, 15, 7, 0),  # 07:00 = 18 SEK 
            dt(2026, 6, 15, 7, 30)  # 07:30 = 18 SEK
        ]
        assert self.calc.get_toll_fee(self.car, dates) == 18
    
    def test_two_passes_different_fees_same_hour(self):
        dates = [
            dt(2026, 6, 15, 8, 0),   # 08:00 = 13 SEK
            dt(2026, 6, 15, 8, 30),  # 08:30 = 8 SEK
        ]
        assert self.calc.get_toll_fee(self.car, dates) == 13
    
    def test_two_passes_different_hours(self):
        dates = [
            dt(2026, 6, 15, 7, 0),  # 07:00 = 18 SEK
            dt(2026, 6, 15, 8, 30)  # 08:30 = 8 SEK
        ]
        assert self.calc.get_toll_fee(self.car, dates) == 26

    def test_daily_fee_capped_at_60(self):
        dates = [
            dt(2026, 6, 15, 7, 0),    # 18 SEK
            dt(2026, 6, 15, 9, 0),    # 8 SEK
            dt(2026, 6, 15, 11, 0),   # 8 SEK
            dt(2026, 6, 15, 15, 30),  # 18 SEK
            dt(2026, 6, 15, 17, 0)    # 13 SEK
        ]
        assert self.calc.get_toll_fee(self.car, dates) == 60
    
    def test_unsorted_dates_handled_correctly(self):
        unsorted_dates = [
            dt(2026, 6, 15, 7, 0),   # 18 SEK
            dt(2026, 6, 15, 9, 0),   # 8 SEK
            dt(2026, 6, 15, 7, 30),  # 18 SEK  
        ]
        sorted_dates = [
            dt(2026, 6, 15, 7, 0),   # 18 SEK
            dt(2026, 6, 15, 7, 30),  # 18 SEK  
            dt(2026, 6, 15, 9, 0),   # 8 SEK
        ]
        assert self.calc.get_toll_fee(self.car, unsorted_dates) == \
               self.calc.get_toll_fee(self.car, sorted_dates)
    
    def test_all_weekends_return_0(self):
        dates = [
            dt(2026, 6, 14, 6, 0),
            dt(2026, 6, 14, 8, 0)
        ]
        assert self.calc.get_toll_fee(self.car, dates) == 0


class TestLoadConfig:
    def test_missing_config_raises_file_not_found(self):
        with patch("builtins.open", side_effect=FileNotFoundError):
            with pytest.raises(FileNotFoundError, match="config.json is not found"):
                TollCalculator()

    def test_missing_key_raises_key_error(self):
        bad_config = json.dumps({"toll_free_vehicles": ["Car"]})  # Missing toll_schedule
        with patch("builtins.open", mock_open(read_data=bad_config)):
            with pytest.raises(KeyError, match="toll_schedule"):
                TollCalculator()
                