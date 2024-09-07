using AutoMapper;
using Events.Application.Dto;
using Events.Domain;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public static class UserUseCases
{
    public class GetAll(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<IEnumerable<UserWithoutParticipantsDto>>(unitOfWork, mapper)
    {
        public override IEnumerable<UserWithoutParticipantsDto> Execute()
        {
            return _unitOfWork
                .UserRepository
                .GetAll()
                .Select(u => _mapper.Map<UserWithoutParticipantsDto>(u))
                .ToArray();
        }

        public override async Task<IEnumerable<UserWithoutParticipantsDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork
                .UserRepository
                .GetAllAsync()
                .Select(u => _mapper.Map<UserWithoutParticipantsDto>(u))
                .ToArrayAsync(cancellationToken);
        }
    }

    public class GetAllWithParticipants(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<IEnumerable<UserWithParticipantsDto>>(unitOfWork, mapper)
    {
        public override IEnumerable<UserWithParticipantsDto> Execute()
        {
            return _unitOfWork
               .UserRepository
               .GetAllWithParticipations()
               .Select(u => _mapper.Map<UserWithParticipantsDto>(u))
               .ToArray();
        }

        public override async Task<IEnumerable<UserWithParticipantsDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork
                .UserRepository
                .GetAllWithParticipationsAsync()
                .Select(u => _mapper.Map<UserWithParticipantsDto>(u))
                .ToArrayAsync();
        }
    }

    public class GetById(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<int, UserWithoutParticipantsDto?>(unitOfWork, mapper)
    {
        public override UserWithoutParticipantsDto? Execute(int id)
        {
            var user = _unitOfWork.UserRepository.FindById(id);
            return _mapper.Map<UserWithoutParticipantsDto>(user);
        }

        public override async Task<UserWithoutParticipantsDto?> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.UserRepository.FindByIdAsync(id);
            return _mapper.Map<UserWithoutParticipantsDto>(user);
        }
    }

    public class GetByLogin(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<string, UserWithoutParticipantsDto?>(unitOfWork, mapper)
    {
        public override UserWithoutParticipantsDto? Execute(string login)
        {
            var user = _unitOfWork.UserRepository.FindByLogin(login);
            return _mapper.Map<UserWithoutParticipantsDto>(user);
        }

        public override async Task<UserWithoutParticipantsDto?> ExecuteAsync(string login, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.UserRepository.FindByLoginAsync(login);
            return _mapper.Map<UserWithoutParticipantsDto>(user);
        }
    }

    public class GetParticipantsById(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<int, IEnumerable<ParticipantWithoutUserDto>>(unitOfWork, mapper)
    {
        public override IEnumerable<ParticipantWithoutUserDto> Execute(int id)
        {
            return _unitOfWork
                .UserRepository
                .GetUserEvents(id)
                .Select(p => _mapper.Map<ParticipantWithoutUserDto>(p))
                .ToArray();
        }

        public override async Task<IEnumerable<ParticipantWithoutUserDto>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            var participants = await _unitOfWork.UserRepository.GetUserEventsAsync(id, cancellationToken).ToArrayAsync(cancellationToken);

            return participants.Select(p => _mapper.Map<ParticipantWithoutUserDto>(p));
        }
    }

    public class GetPage(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<int, int, IEnumerable<UserWithoutParticipantsDto>>(unitOfWork, mapper)
    {
        public override IEnumerable<UserWithoutParticipantsDto> Execute(int pageNum, int pageSize)
        {
            return _unitOfWork
                .UserRepository
                .PageAll(skip: pageNum * pageSize, take: pageSize)
                .Select(u => _mapper.Map<UserWithoutParticipantsDto>(u))
                .ToArray();
        }

        public override async Task<IEnumerable<UserWithoutParticipantsDto>> ExecuteAsync(int pageNum, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork
                .UserRepository
                .PageAllAsync(skip: pageNum * pageSize, take: pageSize, cancellationToken)
                .Select(u => _mapper.Map<UserWithoutParticipantsDto>(u))
                .ToArrayAsync(cancellationToken);
        }
    }


    public class Create(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<UserCreatingDto>(unitOfWork, mapper)
    {
        public override void Execute(UserCreatingDto userDto)
        {
            User user = _mapper.Map<User>(userDto);

            _unitOfWork.UserRepository.Add(user);

            if (!_unitOfWork.SaveChanges())
                throw new ValidationException("Internal error");
        }

        public override async Task ExecuteAsync(UserCreatingDto userDto, CancellationToken cancellationToken = default)
        {
            User user = _mapper.Map<User>(userDto);

            _unitOfWork.UserRepository.Add(user);

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException("Internal error");
        }
    }

    public class Update(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<UserWithoutParticipantsDto>(unitOfWork, mapper)
    {
        public override void Execute(UserWithoutParticipantsDto userDto)
        {
            throw new NotImplementedException();
        }

        public override async Task ExecuteAsync(UserWithoutParticipantsDto userDto, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.UserRepository.FindByIdAsync(userDto.Id, cancellationToken)
                ?? throw new ValidationException("User with this id not found");

            var userToReplace = _mapper.Map<UserWithoutParticipantsDto, User>(userDto, user);

            _unitOfWork.UserRepository.Update(userToReplace);

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException("Internal error");
        }
    }

    public class Remove(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int>(unitOfWork, mapper)
    {
        public override void Execute(int id)
        {
            _unitOfWork.UserRepository.Remove(id);

            if (!_unitOfWork.SaveChanges())
                throw new ValidationException("Internal error");
        }

        public override async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            _unitOfWork.UserRepository.Remove(id);

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException("Internal error");
        }
    }
}
