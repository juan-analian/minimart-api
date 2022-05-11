# Minimart-api
.net backend api

## Requirements to run the project
* Visual Studio / VS Code
* SQL Server Express

## Instructions 
The solution consist of 3 projects
* Web Service (net core 3.1 web api)
* Business Layer  (dll)
* Unit tests for Web service and Business Layer (xUnit)

### 1.Clone this project:
```
git clone https://github.com/juan-analian/minimart-api.git
```

### 2.Configure Database 
Create a DataBase with the name "minimart" in your Sql Server, and change the connection string in appsettings.json file. 
The user must have elevated privileges to create al the objects (tables, constrains, storde procedures)

### 3.Run the project
Open the solution in Visual Studio and run the project (web.api) with IIS Express.
Open the browser and navigate to this url: https://localhost:44366/swagger/

### 4.Initial Setup
Call the api with a GET verb to this resource: /setup  
This end point create the required tables and insert initial data.

## API documentation
/swagger

## DER
https://raw.githubusercontent.com/juan-analian/minimart-api/main/der.png
 

## TODO!: (pendings)
* Paging, sorting and filtering lists.
* Add logs in catch blocks
* Controllers: unify return types for all methods
* Controllers: create a BaseController to define route versioning /v1/api/*
* Controllers: change response type when ModelState is not valid
* Add authentication
* Compleete Unit Test: for Cart Controler 
* Compleete Unit Test: for Cart Services
