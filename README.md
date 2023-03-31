# A.I. as a Service

This documentation describes how to use the OpenAI.cs SDK to interact with OpenAI's API for language model fine-tuning and other operations.

# Data Access Layers (SQL Server and CosmosDB)
With this implementation, you can easily switch between different data stores by simply changing the registered repository implementation in the dependency injection container.
I have implemented a custom repsitory class for using Entity Framework or CosmosDB.

# Entity Framework Core (EF Core)

Entity Framework Core (EF Core) can automatically create the database schema for you based on your defined models and DbContext. This feature is called Code First approach, which allows you to generate the database schema from your code rather than defining the schema manually.

When using Code First approach, you define your models and DbContext in code, and then EF Core can create the necessary tables, columns, relationships, and other database objects based on your definitions.

To create the database schema, you'll use EF Core migrations. Migrations are a way to apply incremental changes to the database schema while preserving existing data.

Here's how to create and apply migrations for your application:

    Install the EF Core CLI tools by running the following command:

csharp

dotnet tool install --global dotnet-ef

    In the root folder of your project, run the following command to create the initial migration:

csharp

dotnet ef migrations add InitialCreate

This command will generate a new folder called Migrations with a C# file that represents the initial migration.

    Next, apply the migration to create the database schema:

sql

dotnet ef database update

This command will create the database (if it doesn't already exist) and apply the migration to create the necessary tables, columns, and relationships.

When you make changes to your models or DbContext, you can create new migrations to update the database schema. Just run the dotnet ef migrations add command followed by a descriptive name for the migration. Then, use the dotnet ef database update command to apply the new migration.

Keep in mind that although EF Core can create and manage the database schema for you, it's still important to have a good understanding of SQL and database design principles to ensure your application performs well and can scale as needed.

Additionally, EF Core supports other approaches like Database First, which allows you to reverse-engineer your code based on an existing database schema. This is useful when you prefer to create the database schema manually or when working with legacy databases.