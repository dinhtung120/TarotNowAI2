using System.ComponentModel;

namespace TarotNow.Domain.Enums;

public enum CallSessionStatus
{
        [Description("requested")]
    Requested = 0,

        [Description("accepted")]
    Accepted = 1,

        [Description("rejected")]
    Rejected = 2,

        [Description("ended")]
    Ended = 3
}
