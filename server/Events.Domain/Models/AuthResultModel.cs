namespace Events.Domain.Models;

public  class AuthResultModel
{
	public string AccessToken { get; set; } = string.Empty;
	public string RefreshToken { get; set; } = string.Empty;
}
