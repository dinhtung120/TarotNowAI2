using System.Reflection;
using TarotNow.Infrastructure.BackgroundJobs.Outbox;
using TarotNow.Infrastructure.Persistence.Outbox;

namespace TarotNow.Infrastructure.UnitTests.BackgroundJobs;

public sealed class OutboxBatchProcessorPartitioningTests
{
    private static readonly MethodInfo ResolvePartitionMethod = typeof(OutboxBatchProcessor)
        .GetMethod("ResolveProcessingPartitionKey", BindingFlags.NonPublic | BindingFlags.Static)
        ?? throw new InvalidOperationException("Cannot find ResolveProcessingPartitionKey method.");

    [Fact]
    public void ResolveProcessingPartitionKey_ShouldExtractCamelCaseUserId()
    {
        var message = new OutboxMessage
        {
            EventType = "event.user.updated",
            PayloadJson = """{"userId":"11111111-1111-1111-1111-111111111111"}"""
        };

        var key = InvokeResolvePartition(message);

        Assert.Equal("UserId:11111111-1111-1111-1111-111111111111", key);
    }

    [Fact]
    public void ResolveProcessingPartitionKey_ShouldExtractPascalCaseUserId()
    {
        var message = new OutboxMessage
        {
            EventType = "event.user.updated",
            PayloadJson = """{"UserId":"22222222-2222-2222-2222-222222222222"}"""
        };

        var key = InvokeResolvePartition(message);

        Assert.Equal("UserId:22222222-2222-2222-2222-222222222222", key);
    }

    [Fact]
    public void ResolveProcessingPartitionKey_ShouldFallbackToEventType_WhenPayloadMalformed()
    {
        var message = new OutboxMessage
        {
            EventType = "event.user.updated",
            PayloadJson = """{"userId":"""
        };

        var key = InvokeResolvePartition(message);

        Assert.Equal("event:event.user.updated", key);
    }

    private static string InvokeResolvePartition(OutboxMessage message)
    {
        return (string)(ResolvePartitionMethod.Invoke(null, [message])
            ?? throw new InvalidOperationException("Partition key invocation returned null."));
    }
}
