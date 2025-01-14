DROP TABLE IF EXISTS toll_dates;
DROP TABLE IF EXISTS car_dates;
DROP TABLE IF EXISTS vehicles;
DROP TABLE IF EXISTS vehicle_types;
DROP TABLE IF EXISTS holiday;
DROP TABLE IF EXISTS rate;
DROP TABLE IF EXISTS cities;

CREATE TABLE vehicle_types (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255),
    is_toll_free BOOLEAN
);

CREATE TABLE cities (
   id INT PRIMARY KEY,
   name VARCHAR(50) NOT NULL,
   code VARCHAR(20) NOT NULL,
   submission_date DATE
);

CREATE TABLE vehicles (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    reg_number VARCHAR(255) NOT NULL UNIQUE,
    vehicle_type_id BIGINT NOT NULL,
    CONSTRAINT fk_vehicle_type FOREIGN KEY (vehicle_type_id) REFERENCES vehicle_types(id)
);

CREATE TABLE rate(
  id INT PRIMARY KEY,
  start_date TIMESTAMP NOT NULL,
  end_date TIMESTAMP NOT NULL,
  tax INT NOT NULL,
  city_id INT,
  foreign key (city_id) references cities(id)
);

CREATE TABLE toll_dates (
    id BIGINT AUTO_INCREMENT PRIMARY KEY, -- Optional: Unique identifier for each entry
    vehicle_id BIGINT NOT NULL,          -- References the id in the vehicles table
    crossed_date TIMESTAMP NOT NULL,     -- Date and time when the vehicle crossed the toll
    CONSTRAINT fk_vehicle FOREIGN KEY (vehicle_id) REFERENCES vehicles(id) ON DELETE CASCADE
);

CREATE TABLE holiday(
  id INT PRIMARY KEY,
  date TIMESTAMP NOT NULL,
  city_id INT,
  foreign key (city_id) references cities(id)
);

INSERT INTO vehicle_types (id, name, is_toll_free)
VALUES
(1, 'Car', false),
(2, 'Diplomat', true),
(3, 'Emergency', true),
(4, 'Foreign', true),
(5, 'Military', true),
(6, 'Motorbike', true),
(7, 'Tractor', true);



-- Re-enable foreign key constraints if disabled
SET REFERENTIAL_INTEGRITY TRUE;

INSERT INTO vehicles (id, reg_number, vehicle_type_id)
VALUES
    (1001, 'SSL259', 1),
    (1002, 'QST667', 2),
    (1003, 'SUI167', 3),
    (1004, 'YUI898', 4),
    (1005, 'WER767', 5),
    (1006, 'POI231', 6),
    (1007, 'WQE098', 7);

INSERT INTO toll_dates (vehicle_id, crossed_date)
    VALUES
    -- Entries for vehicle 1001 (CAR)
    (1001, parsedatetime('01-01-2025 08:15:00', 'dd-MM-yyyy HH:mm:ss')),
    (1001, parsedatetime('01-01-2025 10:30:00', 'dd-MM-yyyy HH:mm:ss')),
    (1001, parsedatetime('02-01-2025 14:45:00', 'dd-MM-yyyy HH:mm:ss')),
    (1001, parsedatetime('03-01-2025 18:00:00', 'dd-MM-yyyy HH:mm:ss')),
    -- Entries for vehicle 1002 (DIPLOMAT)
    (1002, parsedatetime('01-01-2025 09:00:00', 'dd-MM-yyyy HH:mm:ss')),
    (1002, parsedatetime('02-01-2025 11:20:00', 'dd-MM-yyyy HH:mm:ss')),
    (1002, parsedatetime('02-01-2025 16:15:00', 'dd-MM-yyyy HH:mm:ss')),
    (1002, parsedatetime('03-01-2025 19:40:00', 'dd-MM-yyyy HH:mm:ss')),
    -- Entries for vehicle 1003 (EMERGENCY)
    (1003, parsedatetime('01-01-2025 11:15:00', 'dd-MM-yyyy HH:mm:ss')),
    (1003, parsedatetime('01-01-2025 13:50:00', 'dd-MM-yyyy HH:mm:ss')),
    (1003, parsedatetime('02-01-2025 15:25:00', 'dd-MM-yyyy HH:mm:ss')),
    (1003, parsedatetime('03-01-2025 21:00:00', 'dd-MM-yyyy HH:mm:ss')),
    -- Entries for vehicle 1004 (FOREIGN)
    (1004, parsedatetime('01-01-2025 12:30:00', 'dd-MM-yyyy HH:mm:ss')),
    (1004, parsedatetime('02-01-2025 11:45:00', 'dd-MM-yyyy HH:mm:ss')),
    (1004, parsedatetime('02-01-2025 14:50:00', 'dd-MM-yyyy HH:mm:ss')),
    (1004, parsedatetime('03-01-2025 17:25:00', 'dd-MM-yyyy HH:mm:ss')),
    -- Entries for vehicle 1005 (MILITARY)
    (1005, parsedatetime('01-01-2025 14:20:00', 'dd-MM-yyyy HH:mm:ss')),
    (1005, parsedatetime('02-01-2025 08:10:00', 'dd-MM-yyyy HH:mm:ss')),
    (1005, parsedatetime('03-01-2025 10:55:00', 'dd-MM-yyyy HH:mm:ss')),
    (1005, parsedatetime('04-01-2025 18:40:00', 'dd-MM-yyyy HH:mm:ss')),
    -- Entries for vehicle 1006 (MOTORBIKE)
    (1006, parsedatetime('01-01-2025 15:35:00', 'dd-MM-yyyy HH:mm:ss')),
    (1006, parsedatetime('02-01-2025 07:45:00', 'dd-MM-yyyy HH:mm:ss')),
    (1006, parsedatetime('02-01-2025 22:00:00', 'dd-MM-yyyy HH:mm:ss')),
    (1006, parsedatetime('03-01-2025 20:15:00', 'dd-MM-yyyy HH:mm:ss')),
    -- Entries for vehicle 1007 (TRACTOR)
    (1007, parsedatetime('01-01-2025 16:50:00', 'dd-MM-yyyy HH:mm:ss')),
    (1007, parsedatetime('02-01-2025 11:00:00', 'dd-MM-yyyy HH:mm:ss')),
    (1007, parsedatetime('03-01-2025 15:20:00', 'dd-MM-yyyy HH:mm:ss')),
    (1007, parsedatetime('04-01-2025 18:35:00', 'dd-MM-yyyy HH:mm:ss'));


