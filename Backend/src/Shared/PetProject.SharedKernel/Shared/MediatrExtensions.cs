using MediatR;
using PetProject.SharedKernel.Interfaces;
using PetProject.SharedKernel.Shared.Common;

namespace PetProject.SharedKernel.Shared;

public static class MediatrExtensions
{
    public static async Task PublishDomainEvents<TId>(
        this IPublisher publisher,
        AggregateRoot<TId> entity,
        CancellationToken cancellationToken = default) where TId : notnull
    {
        foreach (var domainEvent in entity.DomainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }
        
        entity.ClearDomainEvents();
    }

    public static async Task PublishDomainEvent(
        this IPublisher publisher,
        IDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await publisher.Publish(domainEvent, cancellationToken);
    }
}