# Technical Test: HR Medical Records Management System

## Objective
Develop a medical records management system for the HR department by implementing a RESTful API that allows management of employee medical records.

## Mandatory Technical Requirements
- .NET 8
- PostgreSQL **(local environment)**
- Entity Framework Core (Database First, Fluent API)
- AutoMapper
- Repository Pattern
- Service Pattern
- Swagger for API documentation
- FluentValidation

## Database Structure

### Database Name: **RRHH_DB**

### Table: T_MEDICAL_RECORD
```sql
CREATE TABLE T_MEDICAL_RECORD (
    MEDICAL_RECORD_ID SERIAL PRIMARY KEY,
    FILE_ID INTEGER, -- FILE_ID represents the person to whom the MEDICAL_RECORD belongs, but it is not found in the database.
    AUDIOMETRY VARCHAR(2),
    POSITION_CHANGE VARCHAR(2),
    MOTHER_DATA VARCHAR(2000),
    DIAGNOSIS VARCHAR(100),
    OTHER_FAMILY_DATA VARCHAR(2000),
    FATHER_DATA VARCHAR(2000),
    EXECUTE_MICROS VARCHAR(2),
    EXECUTE_EXTRA VARCHAR(2),
    VOICE_EVALUATION VARCHAR(2),
    DELETION_DATE DATE,
    CREATION_DATE DATE,
    MODIFICATION_DATE DATE,
    END_DATE DATE,
    START_DATE DATE,
    STATUS_ID INTEGER,
    MEDICAL_RECORD_TYPE_ID INTEGER,
    DISABILITY VARCHAR(2),
    MEDICAL_BOARD VARCHAR(200),
    DELETION_REASON VARCHAR(2000),
    OBSERVATIONS VARCHAR(2000),
    DISABILITY_PERCENTAGE NUMERIC(10),
    DELETED_BY VARCHAR(2000),
    CREATED_BY VARCHAR(2000),
    MODIFIED_BY VARCHAR(2000),
    AREA_CHANGE VARCHAR(2)
);

CREATE TABLE STATUS (
    STATUS_ID SERIAL PRIMARY KEY,
    NAME VARCHAR(100),
    DESCRIPTION VARCHAR(500)
);

CREATE TABLE MEDICAL_RECORD_TYPE (
    MEDICAL_RECORD_TYPE_ID SERIAL PRIMARY KEY,
    NAME VARCHAR(100),
    DESCRIPTION VARCHAR(500)
);

ALTER TABLE T_MEDICAL_RECORD
ADD CONSTRAINT FK_STATUS_ID_RECORD
FOREIGN KEY (STATUS_ID) REFERENCES STATUS(STATUS_ID);

ALTER TABLE T_MEDICAL_RECORD
ADD CONSTRAINT FK_MEDICAL_RECORD_TYPE
FOREIGN KEY (MEDICAL_RECORD_TYPE_ID) REFERENCES MEDICAL_RECORD_TYPE(MEDICAL_RECORD_TYPE_ID);

-- Initial test data
INSERT INTO STATUS (NAME, DESCRIPTION) VALUES 
('Active', 'Active medical record'),
('Inactive', 'Inactive medical record');

INSERT INTO MEDICAL_RECORD_TYPE (NAME, DESCRIPTION) VALUES 
('Regular', 'Regular medical record'),
('Special', 'Special medical record');
```

## Functional Requirements

### 1. Endpoints to Implement
- **GetFilterMedicalRecords**: List of medical records with pagination and filters. It should be possible to filter by STATUS_ID (optional), START_DATE (optional), END_DATE (optional), and MEDICAL_RECORD_TYPE_ID (optional).
    Page and PageSize are mandatory parameters.
- **GetMedicalRecordById**: Retrieve medical record by ID. Get a more detailed and formatted description of the medical record.
- **AddMedicalRecord**: Create new medical record. Mandatory fields must be validated and comply with validation rules.
- **UpdateMedicalRecord**: Update existing medical record. Mandatory fields must be validated and comply with validation rules.
- **DeleteMedicalRecord**: Delete medical record (logical deletion)

### 2. Required Validations

#### 2.1 Date Controls
- START_DATE cannot be later than END_DATE
- START_DATE cannot be a future date
- If END_DATE exists, it must be later than START_DATE
- CREATION_DATE is mandatory and must be auto-generated when inserting the record

#### 2.2 Required Fields
The following fields are mandatory:
- DIAGNOSIS
- START_DATE
- STATUS_ID
- MEDICAL_RECORD_TYPE_ID
- FILE_ID
- CREATED_BY

