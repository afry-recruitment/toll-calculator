from datetime import datetime, timedelta

from src.model.tollrecord import TollRecords
from src.services.tollfee_service import get_tollfee_by_vehicle_type


def has_daily_price_quota_exceed(license_no) -> bool:
    today = datetime.today()

    pipeline = [
        {
            '$match': {
                'in_timestamp': {
                    '$gte': datetime.combine(today, datetime.min.time()),
                    '$lt': datetime.combine(today, datetime.max.time())
                },
                'license_no': license_no
            }
        },
        {
            '$group': {
                '_id': None,
                'total_fee': {
                    '$sum': '$fee'
                }
            }
        }
    ]
    results = list(TollRecords.objects.aggregate(*pipeline))

    if results.__len__() > 0 and results[0]['total_fee'] > 60:
        return True
    else:
        return False


def save(payload):
    record = TollRecords(
        license_no=payload['license_no'],
        vehicle_type_code=payload['vehicle_type'],
        is_fee=payload['is_free'],
    )
    record.save()


def get_tollfee(license_no) -> float:
    result = TollRecords.objects.filter(license_no=license_no, status='OPEN')

    if len(result) == 0:
        raise ValueError("No record found")

    result_dict = {}
    for result in result:
        vehicle_dict = {
            "vehicle_type_code": result.vehicle_type_code,
            "in_timestamp": result.in_timestamp,
        }
        result_dict.update(vehicle_dict)

    in_timestamp = result_dict.get('in_timestamp')
    out_timestamp = datetime.now()
    total_hours = (out_timestamp - in_timestamp).total_seconds() / 3600
    if result_dict.get('is_fee'):
        TollRecords.objects(license_no=license_no, status='OPEN').update(status='PAID', out_timestamp=out_timestamp,
                                                                         total_hours=total_hours, fee=0.00)
        return 0.00
    else:
        hourly_rates = get_tollfee_by_vehicle_type(result_dict.get('vehicle_type_code'))
        fee = calculate_toll_fee(in_timestamp, out_timestamp, hourly_rates)
        TollRecords.objects(license_no=license_no, status='OPEN').update(status='PAID', out_timestamp=out_timestamp,
                                                                         total_hours=total_hours, fee=fee)

        return fee


def calculate_toll_fee(start_time, end_time, hourly_rates) -> float:
    datetime_pointer = start_time

    timediff = end_time - start_time
    no_of_days = timediff.days

    day_fee = 0
    if no_of_days > 0:
        day_fee = 60.00 * no_of_days
        datetime_pointer = start_time + timedelta(days=no_of_days)

    hourly_fee = 0
    while datetime_pointer < end_time:

        next_hour_pointer = datetime_pointer + timedelta(hours=1)
        start_time_min = datetime_pointer.minute
        if start_time_min == 0:
            rate = hourly_rates[datetime_pointer.hour]
            hourly_fee += rate
        else:
            current_hour_rate = hourly_rates[datetime_pointer.hour]
            next_hour_rate = hourly_rates[next_hour_pointer.hour]
            rate = max(current_hour_rate, next_hour_rate)
            hourly_fee += rate

        datetime_pointer = datetime_pointer + timedelta(hours=1)

    if hourly_fee > 60:
        hourly_fee = 60
    total_fee = day_fee + hourly_fee

    return float(total_fee)
