using Application.DTOs;
using MediatR;
using System;

namespace Application.Commands
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PdfFilePath { get; set; }
    }
}
