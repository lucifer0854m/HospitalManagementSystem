USE HospitalManagementDB;
GO

CREATE TABLE dbo.Appointments
(
    AppointmentId INT IDENTITY(1,1) PRIMARY KEY,

    PatientId INT NOT NULL,

    DoctorId INT NOT NULL,

    AppointmentDate DATE NOT NULL,

    AppointmentTime TIME NOT NULL,

    Status NVARCHAR(20) DEFAULT 'Scheduled',

    Symptoms NVARCHAR(500),

    Notes NVARCHAR(500),

    CreatedAt DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_Appointments_Patient
        FOREIGN KEY (PatientId)
        REFERENCES dbo.Patients(PatientId),

    CONSTRAINT FK_Appointments_Doctor
        FOREIGN KEY (DoctorId)
        REFERENCES dbo.Doctors(DoctorId)
);
GO