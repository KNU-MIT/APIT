# APIT - 2020
> version 1.3.6 beta

| Role       | Developer           | GitHub    | 
|------------|---------------------|-----------| 
| back-end   | Yurii Yermakav      | JuriLents | 
| front-end  | Miroslav Toloshnyi  | MiTo      | 

and other...

-----------------------------------------------------------------------------------

## Get started in back-end


#### Init env variables and other support data

1. `cd [Apit]`

2. `dotnet restore`

*MAC OS: `brew install mono-libgdiplus`

-----------------------------------------------------------------------------------

#### Add new DB migration

|   | Command                                      | Description                          | 
|---|----------------------------------------------|--------------------------------------| 
| 1 | `dotnet tool install --global dotnet-ef`     | install EntityFramework globally     | 
| 2 | `cd [DatabaseLayer]`                         | change directory                     | 
| 3 | `dotnet ef migrations add [migration_name]`  | add migration and name it            | 
| 4 | `dotnet ef database update`                  | update database via stored migration | 
| 5 | `. ./migration.sh [migration_name] [-v]`     | run bash script (windows . => bash)  | 
------------------------------------------------------------------------------------------- 
