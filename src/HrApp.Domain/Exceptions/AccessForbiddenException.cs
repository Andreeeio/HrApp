namespace HrApp.Domain.Exceptions;

public class AccessForbiddenException : Exception
{
    public AccessForbiddenException(string message) : base(message)
    {
    }
}
