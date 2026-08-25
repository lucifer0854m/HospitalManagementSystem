# Testing and quality assurance

## Automated checks

Run the project verification command before every pull request or release:

```powershell
.\scripts\verify.ps1
```

It restores packages, builds the complete solution in Release configuration, and runs the automated test suite. Add `-Coverage` to also generate a Cobertura coverage report under `src/HospitalManagement.Tests/TestResults`.

## Current coverage focus

The unit suite covers high-value application-service rules:

- Patient-code uniqueness, future date-of-birth rejection, and persisted data normalization
- Department-code uniqueness and safe deletion when doctors are assigned
- Appointment conflict prevention, past-date rejection, and persisted appointment-number normalization
- Billing patient and payment validation, pharmacy duplicate/expiry validation, laboratory request/result validation, reporting calculations, and authorization/health integration paths

## Performance and logging

- Repository list and search queries use `AsNoTracking()` so read-only screens do not retain unnecessary EF Core change-tracking state.
- Every request produces structured JSON log fields for method, path, status code, and elapsed time. Container and cloud platforms can collect standard output directly.
- Run `./scripts/verify.ps1 -Coverage` before releases and track the Cobertura report in the CI quality gate appropriate for your organization.

## Manual QA checklist

Before releasing, confirm the following in a configured test database:

1. Unauthenticated users are redirected to the login page.
2. Each role only sees and accesses permitted modules.
3. An administrator can create a user for every hospital role.
4. Invalid form input shows errors and does not persist data.
5. Patient, doctor, department, and appointment CRUD operations work end-to-end.
6. The production connection string and first-admin credentials are supplied through secrets, not source control.
