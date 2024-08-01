using Application.Queries;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Application.DTOs;

namespace Application.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetUsersQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, ILogger<GetUsersQueryHandler> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching all users.");
                var users = await _userRepository.GetUsersAsync();
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
                _logger.LogInformation("{UserCount} users fetched.", userDtos.Count());
                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users.");
                throw;
            }
        }
    }
}
