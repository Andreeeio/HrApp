namespace HrApp.Domain.Exceptions;

public class FirstLoginException : Exception
{
    public FirstLoginException(string message) : base(message)
    {
    }
}
