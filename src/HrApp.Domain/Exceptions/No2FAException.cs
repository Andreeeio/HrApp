namespace HrApp.Domain.Exceptions;

public class No2FAException : Exception
{
    public No2FAException(string message) : base(message)
    {
    }
}
