namespace HospitalManagement.Infrastructure.Identity;

public static class HospitalRoles
{
    public const string Admin = "Admin";
    public const string Doctor = "Doctor";
    public const string Receptionist = "Receptionist";
    public const string Pharmacist = "Pharmacist";
    public const string LabTechnician = "Lab Technician";

    public static readonly string[] All = [Admin, Doctor, Receptionist, Pharmacist, LabTechnician];
    public const string ClinicalStaff = Doctor + "," + LabTechnician;
    public const string FrontDesk = Admin + "," + Receptionist;
}