#### 2.3 Related Records Validation
- STATUS_ID must exist in the STATUS table
- MEDICAL_RECORD_TYPE_ID must exist in the MEDICAL_RECORD_TYPE table
- A record cannot be deleted without providing DELETION_REASON
- When creating or modifying a record, the status must be valid according to these rules:
  * Cannot assign 'Inactive' status when creating a new record
  * To change to 'Inactive' status, DELETION_REASON must be provided

#### 2.4 Maximum Length Validation
- DIAGNOSIS: maximum 100 characters
- MOTHER_DATA: maximum 2000 characters
- FATHER_DATA: maximum 2000 characters
- OTHER_FAMILY_DATA: maximum 2000 characters
- MEDICAL_BOARD: maximum 200 characters
- DELETION_REASON: maximum 2000 characters
- OBSERVATIONS: maximum 2000 characters
- Two-character fields (must be 'YES' or 'NO'):
  * AUDIOMETRY
  * POSITION_CHANGE
  * EXECUTE_MICROS
  * EXECUTE_EXTRA
  * VOICE_EVALUATION
  * DISABILITY
  * AREA_CHANGE

#### 2.5 Valid Status Control
Allowed statuses and their rules:
1. Active (ID: 1)
   - Default initial status for new records
   - Allows modification of all fields
   - Requires CREATED_BY

2. Inactive (ID: 2)
   - Requires DELETION_REASON
   - Requires END_DATE
   - Requires DELETED_BY
   - Does not allow subsequent modifications
   - Must record DELETION_DATE

#### 2.6 Additional Validation Rules
- DISABILITY_PERCENTAGE must be between 0 and 100 when DISABILITY = 'YES'
- If POSITION_CHANGE = 'YES', OBSERVATIONS field is mandatory
- If END_DATE exists, the record must change to Inactive status
- CREATED_BY, MODIFIED_BY, and DELETED_BY must record the user performing the operation
- MODIFICATION_DATE must be automatically updated when modifying any field

### 3. Response Handling

#### 3.1 Base Response Model
```csharp
public class BaseResponse<T>
{
    public bool? Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public int? Code { get; set; }
    public int? TotalRows { get; set; }
    public string? Exception { get; set; }
}
```

#### 3.2 HTTP Response Codes
- 200 OK: Successful request (GET, PUT/PATCH)
- 400 Bad Request: Validation errors
- 404 Not Found: Resource not found
- 500 Internal Server Error: Unhandled errors

## Deliverables
1. Git repository with complete source code
2. Installation and execution instructions in README

## Evaluation Criteria
- Code structure and organization
- Correct implementation of requested patterns
- Validation and exception handling
- Proper use of AutoMapper
- Clean and properly commented code
- Correct application of Gitflow (Main -> Development -> Feature)

## Delivery Time
- 5 business days from test receipt
- Delivery will be confirmed through creation of a Release PR to main

## Installation Instructions
The candidate must provide clear instructions for:
1. Local PostgreSQL installation
2. Database script execution
3. Running migration with database-first approach using EF with Fluent API
4. Project configuration

## Extras (Not Mandatory)
- Docker

## Delivery Format
- Git repository (GitHub, GitLab, Bitbucket)
- Repository must include this README updated with specific installation instructions for your implementation

---
*Note: For any questions or clarifications about requirements, please create an issue in the repository.*
---
# Medical Records Management System - Installation Guide

