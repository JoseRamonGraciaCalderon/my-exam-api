using MediatR;
using Application.DTOs;

namespace Application.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PdfFilePath { get; set; }
    }
}
