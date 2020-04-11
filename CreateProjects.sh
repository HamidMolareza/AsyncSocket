#! /bin/bash

#Save project name for easy use.
projectName="AsyncSocket"

#Create sln.
dotnet new sln

#Create main classlib for the templates.
dotnet new classlib -o $projectName
dotnet sln add $projectName/$projectName.csproj

#Create xunit for test the classlib.
dotnet new xunit -o Test-$projectName
dotnet sln add Test-$projectName/Test-$projectName.csproj
#Add access reference.
dotnet add Test-$projectName/Test-$projectName.csproj reference $projectName/$projectName.csproj

#Create Consoles for see results, manual testing and debugging.
#Clinet
dotnet new console -o Console-ManualTests/Client
dotnet sln add Console-ManualTests/Client/Client.csproj
dotnet add Console-ManualTests/Client/Client.csproj reference $projectName/$projectName.csproj
#Listener
dotnet new console -o Console-ManualTests/Listener
dotnet sln add Console-ManualTests/Listener/Listener.csproj
dotnet add Console-ManualTests/Listener/Listener.csproj reference $projectName/$projectName.csproj

#Build project to make sure everything is right.
dotnet build
