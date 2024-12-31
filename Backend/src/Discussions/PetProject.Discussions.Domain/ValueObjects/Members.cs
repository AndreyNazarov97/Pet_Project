using CSharpFunctionalExtensions;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Domain.ValueObjects;

public record Members
{
    public long FirstMemberId { get; init; }
    public long SecondMemberId { get; init; }


    private Members(
        long firstMemberId,
        long secondMemberId)
    {
        FirstMemberId = firstMemberId;
        SecondMemberId = secondMemberId;
    }

    public static Result<Members, Error> Create(long firstMemberId, long secondMemberId)
    {
        if (firstMemberId <= 0 || secondMemberId <= 0)
            return Errors.General.ValueIsInvalid("UserId");


        return new Members(firstMemberId, secondMemberId);
    }
}