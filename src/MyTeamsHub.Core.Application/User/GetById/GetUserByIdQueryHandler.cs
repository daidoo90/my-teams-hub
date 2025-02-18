using MyTeamsHub.Core.Application.Common;
using MyTeamsHub.Core.Application.Interfaces.Repositories;
using MyTeamsHub.Core.Domain.Users;

namespace MyTeamsHub.Core.Application.User.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<Core.Domain.Users.User>;

public class GetUserByIdQueryHandler(IEfRepository<UserEntity> users) : IQueryHandler<GetUserByIdQuery, Core.Domain.Users.User>
{
    private readonly IEfRepository<UserEntity> _users = users;

    public async Task<ServiceDataResult<Core.Domain.Users.User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _users.FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);
        if (user == null)
            return ServiceDataResult<Core.Domain.Users.User>.WithError(ErrorCodes.InvalidUser);

        return ServiceDataResult<Core.Domain.Users.User>.WithData(new Core.Domain.Users.User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserId = request.UserId,
            PhoneNumber = user.PhoneNumber,
            Title = user.RoleTitle,
            Status = (int)user.UserStatus,
            UserType = (int)user.UserType
        });
    }
}
