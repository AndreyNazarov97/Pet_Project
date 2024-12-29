using CSharpFunctionalExtensions;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Domain.Entity;

public class Members : SharedKernel.Shared.Common.Entity<MemberId>
{
    private Members(MemberId id) : base(id){}
    
    public long FirstMemberId { get; init; }
    public long SecondMemberId { get; init; }
    

    private Members(
        MemberId id,
        long firstMemberId, 
        long secondMemberId) : base(id)
    {
        FirstMemberId = firstMemberId;
        SecondMemberId = secondMemberId;
    }

    public static Result<Members, Error> Create(long firstMemberId, long secondMemberId)
    {
        if (firstMemberId <= 0 || secondMemberId <= 0)
            return Errors.General.ValueIsInvalid("UserId");
        
        var id = MemberId.NewId();
        
        return new Members(id, firstMemberId, secondMemberId);
    }
}