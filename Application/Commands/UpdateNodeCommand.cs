using MediatR;
using Application.DTOs;

namespace Application.Commands
{
    public class UpdateNodeCommand : IRequest<Unit>
    {
        public TreeNodeDto Node { get; set; }
    }
}
