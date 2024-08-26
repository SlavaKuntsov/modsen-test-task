using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Models;

public class ParticipantModel
{
	public Guid Id { get; set; }

	public string FirstName { get; set; } = string.Empty;

	public string LastName { get; set; } = string.Empty;

	public DateTime DateOfBirth { get; set; }

	public DateTime? EventRegistrationDate { get; set; } = null;

	public string Email { get; set; } = string.Empty;

	public ICollection<EventModel> Events { get; set; } = [];
}
