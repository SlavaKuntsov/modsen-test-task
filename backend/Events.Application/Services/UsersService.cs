using CSharpFunctionalExtensions;

using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;

using MapsterMapper;

namespace Events.Application.Services;

public class UsersService : IUsersServices
{
	private readonly IUsersRepository _usersRepository;
	private readonly IMapper _mapper;

	public UsersService(IUsersRepository usersRepository, IMapper mapper)
    {
		_usersRepository = usersRepository;
		_mapper = mapper;
	}

	public async Task<Result<ParticipantModel>> Get(string email)
	{
		var participantEntitiy = await _usersRepository.Get(email);

		if (participantEntitiy == null)
			return Result.Failure<ParticipantModel>("No user with this email");

		var eventsModels = _mapper.Map<ParticipantModel>(participantEntitiy);

		return eventsModels;
	}

	public async Task<Result<ParticipantModel>> Get(string email, string password)
	{
		var participantEntitiy = await _usersRepository.Get(email, password);

		if (participantEntitiy == null)
			return Result.Failure<ParticipantModel>("User with this email already exists");

		var eventsModels = _mapper.Map<ParticipantModel>(participantEntitiy);

		return eventsModels;
	}

	public async Task<Result<Guid>> Create(ParticipantModel user)
	{
		var existUser = await _usersRepository.Get(user.Email);

		if (existUser == null)
			return Result.Failure<Guid>("User with this email already exists");

		return await _usersRepository.Create(user);
	}
}
