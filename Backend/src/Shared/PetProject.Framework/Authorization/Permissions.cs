namespace PetProject.Framework.Authorization;

public static class Permissions
{
    public static class Volunteer
    {
        public const string Create = "volunteers.create";
        public const string Read = "volunteers.read";
        public const string Update = "volunteers.update";
        public const string Delete = "volunteers.delete";
    }
    
    public static class VolunteerRequest
    {
        public const string Create = "volunteer_requests.create";
        public const string Read = "volunteer_requests.read";
        public const string Update = "volunteer_requests.update";
        public const string Delete = "volunteer_requests.delete";
    }
}