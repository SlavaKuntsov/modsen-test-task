namespace Events.Application.Exceptions;

public class NotFoundException : SystemException
{
	public NotFoundException() {}

    public NotFoundException(string message) : base(message) { }
}
