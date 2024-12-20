CREATE TABLE [Patient] (
    PatientId INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    Gender NVARCHAR(10),
    ContactNumber NVARCHAR(15),
    Address NVARCHAR(255)
);

CREATE TABLE [Doctor] (
    DoctorId INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Specialization NVARCHAR(100),
    Phone NVARCHAR(15),
    Email NVARCHAR(100)
);

CREATE TABLE [Appointment] (
    AppointmentId INT IDENTITY PRIMARY KEY,
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Reason NVARCHAR(255),
    Status NVARCHAR(20),
    Diagnosis NVARCHAR(255),
    FOREIGN KEY (PatientId) REFERENCES Patient(PatientId),
    FOREIGN KEY (DoctorId) REFERENCES Doctor(DoctorId)
);

CREATE TABLE [User] (
    UserId INT IDENTITY PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL
);