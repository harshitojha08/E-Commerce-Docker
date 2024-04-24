#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Product Service/Product Service.csproj", "Product Service/"]
RUN dotnet restore "Product Service/Product Service.csproj"
COPY . .
WORKDIR "/src/Product Service"
RUN dotnet build "Product Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Product Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product Service.dll"]