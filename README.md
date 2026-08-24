# 🏥 Hospital Management System

A modular **Hospital Management System** built with **ASP.NET Core, C#, SQL Server, Entity Framework Core, HTML, CSS, and JavaScript**.

The project is designed to manage core hospital operations such as patients, doctors, appointments, prescriptions, billing, pharmacy, laboratory services, and reporting through a maintainable layered architecture.

> 🚧 **Project Status:** Active Development

---

## 📌 Overview

The Hospital Management System provides a centralized platform for managing hospital information and day-to-day operations.

The application follows a layered architecture to separate business logic, domain entities, data access, and the web presentation layer.

### Main Goals

* Digitize hospital management processes
* Maintain centralized patient and doctor records
* Manage appointments efficiently
* Provide structured database operations
* Separate application responsibilities using clean architecture principles
* Provide a scalable foundation for additional hospital modules
* Support future deployment using Docker

---

## ✨ Features

### 👤 Patient Management

* Add new patients
* View patient information
* Update patient records
* Search patient records
* Maintain patient-related information

### 👨‍⚕️ Doctor Management

* Maintain doctor records
* Manage doctor information
* Associate doctors with departments
* View doctor availability and related information

### 📅 Appointment Management

* Schedule appointments
* Maintain appointment records
* Associate patients with doctors
* Track appointment information
* Manage appointment status

### 💊 Prescription Management

* Maintain prescription information
* Associate prescriptions with appointments
* Store medication-related information

### 💰 Billing

Planned/ongoing module for:

* Patient billing
* Invoice management
* Payment tracking
* Billing reports

### 💊 Pharmacy

Planned/ongoing module for:

* Medicine management
* Medicine inventory
* Stock tracking
* Prescription-based medicine handling

### 🧪 Laboratory

Planned/ongoing module for:

* Laboratory tests
* Test requests
* Test results
* Patient laboratory history

### 📊 Reports

Planned/ongoing reporting capabilities include:

* Patient reports
* Doctor reports
* Appointment reports
* Billing reports
* Pharmacy reports
* Laboratory reports

---

## 🏗️ Architecture

The project uses a layered architecture:

```text
HospitalManagementSystem
│
├── HospitalManagementSystem.sln
│
├── src
│   ├── HospitalManagement.Domain
│   │   ├── Entities
│   │   └── Domain Models
│   │
│   ├── HospitalManagement.Application
│   │   ├── DTOs
│   │   ├── Interfaces
│   │   ├── Mapping
│   │   └── Services
│   │
│   ├── HospitalManagement.Infrastructure
│   │   ├── Data
│   │   ├── Repositories
│   │   ├── Configurations
│   │   └── Database Infrastructure
│   │
│   ├── HospitalManagement.Web
│   │   ├── Controllers
│   │   ├── Models
│   │   ├── Views
│   │   ├── wwwroot
│   │   └── Program.cs
│   │
│   └── HospitalManagement.Tests
│
├── database
│   ├── SQL Scripts
│   ├── Database Creation
│   ├── Table Scripts
│   ├── Seed Data
│   └── Stored Database Objects
│
├── docs
│   └── Screenshots
│
├── scripts
│
├── docker-compose.yml
│
├── .gitignore
├── LICENSE
├── README.md
└── HospitalManagementSystem.sln
```

The current repository contains separate Domain, Application, Infrastructure, Web, and Tests projects under `src/`.

---

## 🛠️ Technology Stack

| Technology            | Purpose                      |
| --------------------- | ---------------------------- |
| C#                    | Primary programming language |
| ASP.NET Core MVC      | Web application framework    |
| .NET                  | Application platform         |
| Entity Framework Core | ORM / data access            |
| SQL Server            | Relational database          |
| HTML5                 | Web structure                |
| CSS3                  | Styling                      |
| JavaScript            | Client-side functionality    |
| AutoMapper            | DTO/entity mapping           |
| Git                   | Version control              |
| GitHub                | Source-code hosting          |
| Docker                | Containerization             |
| Visual Studio         | Development environment      |

