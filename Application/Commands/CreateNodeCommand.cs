using MediatR;
using Application.DTOs;

namespace Application.Commands
{
    public class CreateNodeCommand : IRequest<TreeNodeDto>
    {
        public TreeNodeDto Node { get; set; }
    }
}