INSERT INTO cities (id, name, code)
    VALUES (1, 'Stockholm', 'ST'),
    (2, 'Gothenburg', 'GB'),
    (3, 'Malmo', 'ML'),
    (4, 'Uppsala', 'UP');

INSERT INTO holiday (id, date, city_id)
    VALUES ('1', parsedatetime('01-01-1970 00:00:00', 'dd-MM-yyyy hh:mm:ss'), 2),
    ('2', parsedatetime('06-01-1970 00:00:00', 'dd-MM-yyyy hh:mm:ss'), 2),
    ('3', parsedatetime('18-04-1970 00:00:00', 'dd-MM-yyyy hh:mm:ss'), 2),
    ('4', parsedatetime('20-04-1970 00:00:00', 'dd-MM-yyyy hh:mm:ss'), 2),
    ('5', parsedatetime('21-04-1970 00:00:00', 'dd-MM-yyyy hh:mm:ss'), 2),
    ('6', parsedatetime('01-05-1970 00:00:00', 'dd-MM-yyyy hh:mm:ss'), 2),
    ('7', parsedatetime('29-05-1970 00:00:00', 'dd-MM-yyyy hh:mm:ss'), 2),
    ('8', parsedatetime('06-06-1970 00:00:00', 'dd-MM-yyyy hh:mm:ss'), 2),
    ('9', parsedatetime('21-06-1970 00:00:00', 'dd-MM-yyyy hh:mm:ss'), 2),
    ('10', parsedatetime('01-11-1970 00:00:00', 'dd-MM-yyyy hh:mm:ss'), 2),
    ('11', parsedatetime('25-12-1970 00:00:00', 'dd-MM-yyyy hh:mm:ss'), 2),
    ('12', parsedatetime('26-12-1970 00:00:00', 'dd-MM-yyyy hh:mm:ss'), 2);

INSERT INTO rate (id, start_date, end_date, city_id, tax)
    VALUES('1', parsedatetime('01-01-1970 06:00', 'dd-MM-yyyy HH:mm'), parsedatetime('01-01-1970 06:29', 'dd-MM-yyyy HH:mm'), 2, 8),
    ('2', parsedatetime('01-01-1970 06:30', 'dd-MM-yyyy HH:mm'), parsedatetime('01-01-1970 06:59', 'dd-MM-yyyy HH:mm'), 2, 13),
    ('3', parsedatetime('01-01-1970 07:00', 'dd-MM-yyyy HH:mm'), parsedatetime('01-01-1970 07:59', 'dd-MM-yyyy HH:mm'), 2, 18),
    ('4', parsedatetime('01-01-1970 08:00', 'dd-MM-yyyy HH:mm'), parsedatetime('01-01-1970 08:29', 'dd-MM-yyyy HH:mm'), 2, 13),
    ('5', parsedatetime('01-01-1970 08:30', 'dd-MM-yyyy HH:mm'), parsedatetime('01-01-1970 14:59', 'dd-MM-yyyy HH:mm'), 2, 8),
    ('6', parsedatetime('01-01-1970 15:00', 'dd-MM-yyyy HH:mm'), parsedatetime('01-01-1970 15:29', 'dd-MM-yyyy HH:mm'), 2, 13),
    ('7', parsedatetime('01-01-1970 15:30', 'dd-MM-yyyy HH:mm'), parsedatetime('01-01-1970 16:59', 'dd-MM-yyyy HH:mm'), 2, 18),
    ('8', parsedatetime('01-01-1970 17:00', 'dd-MM-yyyy HH:mm'), parsedatetime('01-01-1970 17:59', 'dd-MM-yyyy HH:mm'), 2, 13),
    ('9', parsedatetime('01-01-1970 18:00', 'dd-MM-yyyy HH:mm'), parsedatetime('01-01-1970 18:29', 'dd-MM-yyyy HH:mm'), 2, 8);

