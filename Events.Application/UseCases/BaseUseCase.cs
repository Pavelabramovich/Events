using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Application.UseCases;


public abstract class BaseUseCase
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IMapper _mapper;


    public BaseUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
}


public abstract class ActionUseCase(IUnitOfWork unitOfWork, IMapper mapper) : BaseUseCase(unitOfWork, mapper)
{
    public abstract void Execute();
    public abstract Task ExecuteAsync(CancellationToken cancellationToken = default);
}

public abstract class ActionUseCase<TArg>(IUnitOfWork unitOfWork, IMapper mapper) : BaseUseCase(unitOfWork, mapper)
{
    public abstract void Execute(TArg arg);
    public abstract Task ExecuteAsync(TArg arg, CancellationToken cancellationToken = default);
}

public abstract class ActionUseCase<TArg1, TArg2>(IUnitOfWork unitOfWork, IMapper mapper) : BaseUseCase(unitOfWork, mapper)
{
    public abstract void Execute(TArg1 arg1, TArg2 arg2);
    public abstract Task ExecuteAsync(TArg1 arg1, TArg2 arg2, CancellationToken cancellationToken = default);
}

public abstract class ActionUseCase<TArg1, TArg2, TArg3>(IUnitOfWork unitOfWork, IMapper mapper) : BaseUseCase(unitOfWork, mapper)
{
    public abstract void Execute(TArg1 arg1, TArg2 arg2, TArg3 arg3);
    public abstract Task ExecuteAsync(TArg1 arg1, TArg2 arg2, TArg3 arg3, CancellationToken cancellationToken = default);
}


public abstract class FuncUseCase<TRes>(IUnitOfWork unitOfWork, IMapper mapper) : BaseUseCase(unitOfWork, mapper)
{
    public abstract TRes Execute();
    public abstract Task<TRes> ExecuteAsync(CancellationToken cancellationToken = default);
}

public abstract class FuncUseCase<TArg, TRes>(IUnitOfWork unitOfWork, IMapper mapper) : BaseUseCase(unitOfWork, mapper)
{
    public abstract TRes Execute(TArg arg);
    public abstract Task<TRes> ExecuteAsync(TArg arg, CancellationToken cancellationToken = default);
}

public abstract class FuncUseCase<TArg1, TArg2, TRes>(IUnitOfWork unitOfWork, IMapper mapper) : BaseUseCase(unitOfWork, mapper)
{
    public abstract TRes Execute(TArg1 arg1, TArg2 arg2);
    public abstract Task<TRes> ExecuteAsync(TArg1 arg1, TArg2 arg2, CancellationToken cancellationToken = default);
}

public abstract class FuncUseCase<TArg1, TArg2, TArg3, TRes>(IUnitOfWork unitOfWork, IMapper mapper) : BaseUseCase(unitOfWork, mapper)
{
    public abstract TRes Execute(TArg1 arg1, TArg2 arg2, TArg3 arg3);
    public abstract Task<TRes> ExecuteAsync(TArg1 arg1, TArg2 arg2, TArg3 arg3, CancellationToken cancellationToken = default);
}
