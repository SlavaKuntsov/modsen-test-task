namespace Events.Domain.Interfaces.Auth;

public interface IPasswordHash
{
	public string Generate(string password);
	public bool Verify(string password, string hashedPassword);
}
