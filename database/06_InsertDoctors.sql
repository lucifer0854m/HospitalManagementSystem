USE HospitalManagementDB;
GO

INSERT INTO dbo.Doctors
(
    DepartmentId,
    FirstName,
    LastName,
    Gender,
    Phone,
    Email,
    Qualification,
    Specialization,
    ExperienceYears,
    ConsultationFee
)
VALUES
(1,'Amit','Sharma','Male','9876543210','amit@medicare.com','MBBS, MD','Cardiologist',12,800),
(2,'Priya','Verma','Female','9876543211','priya@medicare.com','MBBS, DM','Neurologist',8,900),
(3,'Rohit','Singh','Male','9876543212','rohit@medicare.com','MS Orthopedics','Orthopedic Surgeon',10,700),
(4,'Anjali','Mehta','Female','9876543213','anjali@medicare.com','DM Nephrology','Nephrologist',9,1000);