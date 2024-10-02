namespace Events.Application.Exceptions;

public class RegistrationExistsException(string message) : SystemException(message)
{
}
