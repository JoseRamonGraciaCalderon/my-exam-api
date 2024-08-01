using Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Application.Queries
{
    public class GetTreeQuery : IRequest<List<TreeNodeDto>>
    {
    }
}
