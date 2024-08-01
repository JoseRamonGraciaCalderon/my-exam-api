using Application.Commands;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> GetUsers()
        {
            return HandleRequestAsync(async () =>
            {
                var users = await _mediator.Send(new GetUsersQuery());
                return users;
            }, _logger, "Get all users");
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> GetUser(Guid id)
        {
            return HandleRequestAsync(async () =>
            {
                var user = await _mediator.Send(new GetUserQuery { Id = id });
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found");
                }
                return user;
            }, _logger, $"Get user by ID {id}");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CreateUser(CreateUserCommand command)
        {
            return HandleRequestAsync(async () =>
            {
                var userId = await _mediator.Send(command);
                return userId;
            }, _logger, $"Create user with email {command.Email}");
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> UpdateUser(Guid id, UpdateUserCommand command)
        {
            return HandleRequestAsync(async () =>
            {
                if (id != command.Id)
                {
                    throw new ArgumentException("User ID mismatch");
                }

                var user = await _mediator.Send(new GetUserQuery { Id = id });
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found");
                }

                await _mediator.Send(command);
            }, _logger, $"Update user by ID {id}");
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> DeleteUser(Guid id)
        {
            return HandleRequestAsync(async () =>
            {
                var user = await _mediator.Send(new GetUserQuery { Id = id });
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found");
                }

                await _mediator.Send(new DeleteUserCommand { Id = id });
            }, _logger, $"Delete user by ID {id}");
        }

        [HttpPost("{id:guid}/upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> UploadFile(Guid id, IFormFile file)
        {
            return HandleRequestAsync(async () =>
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("No file uploaded");
                }

                // Optional: Validate file extension if needed
                var allowedExtensions = new[] { ".pdf", ".jpg", ".png", ".docx", ".xlsx" }; // Add other allowed extensions as needed
                var extension = Path.GetExtension(file.FileName);
                if (!allowedExtensions.Contains(extension.ToLower()))
                {
                    throw new ArgumentException("Invalid file type");
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists
                var filePath = Path.Combine(uploadsFolder, $"{id}{extension}");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var user = await _mediator.Send(new GetUserQuery { Id = id });
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found");
                }

                // Get the base URL from the request to construct the file URL
                var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                var fileUrl = $"{baseUrl}/uploads/{id}{extension}";

                await _mediator.Send(new UpdateUserCommand
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PdfFilePath = fileUrl // Store the public URL
                });

                return new { file.FileName, file.Length };
            }, _logger, $"Upload file for user by ID {id}");
        }
    }
}
