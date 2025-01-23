# National Library Management System (NLMS)

## Overview
The *National Library Management System (NLMS)* is a modular platform designed for efficient library management. It consists of two microservices:

- *National Library Core Service*: Manages staff, members, lending transactions, and reporting.
- *Catalogue Management Service*: Handles cataloging library resources like books, newspapers, articles, and more.

These services work together to provide seamless operations and resource tracking for the library system.

---
## Prerequisites
Before running the system, ensure the following software and configurations are installed:

1. *.NET 8*: Install the latest version.
2. *MongoDB*: Ensure MongoDB is installed and running locally (localhost).
3. *MS SQL Server*: Install and configure locally.
4. *SQL Server Management Studio*: Use to manage your SQL Server databases.
5. *Database Import*:
   - Import NationalLibraryCoreServiceDb.bacpac into your local MS SQL Server to populate sample data for the Core Service.
6. *Catalogue Management Service Initialization*:
   - On the first run, the service automatically creates a database with sample data.

---

## Architecture

### System Architecture
The system uses a modular, client-server-based architecture.

- *[View System Architecture Diagram](https://online.visual-paradigm.com/share.jsp?id=333833353839302d35)*
### Entity-Relationship Diagram (ERD)
The ERD provides insights into how entities are related within the National Library Core Service.

- *[View ERD Diagram](https://online.visual-paradigm.com/share.jsp?id=333833353839302d34)*

---

## Data Models

### National Library Core Service Data Model
- *[View Data Model](https://online.visual-paradigm.com/w/ykbksizl/diagrams/#G1pQJ_pdzKNv7OzU-sRa4RL-OtqN0n3zuv)*
### Catalogue Management Service Data Collection
```json
{
  "_id": "ObjectId",
  "Id": "string",
  "title": "string",
  "type": "string",
  "genres": ["string"],
  "format": "string",
  "isBorrowable": "boolean",
  "borrowRules": {
    "borrowLimitInDays": "int"
  },
  "catalogedBy": "string",
  "borrowStatus": "string",
  "authors": ["string"],
  "publishers": ["string"]
}
# Services

## National Library Core Service

### Setup
- Ensure MS SQL Server is running locally.
- Populate sample data from the provided database file.
- Import the database in SQL Server using `NationalLibraryCoreServiceDb.bacpac`.

### Authentication
- Authenticate using a JWT token.
- Example credentials:
  - **Email:** rmaneno@gmail.com
  - **Password:** Maneno@265

### Features
- **CRUD operations for staff:** Add, update, view, and delete staff records.
- **CRUD operations for staff details:** Manage individual staff information.
- **User management:** Handle users and their roles.
- **Membership management:** Manage library memberships.
- **Lending transactions:**
  - Validates membership.
  - Checks resource availability via the Catalogue Management Service.

## Catalogue Management Service

### Features
- **CRUD operations for catalog resources:** Add, update, view, and delete catalog items.
- **Resource tracking:** Track the availability and status of library resources.
- **Integration with Core Service:** Shares resource data with the National Library Core Service.

### Tools
- **Swagger:**
  - Provides API documentation.
  - Enables testing endpoints directly in the browser.

## Tools and Technologies Used

### Backend
- ASP.NET Core 8
- MongoDB
- MS SQL Server

### Frontend
- Swagger (for API documentation and testing)

### Authentication
- JWT Tokens

