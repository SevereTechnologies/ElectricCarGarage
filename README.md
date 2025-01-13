# ElectricCarGarage
Intro
- Electric garage is an Electric Vehicle management application for EVs of all types. It repairs, paint, maintained EVs for customers.

Services that make up the solution:
- Client
	- Architecture, Libraries and Patterns
		- Minimal API using Carter library, C#12, Vertical Slice Architecture, CQRS Design Pattern using MediatR library, Marten Library to convert .NET Transaction to DocumentDB in PostgreSQL, Mapster library for mapping DTO classes, FluentValidation for validating inputs and add MediatR validation pipeline. Logging and Exception handling with MediatR pipeline behavior for implemting cross cutting concern. Docker file and Docker-compose yaml file for containerizing and orchastring the DB and service
	- 
- Technician
- Inventory
- Payment
- Discount
- WorkOrder
- Authentication
- Email
- SAGA Orchastration
- Yarp API Gateway

Databases that make up the solution:
- Client (PostgreSQL)
- Technician (PostgreSQL)
- Inventory (PostgreSQL)
- Payment (SQLServer)
- Discount (SQLite)
- Authentication (SQL Server)
- SAGA (PostgreSQL)

Components/Libraries that make up the solution:
- Redis
- RabitMQ
- MassTransit
- EF
- grPC
- REST
- Docker
-

Big Picture
Client(Web,Mobile,desktop)
	-> API Gateway(Yarp API)
		-> Client Service
			-> Database(PostgreSQL)
		-> Technician Service
			-> Database(PostgreSQL)
		-> Inventory Service
			-> Database(PostgreSQL)
			-> Database(Redis)
			-> RabitMQ
		-> Payment Service
			-> Database(SQLServer)
			-> RabitMQ
		-> Discount Service
			-> Database(PostgreSQL)
		-> WorkOrder Service
			-> Database(SQLServer)
			-> RabitMQ
		-> Authentication Service
			-> Database(SQLServer)
		-> Email Service
