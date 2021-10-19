FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
ADD bin/Debug/net5.0/ app/
WORKDIR /app
ENTRYPOINT ["dotnet","Login.FrontendService.dll"]