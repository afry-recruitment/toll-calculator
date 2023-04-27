db.createCollection("sys_tollfee");
db.createCollection("sys_holiday");
db.createCollection("sys_vehicle");


db.sys_tollfee.insertMany([
{vehicle_type_code: "DEFAULT", hour: "0:00", fee: 8},
{vehicle_type_code: "DEFAULT", hour: "1:00", fee: 8},
{vehicle_type_code: "DEFAULT", hour: "2:00", fee: 8},
{vehicle_type_code: "DEFAULT", hour: "3:00", fee: 8},
{vehicle_type_code: "DEFAULT", hour: "4:00", fee: 8},
{vehicle_type_code: "DEFAULT", hour: "5:00", fee: 8},
{vehicle_type_code: "DEFAULT", hour: "6:00", fee: 12},
{vehicle_type_code: "DEFAULT", hour: "7:00", fee: 18},
{vehicle_type_code: "DEFAULT", hour: "8:00", fee: 18},
{vehicle_type_code: "DEFAULT", hour: "9:00", fee: 16},
{vehicle_type_code: "DEFAULT", hour: "10:00", fee: 15},
{vehicle_type_code: "DEFAULT", hour: "11:00", fee: 16},
{vehicle_type_code: "DEFAULT", hour: "12:00", fee: 18},
{vehicle_type_code: "DEFAULT", hour: "13:00", fee: 18},
{vehicle_type_code: "DEFAULT", hour: "14:00", fee: 15},
{vehicle_type_code: "DEFAULT", hour: "15:00", fee: 15},
{vehicle_type_code: "DEFAULT", hour: "16:00", fee: 10},
{vehicle_type_code: "DEFAULT", hour: "17:00", fee: 12},
{vehicle_type_code: "DEFAULT", hour: "18:00", fee: 18},
{vehicle_type_code: "DEFAULT", hour: "19:00", fee: 18},
{vehicle_type_code: "DEFAULT", hour: "20:00", fee: 8},
{vehicle_type_code: "DEFAULT", hour: "21:00", fee: 8},
{vehicle_type_code: "DEFAULT", hour: "22:00", fee: 8},
{vehicle_type_code: "DEFAULT", hour: "23:00", fee: 8},
{vehicle_type_code: "DEFAULT", hour: "24:00", fee: 8}
]);

db.sys_tollfee.insertMany([ { vehicle_type_code: "CAR1000", hour: "0:00", fee: 8 }, { vehicle_type_code: "CAR1000", hour: "1:00", fee: 8 }, { vehicle_type_code: "CAR1000", hour: "2:00", fee: 8 }, { vehicle_type_code: "CAR1000", hour: "3:00", fee: 8 }, { vehicle_type_code: "CAR1000", hour: "4:00", fee: 8 }, { vehicle_type_code: "CAR1000", hour: "5:00", fee: 8 }, { vehicle_type_code: "CAR1000", hour: "6:00", fee: 12 }, { vehicle_type_code: "CAR1000", hour: "7:00", fee: 18 }, { vehicle_type_code: "CAR1000", hour: "8:00", fee: 18 }, { vehicle_type_code: "CAR1000", hour: "9:00", fee: 16 }, { vehicle_type_code: "CAR1000", hour: "10:00", fee: 15 }, { vehicle_type_code: "CAR1000", hour: "11:00", fee: 16 }, { vehicle_type_code: "CAR1000", hour: "12:00", fee: 18 }, { vehicle_type_code: "CAR1000", hour: "13:00", fee: 18 }, { vehicle_type_code: "CAR1000", hour: "14:00", fee: 15 }, { vehicle_type_code: "CAR1000", hour: "15:00", fee: 15 }, { vehicle_type_code: "CAR1000", hour: "16:00", fee: 10 }, { vehicle_type_code: "CAR1000", hour: "17:00", fee: 12 }, { vehicle_type_code: "CAR1000", hour: "18:00", fee: 18 }, { vehicle_type_code: "CAR1000", hour: "19:00", fee: 18 }, { vehicle_type_code: "CAR1000", hour: "20:00", fee: 8 }, { vehicle_type_code: "CAR1000", hour: "21:00", fee: 8 }, { vehicle_type_code: "CAR1000", hour: "22:00", fee: 8 } ]);

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