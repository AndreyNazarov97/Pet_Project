dotnet-ef database drop -f -c AccountsDbContext -p .\src\Accounts\PetProject.Accounts.Infrastructure\ -s .\src\PetProject.Web\
dotnet-ef database drop -f -c VolunteerDbContext -p .\src\VolunteerManagement\PetProject.VolunteerManagement.Infrastructure\ -s .\src\PetProject.Web\
dotnet-ef database drop -f -c SpeciesDbContext -p .\src\SpeciesManagement\PetProject.SpeciesManagement.Infrastructure\ -s .\src\PetProject.Web\

dotnet-ef migrations remove -c AccountsDbContext -p .\src\Accounts\PetProject.Accounts.Infrastructure\ -s .\src\PetProject.Web\
dotnet-ef migrations remove -c VolunteerDbContext -p .\src\VolunteerManagement\PetProject.VolunteerManagement.Infrastructure\ -s .\src\PetProject.Web\
dotnet-ef migrations remove -c SpeciesDbContext -p .\src\SpeciesManagement\PetProject.SpeciesManagement.Infrastructure\ -s .\src\PetProject.Web\
dotnet-ef migrations remove -c VolunteerRequestsDbContext -p .\src\VolunteerRequests\VolunteerRequests.Infrastructure\ -s .\src\PetProject.Web\


dotnet-ef migrations add Accounts_Init -c AccountsDbContext -p .\src\Accounts\PetProject.Accounts.Infrastructure\ -s .\src\PetProject.Web\
dotnet-ef migrations add Volunteers_Init -c VolunteerDbContext -p .\src\VolunteerManagement\PetProject.VolunteerManagement.Infrastructure\ -s .\src\PetProject.Web\
dotnet-ef migrations add Species_Init -c SpeciesDbContext -p .\src\SpeciesManagement\PetProject.SpeciesManagement.Infrastructure\ -s .\src\PetProject.Web\
dotnet-ef migrations add VolunteerRequests_Init -c VolunteerRequestsDbContext -p .\src\VolunteerRequests\VolunteerRequests.Infrastructure\ -s .\src\PetProject.Web\


dotnet-ef database update -c AccountsDbContext -p .\src\Accounts\PetProject.Accounts.Infrastructure\ -s .\src\PetProject.Web\
dotnet-ef database update -c VolunteerDbContext -p .\src\VolunteerManagement\PetProject.VolunteerManagement.Infrastructure\ -s .\src\PetProject.Web\
dotnet-ef database update -c SpeciesDbContext -p .\src\SpeciesManagement\PetProject.SpeciesManagement.Infrastructure\ -s .\src\PetProject.Web\
dotnet-ef database update -c VolunteerRequestsDbContext -p .\src\VolunteerRequests\VolunteerRequests.Infrastructure\ -s .\src\PetProject.Web\

