using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Escrow.Commands.AddQuestion;

public sealed class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, Guid>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public AddQuestionCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<Guid> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new AddQuestionCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (Guid)domainEvent.Result;
    }
}

public sealed class AddQuestionCommandHandlerRequestedDomainEvent : IIdempotentDomainEvent
{
    public AddQuestionCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public string EventIdempotencyKey => CommandEventIdempotencyKey.Build(
        nameof(AddQuestionCommandHandlerRequestedDomainEvent),
        Command.IdempotencyKey);

    public AddQuestionCommandHandlerRequestedDomainEvent(AddQuestionCommand command)
    {
        Command = command;
    }
}
