namespace PetProject.SharedKernel.Shared;

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
            var forId = id == null ? "" : $" for Id '{id}'";
            return Error.NotFound("record.not.found", $"record not found{forId}");
        }

        public static Error LengthIsInvalid(string? name = null)
        {
            var label = name == null ? "" : " " + name + " ";
            return Error.Validation("length.is.invalid", $"invalid{label}length");
        }
        
        public static Error AlreadyExist(string? name = null)
        {
            var label = name ?? "entity";
            return Error.Validation($"{label.ToLower()}.already.exist", $"{label} already exist");
        }
    }
    
    public static class User
    { 
        public static Error InvalidCredentials()
        {
            return Error.Validation("credentials.is.invalid", "Invalid credentials");
        }
    }
    public static class Minio
    {
        public static Error CouldNotDownloadFile()
        {
            return Error.Failure("could.not.download.file", "Could not download file");
        }

        public static Error CouldNotUploadFile()
        {
            return Error.Failure("could.not.upload.file", "Could not upload file");
        }

        public static Error CouldNotDeleteFile()
        {
            return Error.Failure("could.not.delete.file", "Could not delete file");
        }
    }
}