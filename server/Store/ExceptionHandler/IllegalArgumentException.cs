namespace ExceptionHandler;

public class IllegalArgumentException: Exception
{
    public IllegalArgumentException(string message)
        : base(message)
    {
        
    }
}