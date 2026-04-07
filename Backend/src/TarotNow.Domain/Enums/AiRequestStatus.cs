

namespace TarotNow.Domain.Enums;

public static class AiRequestStatus
{
    
    public const string Requested = "requested";
    
    public const string Completed = "completed";
    
    
    public const string FailedBeforeFirstToken = "failed_before_first_token";
    
    public const string FailedAfterFirstToken = "failed_after_first_token";
}
