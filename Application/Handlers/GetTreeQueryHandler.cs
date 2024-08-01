using Application.DTOs;
using Application.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetTreeQueryHandler : IRequestHandler<GetTreeQuery, List<TreeNodeDto>>
    {
        private readonly ITreeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTreeQueryHandler> _logger;

        public GetTreeQueryHandler(ITreeRepository repository, IMapper mapper, ILogger<GetTreeQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TreeNodeDto>> Handle(GetTreeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching the entire tree");
                var tree = await _repository.GetTreeAsync();

                if (tree == null || tree.Count == 0)
                {
                    _logger.LogWarning("No nodes found in the tree");
                    return new List<TreeNodeDto>();
                }

                _logger.LogInformation("Tree fetched successfully");
                return _mapper.Map<List<TreeNodeDto>>(tree);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching the tree");
                throw;
            }
        }
    }
}
