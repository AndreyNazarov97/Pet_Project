FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PetProject.API/PetProject.API.csproj", "PetProject.API/"]
COPY ["PetProject.Application/PetProject.Application.csproj", "PetProject.Application/"]
COPY ["PetProject.Domain/PetProject.Domain.csproj", "PetProject.Domain/"]
COPY ["PetProject.Infrastructure/PetProject.Infrastructure.csproj", "PetProject.Infrastructure.Postgres/"]
COPY ["PetProject.Infrastructure.Authorization/PetProject.Infrastructure.Authorization.csproj", "PetProject.Infrastructure.Authorization/"]
RUN dotnet restore "PetProject.API/PetProject.API.csproj"
WORKDIR "/src"
COPY . .
RUN dotnet build "PetProject.API/PetProject.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/PetProject.API"
RUN dotnet publish "PetProject.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetProject.API.dll"]
