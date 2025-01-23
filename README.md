# NationalLibraryManagementSystem

Overview
The National Library Management System is a comprehensive solution designed to manage library operations efficiently. The system consists of two microservices:
National Library Core Service: Handles staff management, membership management, resource lending, and reporting.
Catalogue Management Service: Manages library resources, including books, newspapers, articles, and more.


Prerequisites
Before running the National Library Management System, ensure you have the following prerequisites installed and configured:
.NET 8: Install the latest version of .NET 8 on your system.
MongoDB: Install MongoDB and ensure it is running on localhost.
MS SQL Server: Install MS SQL Server on localhost.
SQL Server Management Studio: Install SQL Server Management Studio to manage your databases.
Database Import: Import the necessary database backup file (NationalLibraryCoreServiceDb.bacpac) into your local MS SQL Server instance.
Note : When running the CatalogueManagementService for the first time, it will automatically create a database with sample data. This is a one-time process that sets up the database for use with the service.

Architectures

System Architecture 
The system architecture is modular and web/client-server based. 
View System Architecture Diagram https://online.visual-paradigm.com/share.jsp?id=333833353839302d35

ERD Diagram for National Library Core Service
View the diagram https://online.visual-paradigm.com/share.jsp?id=333833353839302d34


Data Models
1. National Library Core Service Data Model
   https://online.visual-paradigm.com/w/ykbksizl/diagrams/#G1pQJ_pdzKNv7OzU-sRa4RL-OtqN0n3zuv 
3. Catalogue Management Service Data Collection
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
  "publishers": ["string"]
}



Services
1. National Library Core Service
    Set Up
   Set upd MS QL IS RUNNING Locally
   populate sample data from databse file
   Import databse in sql server using NationalLibraryCoreServiceDb

   . Authetication using JWT token Login with rmaneno@gmai.com password : Maneno@265
   Crud for staffs,Crud for Staff details, User Manegement, Membership Management, alending trnactions( it validatdatesmembership, resource availability through from catalogue service

   2. Catalogue Management System
      crud ooperrations for catalogue resources, tracking them and sending data to National Library Core Service

Tools
Swagger

