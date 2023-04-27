from mongoengine import *


class Vehicle(DynamicDocument):
    vehicle_type = StringField(required=True)
    vehicle_code = StringField(required=True)
    is_free = BooleanField(required=True)
    description = StringField()
    meta = {
        'collection': 'sys_vehicle',
        'strict': False,
        'indexes': ['vehicle_code'],
    }
