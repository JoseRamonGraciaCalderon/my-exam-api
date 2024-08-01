using Application.Commands;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Authentication;
using Domain.Entities;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
    [Route("api/[controller]")]
    public class TreeController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TreeController> _logger;

        public TreeController(IMediator mediator, ILogger<TreeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> GetTree()
        {
            return HandleRequestAsync(async () =>
            {
                var tree = await _mediator.Send(new GetTreeQuery());
                return tree;
            }, _logger, "Get tree structure");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> GetNodeById(string id)
        {
            return HandleRequestAsync(async () =>
            {
                var node = await _mediator.Send(new GetNodeByIdQuery { Id = id });
                if (node == null)
                {
                    throw new KeyNotFoundException("Node not found");
                }
                return node;
            }, _logger, $"Get node by ID {id}");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CreateNode([FromBody] CreateNodeCommand command)
        {
            return HandleRequestAsync(async () =>
            {
                var createdNode = await _mediator.Send(command);
                return createdNode;
            }, _logger, $"Create node with ID {command.Node.Id}");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> UpdateNode(string id, [FromBody] UpdateNodeCommand command)
        {
            return HandleRequestAsync(async () =>
            {
                if (id != command.Node.Id)
                {
                    throw new ArgumentException("Node ID mismatch");
                }

                var existingNode = await _mediator.Send(new GetNodeByIdQuery { Id = id });
                if (existingNode == null)
                {
                    throw new KeyNotFoundException("Node not found");
                }

                await _mediator.Send(command);
                return NoContent();
            }, _logger, $"Update node with ID {id}");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> DeleteNode(string id)
        {
            return HandleRequestAsync(async () =>
            {
                var existingNode = await _mediator.Send(new GetNodeByIdQuery { Id = id });
                if (existingNode == null)
                {
                    throw new KeyNotFoundException("Node not found");
                }

                await _mediator.Send(new DeleteNodeCommand { Id = id });
                return NoContent();
            }, _logger, $"Delete node with ID {id}");
        }
    }
}
