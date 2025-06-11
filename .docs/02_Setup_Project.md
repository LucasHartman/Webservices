# Setup Project

## Project Directory
* Create project directory: `mkdir Webservice`

## Project
* Create projects:
```
dotnet new classlib -n My.Database
dotnet new classlib -n My.Shared
dotnet new console -n My.App
dotnet new xunit -o My.App.Tests
dotnet new webapi -n My.App.Web
```
* Add project references:
```
dotnet add reference ../My.Database/My.Database.csproj
dotnet add reference ../My.Shared/My.Shared.csproj
dotnet add reference ../My.App/My.App.csproj
```
* Make My.App.csproj: `Microsoft.NET.Sdk.Web`

## Solution
* Create solution: `dotnet new sln -n Webservices`
* Add projects to solution: 
```
dotnet sln Webservices.sln add My.Database/My.Database.csproj
dotnet sln Webservices.sln add My.Shared/My.Shared.csproj
dotnet sln Webservices.sln add My.App/My.App.csproj
dotnet sln Webservices.sln add My.App.Tests/My.App.Tests.csproj
dotnet sln Webservices.sln add My.App.Web/My.App.Web.csproj
```

## Package
* Install:
```
# My.Database
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Tools

# My.App
dotnet add package AutoMapper --version 14.0.0

# My.App.Tests
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.EntityFrameworkCore
```

## Sqlite
* Commands:
```
# Enable EF Core tools
dotnet tool install --global dotnet-ef

# Create and apply a migration

```

## Git
* Create git ignore file (windows): `New-Item -Path . -Name ".gitignore" -ItemType "File"`
* Add to the git ignore file:
```
# Ignore build directories
bin/
obj/

# Ignore user-specific files
*.user

# Ignore publish directory
publish/
/.vs/Webservice/FileContentIndex

# Ignore all appsettings
**/appsettings.json

```
* Commands:
```
# Create Git
git init

# Bind to Github
git remote add origin https://github.com/LucasHartman/Webservices.git

# add file to git
git add .

# get status
git status

# commit
git commit -m "notes"

# push
git push

# Hard rest
git reset --hard HEAD
```

## Publish
* Push from the solution directory: `dotnet publish -c Debug -r win-x64 --self-contained true -o publish`
* Run the published app: `.\publish\My.App.Web.exe`