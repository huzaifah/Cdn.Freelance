# Complete Developer Network (CDN) Freelancers

This app is to manage the CDN freelancers information:
* Create new user
* Update existing user
* Delete user
* Get all users (with pagination)
* Get single user information

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

Install the following tools before running the app.

```
1. Visual Studio 2022
2. pgAdmin tool
3. Postman (optional)
```

### Create the database

Create a new Postgres database and name it `freelance`.

Execute the database scripts located in `..\Cdn.Freelance\Scripts` to create the necessary tables.

### Update application settings

Update the appsettings.json file for database connection.

```
"ConnectionStrings": {
    "FreelanceDatabase": "Host=localhost;Port=5432;Database=freelance;User Id=postgres;Password={your_password};"
},
```

This solution is integrated with Okta Open ID Connect provider.

Create an Okta developer account (https://developer.okta.com/signup/) and add the following settings in your solution with the correct Okta domain for your Okta developer account.

```
"Okta": {
    "OktaDomain": "https://dev-50457353.okta.com",
    "AuthorizationServerId": "default",
    "Audience": "api://default"
}
```

Create a custom scope in Okta to access this app. The scope should be named as `cdn.freelance`.

### Running the app

Execute `Cdn.Freelance.Api` project with `https`.

```
dotnet run --project C:\Cdn.Freelance\Cdn.Freelance.Api\Cdn.Freelance.Api.csproj --launch-profile https
```

Enter this URL to view the API documentation page.

```
https://localhost:7099/swagger
```

Use your favourite REST client tool to make request to available endpoints.

Ensure that you have valid token before making the request.

## Unit Tests

There are three unit tests developed:

* Domain - Domain business logic and validation
* Infrastructure - Repository using Entity Framework In Memory
* API - Command, Query and Handlers

You can execute unit tests with following command:

```
dotnet test --collect:"XPlat Code Coverage"
```

Result of the unit tests and code coverage are collected using Coverlet package.

## Built With

* [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) - Integrated Development Environment
* [ASP.NET Core 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) - .NET Runtime Version

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/huzaifah/Cdn.Freelance/tags). 

## Author

* **Huzaifah Dzulkifli** - *Initial work*