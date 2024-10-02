namespace Events.Application.Exceptions;

public class UserExistsException(string email) : SystemException($"User with email '{email}' already exists.")
{
}
