// switch to the tolldb database
db = db.getSiblingDB('tolldb');

db.createCollection("sys_tollfee");
db.createCollection("sys_holiday");
db.createCollection("sys_vehicle");
db.createCollection("tx_tollrecords");


db.sys_tollfee.insertMany([
{vehicle_type_code: "DEFAULT", hour_in_24h: 0, fee: 8},
{vehicle_type_code: "DEFAULT", hour_in_24h: 1, fee: 8},
{vehicle_type_code: "DEFAULT", hour_in_24h: 2, fee: 8},
{vehicle_type_code: "DEFAULT", hour_in_24h: 3, fee: 8},
{vehicle_type_code: "DEFAULT", hour_in_24h: 4, fee: 8},
{vehicle_type_code: "DEFAULT", hour_in_24h: 5, fee: 8},
{vehicle_type_code: "DEFAULT", hour_in_24h: 6, fee: 12},
{vehicle_type_code: "DEFAULT", hour_in_24h: 7, fee: 18},
{vehicle_type_code: "DEFAULT", hour_in_24h: 8, fee: 18},
{vehicle_type_code: "DEFAULT", hour_in_24h: 9, fee: 16},
{vehicle_type_code: "DEFAULT", hour_in_24h: 10, fee: 15},
{vehicle_type_code: "DEFAULT", hour_in_24h: 11, fee: 16},
{vehicle_type_code: "DEFAULT", hour_in_24h: 12, fee: 18},
{vehicle_type_code: "DEFAULT", hour_in_24h: 13, fee: 18},
{vehicle_type_code: "DEFAULT", hour_in_24h: 14, fee: 15},
{vehicle_type_code: "DEFAULT", hour_in_24h: 15, fee: 15},
{vehicle_type_code: "DEFAULT", hour_in_24h: 16, fee: 10},
{vehicle_type_code: "DEFAULT", hour_in_24h: 17, fee: 12},
{vehicle_type_code: "DEFAULT", hour_in_24h: 18, fee: 18},
{vehicle_type_code: "DEFAULT", hour_in_24h: 19, fee: 18},
{vehicle_type_code: "DEFAULT", hour_in_24h: 20, fee: 8},
{vehicle_type_code: "DEFAULT", hour_in_24h: 21, fee: 8},
{vehicle_type_code: "DEFAULT", hour_in_24h: 22, fee: 8},
{vehicle_type_code: "DEFAULT", hour_in_24h: 23, fee: 8}
]);

db.sys_holiday.insertMany([
{ "date": ISODate("2023-01-01T00:00:00Z"), "description": "New Year Day" },
{ "date": ISODate("2023-01-06T00:00:00Z"), "description": "Epiphany" },
{ "date": ISODate("2023-04-07T00:00:00Z"), "description": "Good Friday" },
{ "date": ISODate("2023-04-09T00:00:00Z"), "description": "Easter Sunday" },
{ "date": ISODate("2023-04-10T00:00:00Z"), "description": "Easter Monday" },
{ "date": ISODate("2023-05-01T00:00:00Z"), "description": "International Workers Day" },
{ "date": ISODate("2023-05-18T00:00:00Z"), "description": "Ascension Day" },
{ "date": ISODate("2023-05-28T00:00:00Z"), "description": "Pentecost" },
{ "date": ISODate("2023-06-06T00:00:00Z"), "description": "National Day of Sweden" },
{ "date": ISODate("2023-06-23T00:00:00Z"), "description": "Midsummer Eve" },
{ "date": ISODate("2023-06-24T00:00:00Z"), "description": "Midsummer Day" },
{ "date": ISODate("2023-11-04T00:00:00Z"), "description": "All Saints Day" },
{ "date": ISODate("2023-12-24T00:00:00Z"), "description": "Christmas Eve" },
{ "date": ISODate("2023-12-25T00:00:00Z"), "description": "Christmas Day" },
{ "date": ISODate("2023-12-26T00:00:00Z"), "description": "St. Stephen Day" },
{ "date": ISODate("2023-12-31T00:00:00Z"), "description": "New Year Eve" }
]);

db.sys_vehicle.insertMany([
{ "vehicle_type": "DEFAULT", "vehicle_code": "DEFAULT", "is_free": false, "description": "Default vehicles that are not defined by special rules" },
{ "vehicle_type": "CAR", "vehicle_code": "CAR1000", "is_free": true, "description": "Cars less than 1000CC engine capacity" },
{ "vehicle_type": "CAR", "vehicle_code": "CAR1800", "is_free": false, "description": "Cars more than 1000CC engine capacity" }
]);
