

namespace TarotNow.Domain.Enums;

public static class QuestionItemStatus
{
        public const string Pending = "pending";

        public const string Accepted = "accepted";

        public const string Released = "released";

        public const string Refunded = "refunded";

        public const string Disputed = "disputed";

        public static bool IsValid(string s) =>
        s is Pending or Accepted or Released or Refunded or Disputed;
}
