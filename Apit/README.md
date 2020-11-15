# APIT - 2020
> version 1.4.1 beta

| Role       | Developer           | GitHub    | 
|------------|---------------------|-----------| 
| back-end   | Yurii Yermakav      | JuriLents | 
| front-end  | Miroslav Toloshnyi  | MiTo      | 

and other...

-----------------------------------------------------------------------------------

## Get started in project

### How to init on server

1. Open terminal and go to project directory
2. Change directory `cd ./Tools/`
3. Edit file `setenv.sh` with your data
4. Run bash script `. setenv.sh` 
5. 

### Init env variables and other support data

1. `cd [Apit]`

2. `dotnet restore`

*MAC OS: `brew install mono-libgdiplus`

-----------------------------------------------------------------------------------

### Add new DB migration

|   | Command                                      | Description                          | 
|---|----------------------------------------------|--------------------------------------| 
| 1 | `dotnet tool install --global dotnet-ef`     | install EntityFramework globally     | 
| 2 | `cd [DatabaseLayer]`                         | change directory                     | 
| 3 | `dotnet ef migrations add [migration_name]`  | add migration and name it            | 
| 4 | `dotnet ef database update`                  | update database via stored migration | 
| 5 | `. ./migration.sh [migration_name] [-v]`     | run bash script (windows . => bash)  | 
------------------------------------------------------------------------------------------- 
