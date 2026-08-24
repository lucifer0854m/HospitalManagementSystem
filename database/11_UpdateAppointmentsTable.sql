USE HospitalManagementDB;
GO

ALTER TABLE dbo.Appointments
ADD
    TokenNumber INT NULL,
    Diagnosis NVARCHAR(500) NULL,
    Prescription NVARCHAR(MAX) NULL;
GO