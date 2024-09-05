# CCSV.Diaries

A simple set of example classes and test cases.

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

## Documentation

[Diaries API Doc](https://github.com/yacineMTB/dingllm.nvim?tab=readme-ov-file#documentation "Diaries API Doc")
