FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY CoffeeShopTalk.WebApi/bin/Release/netcoreapp3.1/publish/ .
ENV ASPNETCORE_ENVIRONMENT Staging
ENTRYPOINT ["dotnet", "CoffeeShopTalk.WebApi.dll"]