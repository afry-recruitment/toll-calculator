from mongoengine import *


class Holiday(DynamicDocument):
    date = DateField(required=True)
    description = StringField()
    meta = {
        'collection': 'sys_holiday',
        'strict': False,
        'indexes': ['date'],
    }
