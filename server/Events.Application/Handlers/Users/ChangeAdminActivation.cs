using Events.Domain.Interfaces.Repositories;

using MediatR;
using Events.Domain.Models.Users;
using Events.Application.Exceptions;

namespace Events.Application.Handlers.Users;

public class ChangeAdminActivationCommand(Guid id, bool isActive) : IRequest
{
	public Guid Id { get; private set; } = id;
	public bool IsActive { get; private set; } = isActive;
}

public class ChangeAdminActivationCommandHandler(IUsersRepository usersRepository) : IRequestHandler<ChangeAdminActivationCommand>
{
	private readonly IUsersRepository _usersRepository = usersRepository;

	public async Task Handle(ChangeAdminActivationCommand request, CancellationToken cancellationToken)
	{
		var existUser = await _usersRepository.Get<AdminModel>(request.Id, cancellationToken);

		if (existUser == null)
			throw new UserExistsException("User with this id doesn't exists");

		await _usersRepository.ChangeAdminActivation(request.Id, request.IsActive, cancellationToken);
	}
}