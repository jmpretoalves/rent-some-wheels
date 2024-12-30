# Intro
A ASP.NET Core MVC project setup in WSL Debian done in a day, with a focus on using lightweight and fast tools to set up a full stack development environment in less than a few minutes.

# About
Rent Some Wheels is a web application that allows for the management of vehicles, clients, and rental contracts, as described below:
- **Vehicle Registration:**
  Enable the registration of vehicles with information such as brand, model, license plate, year of manufacture, and fuel type.
- **Client Registration:**
  Enable the registration of clients, including data such as full name, email, phone number, and driving license.
- **Rental Contract Registration:**
  Enable the creation of rental contracts, associating a client with a vehicle and including fields for start and end dates of the rental.
- **Listing of Vehicles, Clients, and Contracts:**
  Enable the visualization of vehicle, client, and contract lists, showing the status of the vehicles (available or rented).

# Overview of Tech Stack
- **OS:** Debian 12 (WSL setup in Windows 11)
- **Platform:** ASP.NET Core MVC (.NET Core 8.0)
- **Backend:** C#
- **Frontend:** Blazor, HTML, CSS, JavaScript
- **Database:** SQL Server
- **Database Handler:** Entity Framework (ORM)
- **Approach:** Code-First

# Validations

### Vehicle Registration
- **Mandatory Fields:**
  - Brand
  - Model
  - License Plate
  - Year of Manufacture
  - Fuel Type

- **Business Rules:**
  - **Year of Manufacture:** The manufacturing year cannot be later than the current year.
  - **License Plate:** The license plate must be unique (no two vehicles can have the same plate). The license plate follows the current Portugal format.

### Client Registration
- **Mandatory Fields:**
  - Full Name
  - Email
  - Phone Number
  - Driving License

- **Business Rules:**
  - **Email:** Must be unique in the system (no two clients can have the same email).
  - **Phone Number:** Must contain only numbers and have a valid phone number format. The phone number follows the current Portugal format.

### Rental Contract Registration
- **Mandatory Fields:**
  - Client: Must select a client from the list (required).
  - Vehicle: Must select a vehicle from the list (required).
  - Start Date, End Date, Initial Mileage: Required.

- **Business Rules:**
  - **Start Date:** Must be a valid date and cannot be earlier than the current date.
  - **End Date:** Must be later than the start date.

### Listing of Vehicles, Clients, and Contracts
- **Business Rules:**
  - **Vehicle Status (Available/Rented):** The status must be updated automatically based on rental contracts. If the vehicle is currently rented, it should be marked as "Rented." Otherwise, "Available."

# Development Setup

Coded using Visual Studio Code instead of the more common Visual Studio for these types of projects, since VS Code is a lot faster to install and auto-installs the needed extensions for using a remote connection to a WSL Debian environment where a lot of CLI is applied to quickly and easily set up the project, to showcase the multi-platform compatibility that .NET Core allows.

### Setup Instructions

After a `wsl install -d Debian` in PowerShell and creating a user inside the new Debian terminal, the following commands were used, by order:

```
sudo apt update && sudo apt upgrade -y
sudo apt install -y wget gpg

wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm -rf packages-microsoft-prod.deb
curl https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
curl https://packages.microsoft.com/config/debian/12/mssql-server-2022.list | sudo tee /etc/apt/sources.list.d/mssql-server.list

sudo apt update && sudo apt install -y dotnet-sdk-8.0 mssql-server mssql-tools unixodbc-dev git

sudo /opt/mssql/bin/mssql-conf setup

echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc
source ~/.bashrc

dotnet tool install --global dotnet-ef

dotnet new mvc -n "RentSomeWheels" | code RentSomeWheels/
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design

dotnet watch run
```

All these commands will:
- Update all installed packages
- Install wget, git
- Use wget and dpkg to add list of Microsoft packages
- Install .NET 8.0 SDK, SQL Server & tools
- Go through SQL Server setup
- Add to PATH environment some missing commands
- Install .NET Entity Framework globally
- Create a .NET project using ASP.NET Core MVC template
- Enter to project directory in VS Code
- Use .NET CLI to install required packages
- Build & Run the project with hot reload so you can start making changes in code and view them instantly

# How to run
Make sure you have .NET 8.0 SDK + Entity Framework + SQL Server installed, and change in `appsettings.json` the connection string in "RentSomeWheelsContext" to a SQL Server database that is up and running, or create a new one using the following info:
  - Server = localhost;
  - Database = RentSomeWheelsDB;
  - User Id = sa;
  - Password = Ola123..;

Then use the following commands in terminal to build & run the first time:
```
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    dotnet run
```
# TODO
- Review the Model classes, and describe validations with code snippets.
- Add UML Documentation and a Docs/ folder
  - Use Case Diagram
  - Class Diagram
  - Sequence Diagram
  - Entity-Relationship Diagram (ERD)
  - Activity Diagram
- Create unit tests with XUnit
- Add more validations to inputs
- Maybe add parameter mileage to Vehicle? And input the initial mileage in a new Rental Contract based on the Vehicle current mileage
- Install & configure Docker