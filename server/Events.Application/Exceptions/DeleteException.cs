namespace Events.Application.Exceptions;

public class DeleteException(string message) : SystemException(message)
{
}