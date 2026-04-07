

namespace TarotNow.Application.Exceptions;


public class NotFoundException : Exception
{
    public NotFoundException()
        : this("Resource was not found.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
