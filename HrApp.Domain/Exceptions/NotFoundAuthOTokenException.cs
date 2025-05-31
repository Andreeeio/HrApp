namespace HrApp.Domain.Exceptions;

public class NotFoundAuthOTokenException : Exception
{
    public NotFoundAuthOTokenException(string message) : base(message)
    {
    }
}
