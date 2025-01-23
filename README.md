# National Library Management System (NaLib)

## Overview
The *National Library Management System (NaLib)* is a modular platform designed for efficient library management. It consists of two microservices:

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




# National Library Core Service

## Setup

1. Ensure **MS SQL Server** is running locally.
2. Populate sample data from the provided database file.
3. Import the database in SQL Server using `NationalLibraryCoreServiceDb.bacpac`.

## Authentication

- **JWT Token Authentication**:  
  All endpoints are protected and require a valid **JWT token** for access, except for the **User Creation** endpoint, which is publicly accessible. This allows for secure access control across the system.
  
- **Creating Users**:  
  The **User Creation** endpoint is publicly accessible, meaning anyone can create a user account. However, when creating a new user, the **Role Name** and **Library Name** must match exactly with those stored in the database to ensure consistency and prevent errors.  
  If the provided role name or library name does not match the database records, the user creation request will be rejected.

- **Role and Library Name Validation**:  
  The **Role Name** and **Library Name** for each user are validated against the existing data in the database. The creation request will be rejected if the provided names do not match exactly with those stored in the database.

- **Staff Management**:  
  CRUD operations for **staff details** (Add, Update, View, Delete) are protected by **JWT authentication**. Only authorized users with the necessary roles can perform these operations. This ensures the security and integrity of staff records.

## Features

### National Library Core Service

- **CRUD operations for staff**: Add, update, view, and delete staff records.
- **CRUD operations for staff details**: Manage individual staff information.
- **User management**: Handle users and their roles.
- **Membership management**: Manage library memberships.
- **Lending transactions**:
  - Validates membership.
  - Checks resource availability via the **Catalogue Management Service**.
  - Updates the **Catalogue Management Service** to reflect resource checkout and return, ensuring accurate tracking of resource availability.
  - **Checkout of Resources**: Allows users to checkout resources (e.g., books, magazines) from the library. The resource status is updated to "Checked Out" in the catalogue, and the user’s transaction is recorded.
  - **Return Transactions**: Tracks when a user returns a resource, updating its status in the catalogue to "Available" again.
  - **Tracking Lending Behavior**: Tracks the lending behavior of users, such as the frequency of checkouts, overdue resources, and resource history. This data can be used for monitoring user activity and identifying lending patterns.

### Catalogue Management Service

- **CRUD operations for catalog resources**: Add, update, view, and delete catalog items.
- **Resource tracking**: Track the availability and status of library resources.
- **Integration with Core Service**: Shares resource data with the **National Library Core Service**.

## Tools and Technologies Used

### Backend

- **ASP.NET Core 8**
- **MongoDB**
- **MS SQL Server**


- **Swagger** (for API documentation and testing)

### Authentication

- **JWT Tokens** for secure access to protected endpoints
