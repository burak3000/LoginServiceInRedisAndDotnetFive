FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
ADD bin/Release/net5.0/publish/ app/
WORKDIR /app
ENTRYPOINT ["dotnet","Login.BackendService.dll"]
