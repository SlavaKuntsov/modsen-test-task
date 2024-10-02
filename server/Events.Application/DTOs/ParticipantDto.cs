using System.Globalization;
using System.Runtime.Serialization;

using Events.Domain.Constants;

namespace Events.Application.DTOs;

public class ParticipantDto
{
	public Guid Id { get; set; }

	public string Email { get; set; } = string.Empty;

	public string Role { get; set; } = string.Empty;

	public string FirstName { get; set; } = string.Empty;

	public string LastName { get; set; } = string.Empty;

	private DateTime _dateOfBirth;

	public string DateOfBirth
	{
		get => _dateOfBirth.ToString(DateTimeConst.DATE_TIME_FORMAT);
		set
		{
			if (DateTime.TryParseExact(value,
				[DateTimeConst.DATE_TIME_FORMAT, "dd.MM.yyyy HH:mm:ss", "MM/dd/yyyy", "MM/dd/yyyy HH:mm:ss"],
				CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
			{
				_dateOfBirth = date;
			}
			else
			{
				throw new FormatException($"Invalid date format12: {value}");
			}
		}
	}

	public string AccessToken { get; set; } = string.Empty;
}
