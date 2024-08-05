using Application.DTOs;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        [Required(ErrorMessage = "El ID es requerido")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 30 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El correo electrónico debe ser válido")]
        [StringLength(50, ErrorMessage = "El correo electrónico debe tener menos de 50 caracteres")]
        public string Email { get; set; }

        public string? PdfFilePath { get; set; }
    }
}
