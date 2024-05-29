from typing import List
from datetime import datetime
from enum import Enum

from fastapi import FastAPI, Query

import app.toll_calculator as toll_calculator

VehicleTypes = Enum(
    "VehicleTypes",
    {vehicle: vehicle for vehicle in toll_calculator.POSSIBLE_VEHICLES},
    type=str,
)

app = FastAPI()


@app.get("/GetTollFee")
def get_toll_fee(vehicle: VehicleTypes, dates: List[datetime] = Query(None)):
    return toll_calculator.get_toll_fee(vehicle, dates)
