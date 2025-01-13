using CSharpFunctionalExtensions;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.Common;
using PetProject.SharedKernel.Shared.EntityIds;

namespace VolunteerRequests.Domain.Aggregate;

public class UserRestriction : AggregateRoot<UserRestrictionId>
{
    private UserRestriction(UserRestrictionId id) : base(id)
    {
    }

    private UserRestriction(UserRestrictionId id, long userId, DateTime bannedUntil) : base(id)
    {
        UserId = userId;
        BannedUntil = bannedUntil;
    }

    public long UserId { get; private set; }

    public DateTime BannedUntil { get; private set; }

    public static Result<UserRestriction, Error> Create(
        long userId,
        int banDays = 1)
    {
        if (userId <= 0)
            return Errors.General.ValueIsInvalid(nameof(userId));
        if (banDays <= 0)
            return Errors.General.ValueIsInvalid("ban days");
        
        var id = UserRestrictionId.NewId();

        var bannedUntil = DateTime.UtcNow.AddDays(banDays);

        var userRestriction = new UserRestriction(id, userId, bannedUntil);

        return userRestriction;
    }

    public UnitResult<Error> CheckExpirationOfBan()
    {
        if (BannedUntil > DateTime.UtcNow)
            return Error.Failure("account.banned", "Account banned");

        return Result.Success<Error>();
    }

    public UnitResult<Error> ExtendBan(int banDays = 1)
    {
        if (banDays <= 0)
            return Errors.General.ValueIsInvalid("ban days");

        BannedUntil = BannedUntil.AddDays(banDays);

        return Result.Success<Error>();
    }
    
    public UnitResult<Error> RemoveBan()
    {
        BannedUntil = DateTime.UtcNow;
        
        return Result.Success<Error>();
    }
}