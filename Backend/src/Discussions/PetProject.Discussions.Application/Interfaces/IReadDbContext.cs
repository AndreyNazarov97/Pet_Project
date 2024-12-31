using PetProject.Core.Dtos.Discussions;

namespace PetProject.Discussions.Application.Interfaces;

public interface IReadDbContext
{
    IQueryable<DiscussionDto> Discussions { get; }
    IQueryable<MessageDto> Messages { get; }
    IQueryable<MembersDto> Members { get; }
}