# Authentication and roles

The application now uses ASP.NET Core Identity for sign-in, users, and roles.

## Roles

| Role | Access |
| --- | --- |
| Admin | All modules, reports, and user administration |
| Receptionist | Patients, departments, doctors, appointments, billing |
| Doctor | Patients, appointments, prescriptions, laboratory APIs |
| Pharmacist | Pharmacy and inventory APIs |
| Lab Technician | Laboratory APIs |

## First administrator

Before starting the application for the first time, configure an administrator through user secrets (never commit the password):

```powershell
dotnet user-secrets set "InitialAdmin:Email" "admin@hospital.local" --project src/HospitalManagement.Web
dotnet user-secrets set "InitialAdmin:Password" "ChangeThisPassword1" --project src/HospitalManagement.Web
```

The application creates the roles and that administrator when it starts. The administrator can then sign in at `/Account/Login` and create further users through **Users** in the navigation.

## Database update

The Identity migration is in `src/HospitalManagement.Infrastructure/Data/Migrations`. Configure `ConnectionStrings:DefaultConnection`, then apply it:

```powershell
dotnet ef database update --project src/HospitalManagement.Infrastructure --startup-project src/HospitalManagement.Web
```

> The generated migration represents the complete current Entity Framework model. For an already-created database managed exclusively through the SQL scripts, have the database administrator apply only the new `AspNet*` tables or establish an EF migration baseline before running this command.
