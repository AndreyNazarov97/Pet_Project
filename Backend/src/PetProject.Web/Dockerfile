FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PetProject.Web/PetProject.Web.csproj", "PetProject.Web/"]
COPY ["Accounts/PetProject.Accounts.Infrastructure/PetProject.Accounts.Infrastructure.csproj", "Accounts/PetProject.Accounts.Infrastructure/"]
COPY ["Accounts/PetProject.Accounts.Application/PetProject.Accounts.Application.csproj", "Accounts/PetProject.Accounts.Application/"]
COPY ["Accounts/PetProject.Accounts.Domain/PetProject.Accounts.Domain.csproj", "Accounts/PetProject.Accounts.Domain/"]
COPY ["Shared/PetProject.SharedKernel/PetProject.SharedKernel.csproj", "Shared/PetProject.SharedKernel/"]
COPY ["Shared/PetProject.Core/PetProject.Core.csproj", "Shared/PetProject.Core/"]
COPY ["Shared/PetProject.Framework/PetProject.Framework.csproj", "Shared/PetProject.Framework/"]
COPY ["Accounts/PetProject.Accounts.Presentation/PetProject.Accounts.Presentation.csproj", "Accounts/PetProject.Accounts.Presentation/"]
COPY ["SpeciesManagement/PetProject.SpeciesManagement.Infrastructure/PetProject.SpeciesManagement.Infrastructure.csproj", "SpeciesManagement/PetProject.SpeciesManagement.Infrastructure/"]
COPY ["SpeciesManagement/PetProject.SpeciesManagement.Application/PetProject.SpeciesManagement.Application.csproj", "SpeciesManagement/PetProject.SpeciesManagement.Application/"]
COPY ["SpeciesManagement/PetProject.SpeciesManagement.Domain/PetProject.SpeciesManagement.Domain.csproj", "SpeciesManagement/PetProject.SpeciesManagement.Domain/"]
COPY ["SpeciesManagement/PetProject.SpeciesManagement.Presentation/PetProject.SpeciesManagement.Presentation.csproj", "SpeciesManagement/PetProject.SpeciesManagement.Presentation/"]
COPY ["VolunteerManagement/PetProject.VolunteerManagement.Infrastructure/PetProject.VolunteerManagement.Infrastructure.csproj", "VolunteerManagement/PetProject.VolunteerManagement.Infrastructure/"]
COPY ["VolunteerManagement/PetProject.VolunteerManagement.Application/PetProject.VolunteerManagement.Application.csproj", "VolunteerManagement/PetProject.VolunteerManagement.Application/"]
COPY ["VolunteerManagement/PetProject.VolunteerManagement.Domain/PetProject.VolunteerManagement.Domain.csproj", "VolunteerManagement/PetProject.VolunteerManagement.Domain/"]
COPY ["VolunteerManagement/PetProject.VolunteerManagement.Presentation/PetProject.VolunteerManagement.Presentation.csproj", "VolunteerManagement/PetProject.VolunteerManagement.Presentation/"]
RUN dotnet restore "PetProject.Web/PetProject.Web.csproj"
WORKDIR "/src"
COPY . .
RUN dotnet build "PetProject.Web/PetProject.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PetProject.Web/PetProject.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetProject.Web.dll"]
