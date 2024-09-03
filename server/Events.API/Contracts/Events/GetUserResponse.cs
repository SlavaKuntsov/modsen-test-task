﻿using System.Globalization;

public class GetUserResponse
{
	public Guid Id { get; set; }

	public string Email { get; set; }

	//public string Role { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	private DateTime _dateOfBirth;

	public string DateOfBirth
	{
		get => _dateOfBirth.ToString("dd-MM-yyyy");
		set
		{
			if (DateTime.TryParseExact(value, new[] { "dd-MM-yyyy", "dd.MM.yyyy HH:mm:ss" },
				CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
			{
				_dateOfBirth = date;
			}
			else
			{
				throw new FormatException($"Invalid date format1: {value}");
			}
		}
	}

	//private DateTime _eventRegistrationDate;

	//public string EventRegistrationDate
	//{
	//	get => _eventRegistrationDate.ToString("dd-MM-yyyy HH:mm");
	//	set
	//	{
	//		if (DateTime.TryParseExact(value, new[] { "dd-MM-yyyy HH:mm", "dd.MM.yyyy HH:mm:ss" },
	//			CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
	//		{
	//			_eventRegistrationDate = date;
	//		}
	//		else
	//		{
	//			throw new FormatException($"Invalid date format2: {value}");
	//		}
	//	}
	//}
}
