namespace TarotNow.Application.Exceptions;

public sealed class BusinessRuleException : Exception
{
    private const string DefaultErrorCode = "business_rule_violation";
    private const string DefaultMessage = "A business rule has been violated.";

    public string ErrorCode { get; }

    public BusinessRuleException()
        : this(DefaultErrorCode, DefaultMessage)
    {
    }

    public BusinessRuleException(string message)
        : this(DefaultErrorCode, message)
    {
    }

    public BusinessRuleException(string message, Exception innerException)
        : this(DefaultErrorCode, message, innerException)
    {
    }

    public BusinessRuleException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public BusinessRuleException(string errorCode, string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}
