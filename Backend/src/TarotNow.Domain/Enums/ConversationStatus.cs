

namespace TarotNow.Domain.Enums;

public static class ConversationStatus
{
        public const string Pending = "pending";

        public const string AwaitingAcceptance = "awaiting_acceptance";

        public const string Ongoing = "ongoing";

        public const string Completed = "completed";

        public const string Cancelled = "cancelled";

        public const string Expired = "expired";

        public const string Disputed = "disputed";

    public static bool IsTerminal(string status)
        => status is Completed or Cancelled or Expired;

    public static bool IsValid(string status) =>
        status is Pending or AwaitingAcceptance or Ongoing or Completed or Cancelled or Expired or Disputed;
}
