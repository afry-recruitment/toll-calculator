import json
from datetime import datetime

from src.model.tollrecord import TollRecords


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

    toll_records_dict = json.loads(result.to_json())
    if toll_records_dict[0]['is_fee']:
        TollRecords.objects(license_no=license_no, status='OPEN').update(status='PAID')
        return 0.00
    else:
        return 2
