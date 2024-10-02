﻿using Events.Application.DTOs;
using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models.Users;

using MapsterMapper;

using MediatR;

namespace Events.Application.Handlers.Users;

public class UpdateParticipantCommand(Guid id, string firstName, string lastName, DateTime dateOfBirth) : IRequest<ParticipantDto>
{
	public Guid Id { get; private set; } = id;
	public string FirstName { get; private set; } = firstName;
	public string LastName { get; private set; } = lastName;
	public DateTime DateOfBirth { get; private set; } = dateOfBirth;
}

public class UpdateParticipantCommandHandler(IUsersRepository usersRepository, IMapper mapper) : IRequestHandler<UpdateParticipantCommand, ParticipantDto>
{
	private readonly IUsersRepository _usersRepository = usersRepository;
	private readonly IMapper _mapper = mapper;

	public async Task<ParticipantDto> Handle(UpdateParticipantCommand request, CancellationToken cancellationToken)
	{
		var existParticipant = await _usersRepository.Get<ParticipantModel>(request.Id);

		if (existParticipant == null)
			throw new UserExistsException("User with this id doesn't exists");

		ParticipantModel particantModel = new(request.Id,
										existParticipant.Email,
										existParticipant.Password,
										existParticipant.Role,
										request.FirstName,
										request.LastName,
										request.DateOfBirth);

		return _mapper.Map<ParticipantDto>(await _usersRepository.Update(particantModel));
	}
}