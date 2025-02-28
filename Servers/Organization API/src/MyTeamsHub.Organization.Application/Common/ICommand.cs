using MediatR;

namespace MyTeamsHub.Core.Application.Common;

public interface ICommand : IRequest<ServiceResult>
{

}

public interface ICommand<TResponse> : IRequest<ServiceDataResult<TResponse>>
{

}

public interface ICommandHandler<in TRequest> : IRequestHandler<TRequest, ServiceResult>
    where TRequest : ICommand
{

}

public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, ServiceDataResult<TResponse>>
    where TRequest : ICommand<TResponse>
{

}

public interface IQuery<TResponse> : IRequest<ServiceDataResult<TResponse>>
{

}

public interface IQueryHandler<in TRequest, TResponse> : IRequestHandler<TRequest, ServiceDataResult<TResponse>>
    where TRequest : IQuery<TResponse>
{

}
