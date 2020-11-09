# APIT - 2020
> version 1.3.0 beta

| Role       | Developer           | GitHub    | 
|------------|---------------------|-----------| 
| back-end   | Yurii Yermakav      | JuriLents | 
| front-end  | Miroslav Toloshnyi  | MiTo      | 

and other...

-----------------------------------------------------------------------------------

## Get started in back-end


#### Init env variables and other support data

`cd [Apit]`

UNIX: `. ./setenv.sh`

WIN:  `setenv.cmd`


-----------------------------------------------------------------------------------

#### Add new DB migration

| Command                                   | Description                         | 
|-------------------------------------------|-------------------------------------| 
| `dotnet tool install --global dotnet-ef`  | install EntityFramework globally    | 
| `cd [DatabaseLayer]`                      | change directory                    | 
| `. ./migration.sh [migration_name] [-v]`  | run bash script (windows . => bash) | 
-----------------------------------------------------------------------------------
