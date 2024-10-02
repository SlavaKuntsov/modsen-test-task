namespace Events.Application.Exceptions;

public class InvalidTokenException(string message) : SystemException(message)
{
}