---

## 🗄️ Database

The project uses **Microsoft SQL Server**.

The repository contains SQL scripts for database creation, schemas, departments, doctors, patients, appointments, inserts, updates, and seed data.

### Database Name

```text
HospitalManagementDB
```

### Database Script Organization

```text
database/
├── 01_CreateDatabase.sql
├── 02_CreateSchemas.sql
├── 03_Departments.sql
├── 05_CreateDoctorsTable.sql
├── 06_CreateAppointmentsTable.sql
├── 06_InsertDoctors.sql
├── 07_CreatePatientsTable.sql
├── 08_InsertPatients.sql
├── 10_InsertAppointments.sql
├── 11_UpdateAppointmentsTable.sql
├── HospitalDB.sql
└── SeedData.sql
```

---

## 🚀 Getting Started

### Prerequisites

Install the following software before running the project:

* **Visual Studio 2022**
* **.NET SDK**
* **SQL Server**
* **SQL Server Management Studio (SSMS)**
* **Git**
* **Docker Desktop** — optional

---

## 1️⃣ Clone the Repository

```bash
git clone https://github.com/lucifer0854m/HospitalManagementSystem.git
```

Move into the project directory:

```bash
cd HospitalManagementSystem
```

---

## 2️⃣ Open the Solution

Open:

```text
HospitalManagementSystem.sln
```

in Visual Studio.

The solution contains the main application projects:

```text
HospitalManagement.Domain
HospitalManagement.Application
HospitalManagement.Infrastructure
HospitalManagement.Web
HospitalManagement.Tests
```

---

## 3️⃣ Configure SQL Server

Create the database:

```text
HospitalManagementDB
```

You can execute the SQL scripts located in:

```text
database/
```

Start with the database creation script and then execute the required schema/table/seed scripts.

---

## 4️⃣ Configure the Connection String

Update the connection string in the Web project's configuration file:

```text
src/HospitalManagement.Web/appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HospitalManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

For SQL Server authentication, use your own SQL Server credentials instead.

> ⚠️ Never commit real production passwords, API keys, or other secrets to GitHub.

---

## 5️⃣ Restore Dependencies

From the solution directory:

```bash
dotnet restore
```

---

## 6️⃣ Build the Solution

```bash
dotnet build
```

If the build succeeds, all project references and dependencies are available.

---

## 7️⃣ Run the Application

Navigate to the Web project:

```bash
cd src/HospitalManagement.Web
```

Run:

```bash
dotnet run
```

Alternatively, run the project directly from Visual Studio using:

```text
Ctrl + F5
```

or:

```text
F5
```

---

## 🧪 Running Tests

The solution contains a test project:

```text
HospitalManagement.Tests
```

Run all tests using:

```bash
dotnet test
```

---

## 🐳 Docker

The repository also contains:

```text
docker-compose.yml
```

Docker support can be used to simplify local infrastructure and future deployment.

Start the configured containers with:

```bash
docker compose up -d
```

Stop them with:

```bash
docker compose down
```

> Docker configuration may require additional environment-specific settings depending on the SQL Server and application configuration.

---

## 📂 Project Structure

### Domain

```text
HospitalManagement.Domain
```

Contains core business entities and domain models.

Examples:

```text
Patient
Doctor
Appointment
Prescription
Department
Billing
Pharmacy
Laboratory
```

The Domain layer should remain independent of the database and web framework.

---

### Application

```text
HospitalManagement.Application
```

Contains application-level business logic and contracts.

Current structure includes:

```text
DTOs
Interfaces
Mapping
Services
DependencyInjection.cs
```

The repository currently exposes these Application folders and files.

---

### Infrastructure

```text
HospitalManagement.Infrastructure
```

Responsible for infrastructure concerns such as:

* Entity Framework Core
* SQL Server
* Database context
* Repository implementations
* Entity configurations
* Migrations
* Data access

---

### Web

```text
HospitalManagement.Web
```

The ASP.NET Core MVC presentation layer.

Current project structure includes:

```text
Controllers
Models
Properties
Views
wwwroot
DependencyInjection.cs
Program.cs
appsettings.json
appsettings.Development.json
appsettings.Production.json
```

---

### Tests

```text
HospitalManagement.Tests
```

Contains automated tests for application functionality.

Run:

```bash
dotnet test
```

---

## 🔄 Application Flow

The general request flow is:

```text
Browser
   │
   ▼
