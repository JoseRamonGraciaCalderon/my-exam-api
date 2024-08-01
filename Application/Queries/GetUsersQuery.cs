using MediatR;
using Application.DTOs;

namespace Application.Queries
{
    public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
    {
    }
}
