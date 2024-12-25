using CSharpFunctionalExtensions;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Domain.Entity;

public record Users
{
    private Users(){}
    
    public long FirstMemberId { get; init; }
    public long SecondMemberId { get; init; }

    private Users(long firstMemberId, long secondMemberId)
    {
        FirstMemberId = firstMemberId;
        SecondMemberId = secondMemberId;
    }

    public static Result<Users, Error> Create(long firstMemberId, long secondMemberId)
    {
        if (firstMemberId <= 0 || secondMemberId <= 0)
            return Errors.General.ValueIsInvalid("UserId");
        
        return new Users(firstMemberId, secondMemberId);
    }
}