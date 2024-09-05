using System.ComponentModel;
using System.Reflection;

namespace Events.Domain.Enums;

public enum Role
{
	[Description("Admin")]
	Admin = 1,
	[Description("User")]
	User = 2
}

public static class EnumExtensions
{
	public static string GetDescription(this Enum value)
	{
		FieldInfo field = value.GetType().GetField(value.ToString());
		DescriptionAttribute attribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute));
		return attribute == null ? value.ToString() : attribute.Description;
	}
}