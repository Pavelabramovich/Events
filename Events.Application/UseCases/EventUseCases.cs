using AutoMapper;
using Events.Application.Dto;
using Events.Domain;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public static class EventUseCases
{
    public class GetAll(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<IEnumerable<EventWithoutParticipantsDto>>(unitOfWork, mapper)
    {
        public override IEnumerable<EventWithoutParticipantsDto> Execute()
        {
            return _unitOfWork
                .EventRepository
                .GetAll()
                .Select(e => _mapper.Map<EventWithoutParticipantsDto>(e))
                .ToArray();
        }

        public override async Task<IEnumerable<EventWithoutParticipantsDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork
                .EventRepository
                .GetAllAsync(cancellationToken)
                .Select(e => _mapper.Map<EventWithoutParticipantsDto>(e))
                .ToArrayAsync(cancellationToken);
        }
    }

    public class GetById(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<int, EventWithoutParticipantsDto?>(unitOfWork, mapper)
    {
        private const string NotFoundErrorMessage = "Not found event with this id.";


        public override EventWithoutParticipantsDto? Execute(int id)
        {
            var @event = _unitOfWork.EventRepository.FindById(id)
                ?? throw new ValidationException(NotFoundErrorMessage);

            return _mapper.Map<EventWithoutParticipantsDto>(@event);
        }

        public override async Task<EventWithoutParticipantsDto?> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            var @event = await _unitOfWork.EventRepository.FindByIdAsync(id, cancellationToken)
                ?? throw new ValidationException(NotFoundErrorMessage);

            return _mapper.Map<EventWithoutParticipantsDto>(@event);
        }
    }

    public class GetByName(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<string, EventWithoutParticipantsDto?>(unitOfWork, mapper)
    {
        private const string NotFoundErrorMessage = "Not found event with this name.";


        public override EventWithoutParticipantsDto? Execute(string name)
        {
            var @event = _unitOfWork.EventRepository.FindByName(name)
                ?? throw new ValidationException(NotFoundErrorMessage);

            return _mapper.Map<EventWithoutParticipantsDto>(@event);
        }

        public override async Task<EventWithoutParticipantsDto?> ExecuteAsync(string name, CancellationToken cancellationToken = default)
        {
            var @event = await _unitOfWork.EventRepository.FindByNameAsync(name, cancellationToken)
                ?? throw new ValidationException(NotFoundErrorMessage);

            return _mapper.Map<EventWithoutParticipantsDto>(@event);
        }
    }

    public class GetAllWithParticipants(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<IEnumerable<EventWithParticipantsDto>>(unitOfWork, mapper)
    {
        public override IEnumerable<EventWithParticipantsDto> Execute()
        {
            return _unitOfWork
                .EventRepository
                .GetAllWithParticipations()
                .Select(e => _mapper.Map<EventWithParticipantsDto>(e))
                .ToArray();
        }

        public override async Task<IEnumerable<EventWithParticipantsDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork
                .EventRepository
                .GetAllWithParticipationsAsync(cancellationToken)
                .Select(e => _mapper.Map<EventWithParticipantsDto>(e))
                .ToArrayAsync(cancellationToken);
        }
    }

    public class GetParticipantsById(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<int, IEnumerable<ParticipantWithoutEventDto>>(unitOfWork, mapper)
    {
        public override IEnumerable<ParticipantWithoutEventDto> Execute(int id)
        {
            return _unitOfWork
                .EventRepository
                .GetEventParticipations(id)
                .Select(p => _mapper.Map<ParticipantWithoutEventDto>(p))
                .ToArray();
        }

        public override async Task<IEnumerable<ParticipantWithoutEventDto>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork
                .EventRepository
                .GetEventParticipationsAsync(id, cancellationToken)
                .Select(p => _mapper.Map<ParticipantWithoutEventDto>(p))
                .ToArrayAsync(cancellationToken);
        }
    }

    public class GetPage(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<int, int, IEnumerable<EventWithoutParticipantsDto>>(unitOfWork, mapper)
    {
        public override IEnumerable<EventWithoutParticipantsDto> Execute(int pageNum, int pageSize)
        {
            return _unitOfWork
               .EventRepository
               .PageAll(skip: pageNum * pageSize, take: pageSize)
               .Select(e => _mapper.Map<EventWithoutParticipantsDto>(e))
               .ToArray();
        }

        public override async Task<IEnumerable<EventWithoutParticipantsDto>> ExecuteAsync(int pageNum, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork
                .EventRepository
                .PageAllAsync(skip: pageNum * pageSize, take: pageSize, cancellationToken)
                .Select(e => _mapper.Map<EventWithoutParticipantsDto>(e))
                .ToArrayAsync(cancellationToken);
        }
    }


    public class Create(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<EventCreatingDto>(unitOfWork, mapper)
    {
        public override void Execute(EventCreatingDto eventDto)
        {
            var @event = _mapper.Map<Event>(eventDto);

            _unitOfWork.EventRepository.Add(@event);

            if (!_unitOfWork.SaveChanges())
                throw new ValidationException("Internal error");
        }

        public override async Task ExecuteAsync(EventCreatingDto eventDto, CancellationToken cancellationToken = default)
        {
            var @event = _mapper.Map<Event>(eventDto);

            _unitOfWork.EventRepository.Add(@event);

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException("Internal error");
        }
    }

    public class Update(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<EventWithoutParticipantsDto>(unitOfWork, mapper)
    {
        public override void Execute(EventWithoutParticipantsDto eventDto)
        {
            var @event = _unitOfWork.EventRepository.FindById(eventDto.Id)
                ?? throw new ValidationException("Event with this id not found");

            Event eventToReplace = _mapper.Map(eventDto, @event);

            _unitOfWork.EventRepository.Update(eventToReplace);

            if (!_unitOfWork.SaveChanges())
                throw new ValidationException("Internal error");
        }

        public override async Task ExecuteAsync(EventWithoutParticipantsDto eventDto, CancellationToken cancellationToken = default)
        {
            var @event = await _unitOfWork.EventRepository.FindByIdAsync(eventDto.Id, cancellationToken)
                ?? throw new ValidationException("Event with this id not found");

            Event eventToReplace = _mapper.Map(eventDto, @event);

            _unitOfWork.EventRepository.Update(eventToReplace);

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException("Internal error");
        }
    }

    public class UpdatePaticipation(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int, int>(unitOfWork, mapper)
    {
        public override void Execute(int eventId, int userId)
        {
            var @event = _unitOfWork.EventRepository.FindById(@eventId)
                ?? throw new ValidationException($"Could not find event with id = {eventId}");

            int participantsCount = _unitOfWork.EventRepository.ParticipantsCount(eventId);

            if (@event.MaxPeopleCount <= participantsCount)
                throw new ValidationException("All seats for the event are taken");

            _unitOfWork.EventRepository.AddParticipant(eventId, userId);

            if (!_unitOfWork.SaveChanges())
                throw new ValidationException("Internal error");
        }

        public override async Task ExecuteAsync(int eventId, int userId, CancellationToken cancellationToken = default)
        {
            var @event = await _unitOfWork.EventRepository.FindByIdAsync(@eventId, cancellationToken)
               ?? throw new ValidationException($"Could not find event with id = {eventId}");

            int participantsCount = await _unitOfWork.EventRepository.ParticipantsCountAsync(eventId, cancellationToken);

            if (@event.MaxPeopleCount <= participantsCount)
                throw new ValidationException("All seats for the event are taken");

            await _unitOfWork.EventRepository.AddParticipantAsync(eventId, userId, cancellationToken);

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException("Internal error");
        }
    }


    public class Remove(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int>(unitOfWork, mapper)
    {
        public override void Execute(int id)
        {
            var @event = _unitOfWork.EventRepository.FindById(id)
                ?? throw new ValidationException("Event with this id not found");

            _unitOfWork.EventRepository.Remove(@event.Id);

            if (!_unitOfWork.SaveChanges())
                throw new ValidationException("Internal error");
        }

        public override async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            var @event = await _unitOfWork.EventRepository.FindByIdAsync(id, cancellationToken)
                ?? throw new ValidationException("Event with this id not found");

            _unitOfWork.EventRepository.Remove(@event.Id);

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException("Internal error");
        }
    }

    public class RemoveParticipation(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int, int>(unitOfWork, mapper)
    {
        public override void Execute(int eventId, int userId)
        {
            _unitOfWork.EventRepository.RemoveParticipant(eventId, userId);

            if (!_unitOfWork.SaveChanges())
                throw new ValidationException("Internal error");
        }

        public override async Task ExecuteAsync(int eventId, int userId, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.EventRepository.RemoveParticipantAsync(eventId, userId, cancellationToken);

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException("Internal error");
        }
    }
}
