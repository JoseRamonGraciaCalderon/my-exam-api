using MediatR;
using Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using AutoMapper;
using Application.DTOs;

namespace Application.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository userRepository, ILogger<CreateUserCommandHandler> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating a new user with email {Email}", request.Email);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Email = request.Email,
                    PdfFilePath = request.PdfFilePath
                };

                await _userRepository.AddUserAsync(user);

                _logger.LogInformation("User created with ID {UserId}", user.Id);

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user with email {Email}", request.Email);
                throw;
            }
        }
    }
}
