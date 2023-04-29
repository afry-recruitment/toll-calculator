from mongoengine import *


class TollFee(DynamicDocument):
    vehicle_type_code = StringField(required=True)
    hour_in_24h = IntField(required=True)
    fee = FloatField(required=True)
    meta = {
        'collection': 'sys_tollfee',
        'strict': False,
        'indexes': ['vehicle_type_code'],
    }
