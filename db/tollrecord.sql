// Connect to the MongoDB database
use tolldb;

// Define the list of possible license numbers and vehicle types
var license_nos = ['ABC123', 'DEF456', 'GHI789', 'JKL012', 'MNO345'];
var vehicle_types = ['DEFAULT', 'CAR1000', 'CAR1800'];

// Generate 15 dummy records for the toll_records collection
for (var i = 0; i < 15; i++) {
    // Select a random license number and vehicle type
    var license_no = license_nos[Math.floor(Math.random() * license_nos.length)];
    var vehicle_type = vehicle_types[Math.floor(Math.random() * vehicle_types.length)];

    // Generate random timestamps for the in and out times
    var days_ago = Math.floor(Math.random() * 30) + 1;
    var in_timestamp = new Date(Date.now() - (days_ago * 24 * 60 * 60 * 1000));
    var hours_parked = Math.floor(Math.random() * 12) + 1;
    var out_timestamp = new Date(in_timestamp.getTime() + (hours_parked * 60 * 60 * 1000));

    // Generate random values for the remaining fields
    var is_fee = Math.random() >= 0.5;
    var total_hours = (Math.random() * 11) + 1;
    var fee = parseFloat((Math.random() * 100).toFixed(2));
    var status = ['OPEN', 'PAID'][Math.floor(Math.random() * 3)];
    var description = ['Regular toll', 'Express lane', 'Late payment', 'Special fee'][Math.floor(Math.random() * 4)];

    // Create a new document for the toll_records collection
    var record = {
        license_no: license_no,
        vehicle_type_code: vehicle_type,
        in_timestamp: in_timestamp,
        is_fee: is_fee,
        out_timestamp: out_timestamp,
        total_hours: total_hours,
        fee: fee,
        status: status,
        description: description
    };

    // Insert the new document into the toll_records collection
    db.toll_records.insertOne(record);
}

print('Dummy data generated for toll_records collection.');
