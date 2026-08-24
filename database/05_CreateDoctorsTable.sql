USE HospitalManagementDB;
GO

CREATE TABLE dbo.Doctors
(
    DoctorId INT IDENTITY(1,1) PRIMARY KEY,

    DepartmentId INT NOT NULL,

    FirstName NVARCHAR(50) NOT NULL,

    LastName NVARCHAR(50) NOT NULL,

    Gender NVARCHAR(10),

    Phone NVARCHAR(15),

    Email NVARCHAR(100),

    Qualification NVARCHAR(100),

    Specialization NVARCHAR(100),

    ExperienceYears INT,

    ConsultationFee DECIMAL(10,2),

    IsActive BIT NOT NULL DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_Doctors_Departments
        FOREIGN KEY (DepartmentId)
        REFERENCES dbo.Departments(DepartmentId)
);
GO