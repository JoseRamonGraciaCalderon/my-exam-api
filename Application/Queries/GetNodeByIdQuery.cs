using Application.DTOs;
using MediatR;

namespace Application.Queries
{
    public class GetNodeByIdQuery : IRequest<TreeNodeDto>
    {
        public string Id { get; set; }
    }
}
