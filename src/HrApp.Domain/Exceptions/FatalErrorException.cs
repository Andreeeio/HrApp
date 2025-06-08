namespace HrApp.Domain.Exceptions;

public class FatalErrorException : Exception
{
    public FatalErrorException(string message) : base(message)
    {
    }
    public FatalErrorException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