## Table of Contents
1. [PostgreSQL Installation](#1-postgresql-installation)
2. [Database Setup](#2-database-setup)
3. [Project Configuration](#3-project-configuration)
4. [Entity Framework Setup](#4-entity-framework-setup)
5. [Running the Application](#5-running-the-application)

## 1. PostgreSQL Installation

### Windows
1. Download PostgreSQL installer:
   - Go to https://www.postgresql.org/download/windows/
   - Download the latest version (17.x or newer)
   - Run the installer

2. During installation:
   - Select all components
   - Set password for postgres user
   - Default port: 5432
   - Default locale

3. Verify installation:
   - Open Command Prompt
   - Type: `psql --version`
   - Should see version information

### MacOS
Using Homebrew:
   ```bash
   brew install postgresql@14
   brew services start postgresql@14
   ```

### Linux
```bash
sudo apt update
sudo apt install postgresql postgresql-contrib
sudo systemctl start postgresql
sudo systemctl enable postgresql
```

## 2. Database Setup

1. Create Database:
```sql
CREATE DATABASE RRHH_DB;
```

2. Execute the following SQL scripts in order:

```sql
CREATE TABLE STATUS (
    STATUS_ID SERIAL PRIMARY KEY,
    NAME VARCHAR(100),
    DESCRIPTION VARCHAR(500)
);

CREATE TABLE MEDICAL_RECORD_TYPE (
    MEDICAL_RECORD_TYPE_ID SERIAL PRIMARY KEY,
    NAME VARCHAR(100),
    DESCRIPTION VARCHAR(500)
);

CREATE TABLE T_MEDICAL_RECORD (
    MEDICAL_RECORD_ID SERIAL PRIMARY KEY,
    FILE_ID INTEGER,
    AUDIOMETRY VARCHAR(2),
    POSITION_CHANGE VARCHAR(2),
    MOTHER_DATA VARCHAR(2000),
    DIAGNOSIS VARCHAR(100),
    OTHER_FAMILY_DATA VARCHAR(2000),
    FATHER_DATA VARCHAR(2000),
    EXECUTE_MICROS VARCHAR(2),
    EXECUTE_EXTRA VARCHAR(2),
    VOICE_EVALUATION VARCHAR(2),
    DELETION_DATE DATE,
    CREATION_DATE DATE,
    MODIFICATION_DATE DATE,
    END_DATE DATE,
    START_DATE DATE,
    STATUS_ID INTEGER,
    MEDICAL_RECORD_TYPE_ID INTEGER,
    DISABILITY VARCHAR(2),
    MEDICAL_BOARD VARCHAR(200),
    DELETION_REASON VARCHAR(2000),
    OBSERVATIONS VARCHAR(2000),
    DISABILITY_PERCENTAGE NUMERIC(10),
    DELETED_BY VARCHAR(2000),
    CREATED_BY VARCHAR(2000),
    MODIFIED_BY VARCHAR(2000),
    AREA_CHANGE VARCHAR(2)
);

ALTER TABLE T_MEDICAL_RECORD
ADD CONSTRAINT FK_STATUS_ID_RECORD
FOREIGN KEY (STATUS_ID) REFERENCES STATUS(STATUS_ID);

ALTER TABLE T_MEDICAL_RECORD
ADD CONSTRAINT FK_MEDICAL_RECORD_TYPE
FOREIGN KEY (MEDICAL_RECORD_TYPE_ID) REFERENCES MEDICAL_RECORD_TYPE(MEDICAL_RECORD_TYPE_ID);

INSERT INTO STATUS (NAME, DESCRIPTION) VALUES
('Active', 'Active medical record'),
('Inactive', 'Inactive medical record');

INSERT INTO MEDICAL_RECORD_TYPE (NAME, DESCRIPTION) VALUES
('Regular', 'Regular medical record'),
('Special', 'Special medical record');
```

## 3. Project Configuration

1. Clone the repository:
```bash
git clone https://github.com/JulianMartinezz/PuebaTecnicaBackend-Gabriel-HerreraB.git
cd PuebaTecnicaBackend-Gabriel-HerreraB
cd Backend
```

2. Install .NET 8.0 SDK:
   - Download from: https://dotnet.microsoft.com/download/dotnet/8.0

3. Install required NuGet packages:
```bash
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.1
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.1
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.1
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 9.0.3
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
dotnet add package FluentValidation.AspNetCore --version 11.3.0
```

4. Configure appsettings.json:
```json
{
  "ConnectionStrings": {
    "PostgresConnection": "Host=localhost;Database=RRHH_DB;Username=postgres;Password=your_password"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## 4. Entity Framework Setup

1. Install EF Core tools globally:
```bash
dotnet tool install --global dotnet-ef
```

2. Scaffold the database:
```bash
dotnet ef dbcontext scaffold "Host=localhost;Database=RRHH_DB;Username=postgres;Password=your_password" Npgsql.EntityFrameworkCore.PostgreSQL -o Models -c HRDbContext -f --no-onconfiguring
```

3. Update DbContext to use configuration:
```csharp
public HRDbContext(DbContextOptions<HRDbContext> options) : base(options)
{
}
```

4. Verify Fluent API configurations in DbContext:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
}
```

## 5. Running the Application

1. Build the project:
```bash
dotnet build
```

2. Run the application:
```bash
dotnet run
```

3. Access Swagger UI:
- Open browser
- Navigate to: https://localhost:5001/swagger (or http://localhost:5000/swagger)

4. Verify API endpoints:
- GET /api/MedicalRecord/GetFilterMedicalRecords
- GET /api/MedicalRecord/{id}
- POST /api/MedicalRecord
- PUT /api/MedicalRecord
- DELETE /api/MedicalRecord