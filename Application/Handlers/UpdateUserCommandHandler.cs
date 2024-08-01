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
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateUserCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUserRepository userRepository, ILogger<UpdateUserCommandHandler> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating user with ID {UserId}.", request.Id);

                var user = new User
                {
                    Id = request.Id,
                    Name = request.Name,
                    Email = request.Email,
                    PdfFilePath = request.PdfFilePath
                };

                await _userRepository.UpdateUserAsync(user);

                _logger.LogInformation("User with ID {UserId} updated successfully.", request.Id);

                var updatedUser = await _userRepository.GetUserAsync(request.Id);
                return _mapper.Map<UserDto>(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {UserId}.", request.Id);
                throw;
            }
        }
    }
}
