

namespace TarotNow.Application.Exceptions;


public class BadRequestException : Exception
{
    public BadRequestException()
        : this("Bad request.")
    {
    }

    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
