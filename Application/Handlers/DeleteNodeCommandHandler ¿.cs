using Application.Commands;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class DeleteNodeCommandHandler : IRequestHandler<DeleteNodeCommand, Unit>
    {
        private readonly ITreeRepository _repository;
        private readonly ILogger<DeleteNodeCommandHandler> _logger;

        public DeleteNodeCommandHandler(ITreeRepository repository, ILogger<DeleteNodeCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteNodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting node with ID {NodeId}", request.Id);
                await _repository.DeleteNodeAsync(request.Id);
                _logger.LogInformation("Node with ID {NodeId} deleted successfully", request.Id);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting node with ID {NodeId}", request.Id);
                throw;
            }
        }
    }
}
