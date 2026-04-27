using TarotNow.Domain.Events;

namespace TarotNow.Domain.UnitTests.Events;

public sealed class DepositWebhookReceivedDomainEventTests
{
    [Fact]
    public void EventIdempotencyKey_ShouldBeStableForSameProviderIdentity()
    {
        var payloadA =
            """
            {
              "code":"00",
              "success":true,
              "data":{
                "orderCode":920002,
                "amount":50000,
                "paymentLinkId":"plink_920002",
                "reference":"REF-IDEMP-001",
                "transactionDateTime":"2026-04-27 10:00:00",
                "code":"00",
                "desc":"success-v1"
              }
            }
            """;
        var payloadB =
            """
            {
              "success": true,
              "code":"00",
              "data":{
                "desc":"success-v2",
                "code":"00",
                "transactionDateTime":"2026-04-27 10:00:00",
                "reference":"REF-IDEMP-001",
                "paymentLinkId":"plink_920002",
                "amount":50000,
                "orderCode":920002
              }
            }
            """;

        var eventA = new DepositWebhookReceivedDomainEvent { RawPayload = payloadA };
        var eventB = new DepositWebhookReceivedDomainEvent { RawPayload = payloadB };

        Assert.Equal(eventA.EventIdempotencyKey, eventB.EventIdempotencyKey);
        Assert.StartsWith("deposit:webhook:payos:", eventA.EventIdempotencyKey, StringComparison.Ordinal);
    }

    [Fact]
    public void EventIdempotencyKey_ShouldChangeWhenProviderIdentityChanges()
    {
        var payloadA =
            """
            {
              "success": true,
              "data": {
                "orderCode": 920002,
                "amount": 50000,
                "paymentLinkId": "plink_920002",
                "reference": "REF-IDEMP-001",
                "transactionDateTime": "2026-04-27 10:00:00",
                "code": "00"
              }
            }
            """;
        var payloadB =
            """
            {
              "success": true,
              "data": {
                "orderCode": 920002,
                "amount": 50000,
                "paymentLinkId": "plink_920002",
                "reference": "REF-IDEMP-999",
                "transactionDateTime": "2026-04-27 10:00:00",
                "code": "00"
              }
            }
            """;

        var eventA = new DepositWebhookReceivedDomainEvent { RawPayload = payloadA };
        var eventB = new DepositWebhookReceivedDomainEvent { RawPayload = payloadB };

        Assert.NotEqual(eventA.EventIdempotencyKey, eventB.EventIdempotencyKey);
    }
}
