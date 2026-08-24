USE HospitalManagementDB;
GO

INSERT INTO dbo.Appointments
(
PatientId,
DoctorId,
AppointmentDate,
AppointmentTime,
TokenNumber,
Status,
Symptoms,
Diagnosis,
Prescription,
Notes
)

VALUES

(1001,1,'2026-08-02','10:00',1,'Completed',
'Chest Pain',
'Hypertension',
'Tab Telma 40mg',
'Review after 15 days'),

(1002,2,'2026-08-02','11:00',2,'Completed',
'Migraine',
'Migraine',
'Tab Sumatriptan',
'Avoid Stress'),

(1003,3,'2026-08-03','09:30',1,'Scheduled',
'Kidney Pain',
NULL,
NULL,
'Blood Test Pending');
GO