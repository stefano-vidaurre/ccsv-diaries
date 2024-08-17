# CCSV.Diaries

A simple set of example classes.

## Migrations

- Installing dotnet-ef tools:

~~~bash
dotnet tool install --global dotnet-ef
~~~

- Create migration command:

~~~bash
dotnet ef migrations add "<Migration Name>" --project "src/CCSV.Diaries" --context "InFileApplicationContext" -o "Contexts/Migrations/InFileApplicationContextMigrations" 
~~~

- Update migration command:

~~~bash
dotnet ef database update --project "src/CCSV.Diaries.UI.Api" --context "InFileApplicationContext" 
~~~
