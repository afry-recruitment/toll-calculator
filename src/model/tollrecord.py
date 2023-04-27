from datetime import datetime

from mongoengine import *


class TollRecords(DynamicDocument):
    license_no = StringField(required=True)
    vehicle_type_code = StringField(required=True)
    in_timestamp = DateTimeField(required=True, default=datetime.now)
    is_fee = BooleanField(required=True)
    out_timestamp = DateTimeField()
    total_hours = FloatField()
    fee = FloatField()
    status = StringField(choices=['OPEN', 'PAID', 'CANCELLED'], default='OPEN')
    description = StringField()

    meta = {
        'collection': 'tx_tollrecords',
        'strict': False,
        'indexes': ['license_no', 'in_timestamp'],
    }
