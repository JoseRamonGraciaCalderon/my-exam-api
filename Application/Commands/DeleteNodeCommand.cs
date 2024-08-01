using MediatR;

namespace Application.Commands
{
    public class DeleteNodeCommand : IRequest<Unit>
    {
        public string Id { get; set; }
    }
}
