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
    public class UpdateNodeCommandHandler : IRequestHandler<UpdateNodeCommand, Unit>
    {
        private readonly ITreeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateNodeCommandHandler> _logger;

        public UpdateNodeCommandHandler(ITreeRepository repository, IMapper mapper, ILogger<UpdateNodeCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateNodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating node with ID {NodeId}", request.Node.Id);
                var node = _mapper.Map<TreeNode>(request.Node);
                await _repository.UpdateNodeAsync(node);
                _logger.LogInformation("Node with ID {NodeId} updated successfully", request.Node.Id);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating node with ID {NodeId}", request.Node.Id);
                throw;
            }
        }
    }
}
