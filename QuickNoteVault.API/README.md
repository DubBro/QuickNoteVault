# QuickNoteVault.API

This is the API of the QuickNote Vault built with ASP.NET Core 8.

## Prerequisites

Ensure you have the following installed on your system:

- [.NET SDK 9.0.101](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [SQL Server 2022](https://www.microsoft.com/en/sql-server/sql-server-downloads)

## Setup

1. Clone the repository

```bash
git clone https://github.com/DubBro/QuickNoteVault.git
```

2. Open folder with API project

```bash
cd QuickNoteVault/QuickNoteVault.API/QuickNoteVault.API
```

3. Run the application

```bash
dotnet run
```

4. Access the API in your browser or via a tool like Postman

```bash
https://localhost:7294
```

## Usage

1. **API documentation:** Use the Swagger interface at `/swagger` to explore and test API endpoints interactively.

2. **List of endpoints**:

- `GET /Note/Get/{id}`: Retrieve a note by Id.
- `GET /Note/GetAll/{userId}`: Retrieve all notes of a user by user's Id.
- `POST /Note/Add`: Add a new note.
- `PUT /Note/Update`: Update an existing note.
- `DELETE /Note/Delete/{id}`: Delete a note by Id.
