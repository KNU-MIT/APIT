# APIT - 2020
> version 1.4.6 beta

| Role       | Developer           | GitHub    | 
|------------|---------------------|-----------| 
| back-end   | Yurii Yermakav      | JuriLents | 
| front-end  | Miroslav Toloshnyi  | MiTo      | 

and other...

-----------------------------------------------------------------------------------

## Get started in project

### How to init on server

1. Open terminal and go to project directory

#### 1. Init environment variables

1. `dotnet restore`
2. Change directory `cd ./Tools` open file `setenv.sh` (for OS Windows use `.cmd` files | not tested)
3. Edit file `setenv.sh` with your data
4. Run bash script `. setenv.sh` 
>  If not works, configure environment variables manually or define they in the appsettings.json (not recommended)

> *MAC OS*: `brew install mono-libgdiplus`

#### 2. Init MS SQL database

1. `dotnet tool install --global dotnet-ef` - install EntityFramework globally (if it not installed)
2. `cd ./DatabaseLayer`
3. `dotnet ef database update`
> If you want to use some other DB, you could use other way to apply migrations

#### 3. Run server

> Run from your IDE OR from terminal
1. `cd ./Apit`
2. `dotnet run`

### Init env variables and other support data


-----------------------------------------------------------------------------------

### Add new DB migration

|   | Command                                      | Description                          | 
|---|----------------------------------------------|--------------------------------------| 
| 1 | `dotnet ef migrations add [migration_name]`  | add migration and name it            | 
| 2 | `dotnet ef database update`                  | update database via stored migration | 
| 3 | `. ./migration.sh [migration_name] [-v]`     | run bash script (windows . => bash)  | 
------------------------------------------------------------------------------------------- 
