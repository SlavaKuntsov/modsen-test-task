using Events.Domain.Enums;

namespace Events.Domain.Models.Users;

public abstract class UserModel
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public Role Role { get; set; }

    public UserModel()
    {

    }

    public UserModel(Guid id, string email, string password, Role role)
    {
        Id = id;
        Email = email;
        Password = password;
        Role = role;
    }

    // TODO - может здесь сделать Create(принимает все поля для ParticipantModel и т.д) который по условия будет создавать или ParticipantModel или AdminModel
    // TODO - или в каждой моделе свои Create
}
