using MediatR;
using Application.DTOs;

namespace Application.Queries
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public Guid Id { get; set; }
    }
}
