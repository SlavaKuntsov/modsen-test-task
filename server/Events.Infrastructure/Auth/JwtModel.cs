namespace Events.Infrastructure.Auth;

public class JwtModel
{
	public string SecretKey { get; set; } = string.Empty;

	public int ExpiresHours { get; set; }
}
