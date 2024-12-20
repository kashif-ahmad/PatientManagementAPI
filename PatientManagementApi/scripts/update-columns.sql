-- Update Patient.ContactNumber and Patient.Address
ALTER TABLE Patient
ALTER COLUMN ContactNumber NVARCHAR(255);

ALTER TABLE Patient
ALTER COLUMN Address NVARCHAR(255);

-- Update Doctor.Phone and Doctor.Email
ALTER TABLE Doctor
ALTER COLUMN Phone NVARCHAR(255);

ALTER TABLE Doctor
ALTER COLUMN Email NVARCHAR(255);