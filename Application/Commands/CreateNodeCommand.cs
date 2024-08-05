using MediatR;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;
namespace Application.Commands
{
    public class CreateNodeCommand : IRequest<TreeNodeDto>
    {
        [Required(ErrorMessage = "El nodo es requerido")]
        public TreeNodeDto Node { get; set; }
    }
}
