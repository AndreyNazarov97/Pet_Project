namespace PetProject.Domain.Shared;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid");
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for id {id}";
            return Error.NotFound("record.not.found", $"The record{forId} was not found");
        }
        
        public static Error ValueIsRequired(string? name = null)
        {
            var label = name == null ? "" : " " + name + " ";
            return Error.Validation("length.is.invalid", $"invalid{label}length");
        }
    }
    public static class Volunteer
    {
        public static Error PhoneNumberAlreadyExists()
        {
            return Error.Validation(
                "record.already.exists", 
                "Phone number already exists");
        }
    }
}