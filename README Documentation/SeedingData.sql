INSERT INTO TollExempts (IsExempt) VALUES
(0), -- Not tax exempt
(1); -- Tax exempt
---------------------------------------------

  INSERT INTO VehicleTypes (Type, TollExemptID) VALUES
(0, 1),  -- Tullpliktig, betalar tull
(1, 2),  -- Utryckningsfordon, ej tull
(2, 2),  -- Buss, ej tull
(3, 2),  -- Diplomat, ej tull
(4, 2),  -- Motorcykel, ej tull
(5, 2),  -- Milit�r, ej tull
(6, 2);  -- R�relsehinder, ej tull
-------------------------------------------

INSERT INTO Vehicles (LicensePlate, VehicleTypeID, ActiveFeeDataID) VALUES
('ABC123', 1, NULL),  -- Bil
('XYZ789', 5, NULL),  -- Milit�r
('OWL457', 1, NULL),  -- Bil
('WEW898', 2, NULL);  -- Ambulans
