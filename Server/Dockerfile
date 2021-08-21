#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Use your choice of image as base. Mine is alpine! 
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY . .

RUN dotnet restore "IMMPlaH.API/IMMPlaH.API.csproj"
WORKDIR "/src/."
COPY . .
RUN dotnet build "IMMPlaH.API/IMMPlaH.API.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "IMMPlaH.API/IMMPlaH.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
CMD ASPNETCORE_URLS=http://*:$PORT dotnet IMMPlaH.API.dll
