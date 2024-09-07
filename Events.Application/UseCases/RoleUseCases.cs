using AutoMapper;
using Events.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Events.Application.UseCases;


public static class RoleUseCases
{
    public class GetAll(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<IEnumerable<string>>(unitOfWork, mapper)
    {
        public override IEnumerable<string> Execute()
        {
            var roles = _unitOfWork.RoleRepository.GetAll();
            return roles.Select(r => r.Name).ToArray();
        }

        public override async Task<IEnumerable<string>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var roles = await _unitOfWork.RoleRepository.GetAllAsync(cancellationToken).ToArrayAsync(cancellationToken);
            return roles.Select(r => r.Name).ToArray();
        }
    }

    public class Exists(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<string, bool>(unitOfWork, mapper)
    {
        public override bool Execute(string name)
        {
            var roles = _unitOfWork.RoleRepository.GetAll();
            return roles.Any(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public override async Task<bool> ExecuteAsync(string name, CancellationToken cancellationToken = default)
        {
            var roles = await _unitOfWork.RoleRepository.GetAllAsync(cancellationToken).ToArrayAsync(cancellationToken);
            return roles.Any(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }

    public class Create(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<string>(unitOfWork, mapper)
    {
        public override void Execute(string name)
        {
            _unitOfWork.RoleRepository.Add(new Role { Name = name });

            if (!_unitOfWork.SaveChanges())
                throw new ValidationException();
        }

        public override async Task ExecuteAsync(string name, CancellationToken cancellationToken = default)
        {
            _unitOfWork.RoleRepository.Add(new Role { Name = name });

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException();
        }
    }

    public class Update(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<string, string>(unitOfWork, mapper)
    {
        public override void Execute(string oldName, string newName)
        {
            throw new NotImplementedException();
        }

        public override async Task ExecuteAsync(string oldName, string newName, CancellationToken cancellationToken = default)
        {
            var roles = await _unitOfWork.RoleRepository.GetAllAsync(cancellationToken).ToArrayAsync(cancellationToken);
            var role = roles.FirstOrDefault(r => r.Name.Equals(oldName, StringComparison.OrdinalIgnoreCase));

            if (role is null)
                throw new ValidationException();

            _unitOfWork.RoleRepository.Remove(role.Name);
            _unitOfWork.RoleRepository.Add(new Role() { Name = newName });

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException();
        }
    }

    public class Remove(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<string>(unitOfWork, mapper)
    {
        public override void Execute(string name)
        {
            throw new NotImplementedException();
        }

        public override async Task ExecuteAsync(string name, CancellationToken cancellationToken = default)
        {
            var roles = await _unitOfWork.RoleRepository.GetAllAsync(cancellationToken).ToArrayAsync(cancellationToken);
            var role = roles.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (role is null)
                throw new ValidationException();

            _unitOfWork.RoleRepository.Remove(role.Name);

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException();
        }
    }
}
