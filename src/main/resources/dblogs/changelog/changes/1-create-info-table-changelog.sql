-- liquibase formatted sql
-- changeset Sujata:1

CREATE TABLE city (
    id bigint NOT NULL AUTO_INCREMENT,
    name varchar(255) NOT NULL,
    PRIMARY KEY (id)
);
INSERT INTO city VALUES (1,'Gothenburg');

CREATE TABLE vehicle (
    id bigint NOT NULL AUTO_INCREMENT,
    name varchar(255) NOT NULL,
    PRIMARY KEY (id)
);

INSERT INTO vehicle VALUES (1,'Emergency'),(2,'Bus'),(3,'Diplomat'),(4,'Tractor'),(5,'Military'),(6,'Foreign'),(7,'Car'),(8,'Motorbike');

CREATE TABLE city_tax_exempt_vehicle (
    city_id bigint NOT NULL,
    vehicle_id bigint NOT NULL,
    PRIMARY KEY (city_id,vehicle_id),
    CONSTRAINT `FK_vehicle_id` FOREIGN KEY (vehicle_id) REFERENCES vehicle (id),
    CONSTRAINT `FK_city_id` FOREIGN KEY (city_id) REFERENCES city (id)
);
INSERT INTO city_tax_exempt_vehicle VALUES (1,1),(1,3),(1,4),(1,5),(1,6),(1,8);

CREATE TABLE city_holiday_calendar (
    id bigint NOT NULL AUTO_INCREMENT,
    date datetime(6) NOT NULL,
    city_id bigint NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT FK_city_holiday_calendar_city FOREIGN KEY (city_id) REFERENCES city (id)
);
INSERT INTO city_holiday_calendar VALUES (1,'2013-12-30 00:00:00.000000',1);

CREATE TABLE city_holiday_month_calendar (
    city_id bigint NOT NULL,
    is_april tinyint(1) NOT NULL DEFAULT '0',
    is_august tinyint(1) NOT NULL DEFAULT '0',
    is_december tinyint(1) NOT NULL DEFAULT '0',
    is_february tinyint(1) NOT NULL DEFAULT '0',
    is_january tinyint(1) NOT NULL DEFAULT '0',
    is_july tinyint(1) NOT NULL DEFAULT '0',
    is_june tinyint(1) NOT NULL DEFAULT '0',
    is_march tinyint(1) NOT NULL DEFAULT '0',
    is_may tinyint(1) NOT NULL DEFAULT '0',
    is_november tinyint(1) NOT NULL DEFAULT '0',
    is_october tinyint(1) NOT NULL DEFAULT '0',
    is_september tinyint(1) NOT NULL DEFAULT '0',
    PRIMARY KEY (city_id),
    CONSTRAINT `FK_holiday_month_calendar` FOREIGN KEY (city_id) REFERENCES city (id)
) ;
INSERT INTO city_holiday_month_calendar VALUES (1,0,0,0,0,0,1,0,0,0,0,0,0);

CREATE TABLE city_working_calendar (
    city_id bigint NOT NULL PRIMARY KEY ,
    is_friday tinyint(1) NOT NULL DEFAULT '1',
    is_monday tinyint(1) NOT NULL DEFAULT '1',
    is_saturday tinyint(1) NOT NULL DEFAULT '0',
    is_sunday tinyint(1) NOT NULL DEFAULT '0',
    is_thursday tinyint(1) NOT NULL DEFAULT '1',
    is_tuesday tinyint(1) NOT NULL DEFAULT '1',
    is_wednesday tinyint(1) NOT NULL DEFAULT '1',
    CONSTRAINT `FK_city_working_calendar` FOREIGN KEY (city_id) REFERENCES city (id)
);
INSERT INTO city_working_calendar VALUES (1,1,1,0,0,1,1,1);

CREATE TABLE city_tax_rate (
    id bigint NOT NULL AUTO_INCREMENT,
    charge decimal(19,2) NOT NULL,
    from_time time NOT NULL,
    to_time time NOT NULL,
    city_id bigint NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT `FK_tax_rate` FOREIGN KEY (city_id) REFERENCES city (id)
);
INSERT INTO city_tax_rate VALUES (1,8.00,'06:00:00','06:29:59',1),(2,13.00,'06:30:00','06:59:59',1),(3,18.00,'07:00:00','07:59:59',1),
                                 (4,13.00,'08:00:00','08:29:59',1),(5,8.00,'08:30:00','14:59:59',1),(6,13.00,'15:00:00','15:29:59',1),
                                 (7,18.00,'15:30:00','16:59:59',1),(8,13.00,'17:00:00','17:59:59',1),(9,8.00,'18:00:00','18:29:59',1),
                                 (10,0.00,'18:30:00','23:59:59',1),(11,0.00,'00:00:00','05:59:59',1);

CREATE TABLE city_tax_choice (
    max_tax_per_day int NOT NULL DEFAULT '60',
    number_of_tax_free_days_before_holiday int NOT NULL DEFAULT '0',
    single_charge_interval_in_min int NOT NULL DEFAULT '0',
    city_id bigint NOT NULL,
    PRIMARY KEY (city_id),
    CONSTRAINT `FK_city_tax_choice` FOREIGN KEY (city_id) REFERENCES city (id)
);
INSERT INTO city_tax_choice VALUES (60,1,60,1);