HospitalManagement.Web
   │
   ▼
Controllers
   │
   ▼
Application Services
   │
   ▼
Interfaces / DTOs
   │
   ▼
Infrastructure
   │
   ▼
Entity Framework Core
   │
   ▼
SQL Server
```

This separation helps keep the application maintainable and easier to test.

---

## 🔐 Security Considerations

The project is intended for development and educational purposes.

Before production deployment, additional security controls should be implemented, including:

* Authentication
* Authorization
* Role-based access control
* Password hashing
* Input validation
* CSRF protection
* Secure session management
* HTTPS
* Secret management
* Audit logging
* Database backup and recovery
* Protection of patient/medical information
* Production error handling

### Important

Do not commit:

```text
Passwords
API keys
Connection strings containing credentials
Private certificates
Production secrets
Personal patient information
```

---

## 📸 Screenshots

Project screenshots are maintained under:

```text
docs/Screenshots/
```

Screenshots can be added here as the application UI is completed.

Example:

```markdown
![Dashboard](docs/Screenshots/dashboard.png)
```

---

## 🗺️ Development Roadmap

### Phase 1 — Foundation

* [x] Create GitHub repository
* [x] Create solution
* [x] Create layered projects
* [x] Create database scripts
* [x] Configure basic project structure

### Phase 2 — Core Modules

* [ ] Patient management
* [ ] Doctor management
* [ ] Department management
* [ ] Appointment management
* [ ] Prescription management

### Phase 3 — Extended Modules

* [ ] Billing
* [ ] Pharmacy
* [ ] Laboratory
* [ ] Reports
* [ ] Dashboard

### Phase 4 — Security

* [ ] Authentication
* [ ] Authorization
* [ ] Role-based access control
* [ ] Secure configuration
* [ ] Audit logging

### Phase 5 — Quality

* [ ] Unit testing
* [ ] Integration testing
* [ ] Error handling
* [ ] Logging
* [ ] Validation
* [ ] Performance improvements

### Phase 6 — Deployment

* [ ] Docker configuration
* [ ] Production configuration
* [ ] CI/CD pipeline
* [ ] Cloud deployment
* [ ] Production database
* [ ] Monitoring

---

## 🤝 Contributing

Contributions and suggestions are welcome.

### Development Workflow

Create a feature branch:

```bash
git checkout -b feature/your-feature
```

Make your changes and commit:

```bash
git add .
git commit -m "feat: add your feature"
```

Push the branch:

```bash
git push origin feature/your-feature
```

Then open a Pull Request on GitHub.

### Recommended Commit Format

Use clear commit messages such as:

```text
feat: add patient management
fix: resolve appointment validation issue
docs: update project documentation
refactor: improve repository implementation
test: add patient service tests
chore: update dependencies
```

---

## 👨‍💻 Author

**Deepak Lodhi**

GitHub:

https://github.com/lucifer0854m

Project:

https://github.com/lucifer0854m/HospitalManagementSystem

---

## 📄 License

This project is licensed under the **MIT License**.

See the `LICENSE` file for details.

---

## ⚠️ Disclaimer

This project is a software development/portfolio project and is **not intended to replace a certified hospital information system or professional medical software**.

It should not be used with real patient data without appropriate security, privacy, compliance, testing, auditing, and organizational controls.

---

## ⭐ Support

If you find this project useful for learning or development, consider giving the repository a ⭐ on GitHub.

---

**Built with C#, ASP.NET Core, SQL Server, Entity Framework Core, and a focus on clean, maintainable software architecture.**
