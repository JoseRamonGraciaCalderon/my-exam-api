using MediatR;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;
namespace Application.Commands
{
    public class UpdateNodeCommand : IRequest<Unit>
    {
        [Required(ErrorMessage = "El nodo es requerido")]
        public TreeNodeDto Node { get; set; }
    }
}
