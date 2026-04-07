

namespace TarotNow.Domain.Enums;

public static class ChatMessageType
{
        public const string Text = "text";

        public const string System = "system";

        public const string CardShare = "card_share";

        public const string Image = "image";

        public const string Voice = "voice";

        public const string PaymentOffer = "payment_offer";

        public const string PaymentAccept = "payment_accept";

        public const string PaymentReject = "payment_reject";

        public const string SystemRefund = "system_refund";

        public const string SystemRelease = "system_release";

        public const string SystemDispute = "system_dispute";

        public const string CallLog = "call_log";

        public static bool IsValid(string type)
    {
        return type is Text or System or CardShare or Image or Voice
            or PaymentOffer or PaymentAccept or PaymentReject
            or SystemRefund or SystemRelease or SystemDispute
            or CallLog;
    }
}
