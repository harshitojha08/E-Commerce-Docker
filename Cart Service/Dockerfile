#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Cart Service/Cart Service.csproj", "Cart Service/"]
RUN dotnet restore "Cart Service/Cart Service.csproj"
COPY . .
WORKDIR "/src/Cart Service"
RUN dotnet build "Cart Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cart Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cart Service.dll"]