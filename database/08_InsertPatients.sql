USE HospitalManagementDB;
GO

INSERT INTO dbo.Patients
(
    FirstName,
    LastName,
    Gender,
    DateOfBirth,
    BloodGroup,
    Phone,
    Email,
    Address,
    EmergencyContact
)
VALUES
('Deepak','Lodhi','Male','2002-03-15','O+','6260232301','deepak@example.com','Bhopal','9876543210'),

('Rahul','Patel','Male','1998-08-12','A+','9999999999','rahul@example.com','Indore','8888888888'),

('Priya','Sharma','Female','1995-05-20','B+','9876543211','priya@example.com','Delhi','7777777777');
GO