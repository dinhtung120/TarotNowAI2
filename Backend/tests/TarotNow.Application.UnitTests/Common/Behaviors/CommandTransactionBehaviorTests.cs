using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using TarotNow.Application.Behaviors;
using TarotNow.Application.Interfaces;
using Xunit;

namespace TarotNow.Application.UnitTests.Common.Behaviors
{
    public sealed class CommandTransactionBehaviorTests
    {
        [Fact]
        public async Task Handle_ChatCommand_FlushesOutboxAfterCommit()
        {
            var coordinator = new Mock<ITransactionCoordinator>();
            coordinator
                .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
                .Returns<Func<CancellationToken, Task>, CancellationToken>((action, token) => action(token));

            var outbox = new Mock<IOutboxBatchProcessor>();
            outbox.Setup(x => x.ProcessOnceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var behavior = new CommandTransactionBehavior<TarotNow.Application.Features.Chat.Commands.TestDoubles.ChatPingCommand, bool>(
                coordinator.Object,
                outbox.Object,
                Mock.Of<ILogger<CommandTransactionBehavior<TarotNow.Application.Features.Chat.Commands.TestDoubles.ChatPingCommand, bool>>>());

            var result = await behavior.Handle(
                new TarotNow.Application.Features.Chat.Commands.TestDoubles.ChatPingCommand(),
                _ => Task.FromResult(true),
                CancellationToken.None);

            Assert.True(result);
            outbox.Verify(x => x.ProcessOnceAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_RespondConversationAddMoneyCommand_FlushesOutboxTwice()
        {
            var coordinator = new Mock<ITransactionCoordinator>();
            coordinator
                .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
                .Returns<Func<CancellationToken, Task>, CancellationToken>((action, token) => action(token));

            var outbox = new Mock<IOutboxBatchProcessor>();
            outbox.SetupSequence(x => x.ProcessOnceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1)
                .ReturnsAsync(0);

            var behavior = new CommandTransactionBehavior<TarotNow.Application.Features.Chat.Commands.TestDoubles.RespondConversationAddMoneyCommand, bool>(
                coordinator.Object,
                outbox.Object,
                Mock.Of<ILogger<CommandTransactionBehavior<TarotNow.Application.Features.Chat.Commands.TestDoubles.RespondConversationAddMoneyCommand, bool>>>());

            var result = await behavior.Handle(
                new TarotNow.Application.Features.Chat.Commands.TestDoubles.RespondConversationAddMoneyCommand(),
                _ => Task.FromResult(true),
                CancellationToken.None);

            Assert.True(result);
            outbox.Verify(x => x.ProcessOnceAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_NonChatCommand_DoesNotFlushOutbox()
        {
            var coordinator = new Mock<ITransactionCoordinator>();
            coordinator
                .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
                .Returns<Func<CancellationToken, Task>, CancellationToken>((action, token) => action(token));

            var outbox = new Mock<IOutboxBatchProcessor>();
            var behavior = new CommandTransactionBehavior<NonChatCommand, bool>(
                coordinator.Object,
                outbox.Object,
                Mock.Of<ILogger<CommandTransactionBehavior<NonChatCommand, bool>>>());

            var result = await behavior.Handle(
                new NonChatCommand(),
                _ => Task.FromResult(true),
                CancellationToken.None);

            Assert.True(result);
            outbox.Verify(x => x.ProcessOnceAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_NonTransactionalCommand_SkipsTransactionAndOutboxFlush()
        {
            var coordinator = new Mock<ITransactionCoordinator>();
            var outbox = new Mock<IOutboxBatchProcessor>();
            var behavior = new CommandTransactionBehavior<NonTransactionalChatCommand, bool>(
                coordinator.Object,
                outbox.Object,
                Mock.Of<ILogger<CommandTransactionBehavior<NonTransactionalChatCommand, bool>>>());

            var result = await behavior.Handle(
                new NonTransactionalChatCommand(),
                _ => Task.FromResult(true),
                CancellationToken.None);

            Assert.True(result);
            coordinator.Verify(
                x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()),
                Times.Never);
            outbox.Verify(x => x.ProcessOnceAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        public sealed class NonChatCommand : IRequest<bool>
        {
        }

        public sealed class NonTransactionalChatCommand : IRequest<bool>, INonTransactionalCommand
        {
        }
    }
}

namespace TarotNow.Application.Features.Chat.Commands.TestDoubles
{
    public sealed class ChatPingCommand : IRequest<bool>
    {
    }

    public sealed class RespondConversationAddMoneyCommand : IRequest<bool>
    {
    }
}
