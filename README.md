# Graidex (backend)

> This is backend repository. To see frontend, follow the link: https://github.com/kinmary/graidex-frontend

This is a student testing system that provides teachers with an effective evaluation tool. The platform will have specific features geared towards effective knowledge testing. This project will be performed in three stages: casual testing system creation, implementation of AI evaluation, and implementation of plagiarism checks.

## Technologies

- .NET 7
- ASP.NET Core
- Entity Framework Core
- MS SQL Server
- JWT tokens
- BCrypt
- AutoMapper
- FluentValidation
- OneOf
- Swagger UI
- NUnit
- Moq

## Startup

1. Open Visual Studio 2022 or later and clone this project.
2. Add `appsettings.json` file to `Graidex.API`. This file should be as follows:
```json
{
  "AppSettings": {
    "Token":  "[Key token string, 512 bits or more]",
    "FrontendUrl": "[Url for frontend]",
    "MongoDb": {
      "DatabaseName": "GraidexDb"
    },
    "OpenAIToken" : "[OpenAI token string]"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "GraidexDb": "[Database connection string]",
    "GraidexDb.MongoDb": "[MongoDb connection string]"
  }
}
```

3. Run the app (`Ctrl+F5`).

## Base functionality

The system will have two main types of users: teachers and students. Teachers will be able to create subjects and add students to the subjects. Teachers will be able to create, change, and grade tests, while students will be able to take them.

## AI features

There will be integrated a third-party AI-helper, which will assist teachers in automatically evaluating students' answers for open questions and providing them with feedback. While the AI-helper will handle the initial evaluation, teachers will have the option to manually adjust the evaluation as they see fit.

## Plagiarism checks

Another feature will be plagiarism checks using third-party tools. This will allow us to compare students' work with one another, as well as with online data, and check whether the answer was generated by AI. Depending on the settings set by the teacher, these checks could have an impact on students' grades.
