using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models.Users;

using MapsterMapper;

using MediatR;

namespace Events.Application.Handlers.Events;

public class GetEventParticipantsQuery(Guid id) : IRequest<IList<ParticipantModel>>
{
	public Guid Id { get; private set; } = id;
}

public class GetEventParticipantsQueryHandler(IEventsParticipantsRepository eventsParticipantsRepository, IMapper mapper) : IRequestHandler<GetEventParticipantsQuery, IList<ParticipantModel>>
{
	private readonly IEventsParticipantsRepository _eventsParticipantsRepository = eventsParticipantsRepository;
	private readonly IMapper _mapper = mapper;

	public async Task<IList<ParticipantModel>> Handle(GetEventParticipantsQuery request, CancellationToken cancellationToken)
	{
		var userModels = await _eventsParticipantsRepository.GetParticipantsByEvent(request.Id);

		if (userModels == null || !userModels.Any())
			throw new NotFoundException($"Particants not found");

		return _mapper.Map<IList<ParticipantModel>>(userModels);
	}
}