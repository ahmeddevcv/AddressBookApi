Address Book API

A .NET Core Web API application for user registration, authentication, and personal contact management.

🔐 Authentication

All contact endpoints are protected using JWT Bearer authentication.
You must log in first to obtain a token.


<br/>
🚀 Features

User registration and login

JWT-based authentication

Secure password hashing

Personal address book per user

CRUD operations for contacts

Sorting and pagination for contacts

Docker Compose (API + SQL Server)

Swagger API documentation

Postman

<br/>


🛠️ Tech Stack

.NET 8 Web API

Entity Framework Core

SQL Server

JWT Authentication

Docker & Docker Compose

Swagger / OpenAPI

<br/>


📦 Installation & Setup
Prerequisites

.NET SDK 8

SQL Server (Local or Docker)

Docker Desktop (optional)

Postman (for testing)

<br/>



▶️ Run Locally (Without Docker)

1. Clone the repository:
```bash
git clone https://github.com/ahmeddevcv/AddressBookApi
```

🐳 Run with Docker

The project includes a Docker Compose setup to run the API and SQL Server in separate containers.

1. Build and start the containers:
```bash
docker-compose up --build
```