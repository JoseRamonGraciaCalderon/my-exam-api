using Application.Commands;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class CreateNodeCommandHandler : IRequestHandler<CreateNodeCommand, TreeNodeDto>
    {
        private readonly ITreeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateNodeCommandHandler> _logger;

        public CreateNodeCommandHandler(ITreeRepository repository, IMapper mapper, ILogger<CreateNodeCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TreeNodeDto> Handle(CreateNodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var node = _mapper.Map<TreeNode>(request.Node);
                GenerateIdsForNodeAndChildren(node);

                await _repository.AddNodeAsync(node);
                _logger.LogInformation("Node with ID {NodeId} created successfully", node.Id);

                return _mapper.Map<TreeNodeDto>(node);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating node with ID {NodeId}", request.Node.Id);
                throw;
            }
        }

        private void GenerateIdsForNodeAndChildren(TreeNode node)
        {
            node.Id = Guid.NewGuid().ToString();
            foreach (var child in node.Children)
            {
                GenerateIdsForNodeAndChildren(child);
            }
        }
    }
}
