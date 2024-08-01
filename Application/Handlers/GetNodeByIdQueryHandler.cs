using Application.DTOs;
using Application.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetNodeByIdQueryHandler : IRequestHandler<GetNodeByIdQuery, TreeNodeDto>
    {
        private readonly ITreeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetNodeByIdQueryHandler> _logger;

        public GetNodeByIdQueryHandler(ITreeRepository repository, IMapper mapper, ILogger<GetNodeByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TreeNodeDto> Handle(GetNodeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching node with ID {NodeId}", request.Id);
                var node = await _repository.GetNodeByIdAsync(request.Id);

                if (node == null)
                {
                    _logger.LogWarning("Node with ID {NodeId} not found", request.Id);
                    return null;
                }

                _logger.LogInformation("Node with ID {NodeId} fetched successfully", request.Id);
                return _mapper.Map<TreeNodeDto>(node);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching node with ID {NodeId}", request.Id);
                throw;
            }
        }
    }
}
