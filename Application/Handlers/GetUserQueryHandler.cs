using Application.Queries;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Application.DTOs;

namespace Application.Handlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetUserQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IUserRepository userRepository, ILogger<GetUserQueryHandler> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching user with ID {UserId}.", request.Id);
                var user = await _userRepository.GetUserAsync(request.Id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", request.Id);
                    return null;
                }
                else
                {
                    _logger.LogInformation("User with ID {UserId} fetched.", request.Id);
                    return _mapper.Map<UserDto>(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user with ID {UserId}.", request.Id);
                throw;
            }
        }
    }
}
