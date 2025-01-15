using CSharpFunctionalExtensions;

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

        public static Error NotFound(long id)
        {
            return Error.NotFound("record.not.found", $"record not found for Id {id}");
        }
        
        public static Error Null(string? name = null)
        {
            var label = name ?? "value";
            return Error.Null("Null.entity", $"{label} is null");
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

    public static class Tokens
    {
        public static Error TokenExpired()
        {
            return Error.Failure("token.expired", "Token expired");
        }

        public static Error InvalidToken()
        {
            return Error.Failure("token.invalid", "Invalid token");
        }
    }


    public static class User
    {
        public static Error InvalidCredentials()
        {
            return Error.Validation("credentials.is.invalid", "Invalid credentials");
        }

        public static Error NotFound(long userId)
        {
            return Error.NotFound("user.not.found", $"user with id {userId} not found");
        }
    }
    
    public static class VolunteerRequests
    {
        public static Error InvalidStatus()
        {
            return Error.Failure("invalid.request.status", "Invalid request status");
        }
        
        public static Error AccessDenied()
        {
            return Error.Failure("access.denied", "Access denied");
        }
    }
    
    public static class Discussion
    {
        public static Error UserNotInDiscussion(long userId)
        {
            return Error.Failure("user.not.in.discussion", $"User with id {userId} not in discussion");
        }
        
        public static Error NotYourMessage()
        {
            return Error.Failure("not.your.message", "Not your message");
        }

        public static UnitResult<Error> DiscussionAlreadyClosed()
        {
            return Error.Failure("discussion.already.closed", "Discussion already closed");
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