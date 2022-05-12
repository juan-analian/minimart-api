# Minimart-api
.net backend api

## Functional requirements
The system should:
• Be able to setup all data from a simple GET
• Be able to query available stores at a certain time in the day and return only those that apply 
• Be able to query all available products, across stores, with their total stock.
• Be able to query if a product is available, at a certain store, and return that product's info
• Be able to query available products for a particular store
• Be able to manage a simple virtual cart (add/remove from it). It cannot allow to add a product that has NO stock
• Be able to check the validity of a Voucher code on said virtual cart. Calculate discounts and return both original and discounted prices

## Requirements to run the project
* net core 3.1 sdk
* Visual Studio / VS Code
* SQL Server Express

## Packages
*Dapper
*AutoMapper
*Swashbuckle 
*Moq 

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

GET https://localhost:44366/swagger/

### Store service
GET https://localhost:44366/api/stores to get all stores with their open and closing hours and days

GET https://localhost:44366/api/stores?atHour=12 to get all stores opened at 12pm in any weekday.

GET https://localhost:44366/api/stores?atHour=12&weekDay=1 to get all stores opened at 12pm on mondays.

GET https://localhost:44366/api/stores?weekDay=1 to get all stores opened on mondays.


### Product service
GET https://localhost:44366/api/products to get all products. The stock is a sum of the stock for each product in each store.

GET https://localhost:44366/api/products?storeId=2 to get all products from a specifi store. The stock is related to this store.

GET https://localhost:44366/api/products/{productId}/stores/{storeId} to get a single product by id for a specific store.


### Cart service
POST https://localhost:44366/api/carts to create a cart asociated with a store and insert the 1st item

POST https://localhost:44366/api/carts/{cartId} to add item to an existing store

DELETE https://localhost:44366/api/carts/{cartId}/items/{productId} remove existing item from a cart.

PUT https://localhost:44366/api/carts/{cartId}/voucher/{voucherId} assing a voucher to an existing cart.

GET https://localhost:44366/api/carts/{cartId} to get a master detail info of the cart with prices and discount values - where the magic happens :)


## DER
[Database diagram](https://raw.githubusercontent.com/juan-analian/minimart-api/main/der.png)
 
## TODO!: (pendings)
* Loggin: Add logs in catch blocks
* Controllers: unify return types for all methods
* Controllers: create a BaseController to define route versioning  api/v1/*
* Controllers: change response type when ModelState is not valid
* Paging, sorting and filtering lists.
