## Get started in project

### How to init on server

1. Open terminal and go to project directory
2. `dotnet restore`

#### 1. Init environment variables

1. Change directory `cd ./Tools` open file `setenv.sh` (for OS Windows use `.cmd` files | not tested)
2. Edit file `setenv.sh` with your private information
3. Run bash script `. setenv.sh` 
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
