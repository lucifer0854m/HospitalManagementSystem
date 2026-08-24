USE HospitalManagementDB;
GO

CREATE TABLE dbo.Patients
(
    PatientId INT IDENTITY(1001,1) PRIMARY KEY,

    FirstName NVARCHAR(50) NOT NULL,

    LastName NVARCHAR(50) NOT NULL,

    Gender NVARCHAR(10),

    DateOfBirth DATE,

    BloodGroup NVARCHAR(5),

    Phone NVARCHAR(15),

    Email NVARCHAR(100),

    Address NVARCHAR(255),

    EmergencyContact NVARCHAR(15),

    IsActive BIT NOT NULL DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    UpdatedAt DATETIME2 NULL
);
GO