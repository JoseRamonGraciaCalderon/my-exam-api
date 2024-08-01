using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        protected async Task<IActionResult> HandleRequestAsync<T>(Func<Task<T>> func, ILogger logger, string actionDescription)
        {
            try
            {
                logger.LogInformation("Starting: {ActionDescription}", actionDescription);
                var result = await func();
                logger.LogInformation("Completed: {ActionDescription}", actionDescription);
                return Ok(new { Status = "Success", Message = "Operation completed successfully", Data = result });
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "Validation error in: {ActionDescription}", actionDescription);
                return BadRequest(new { Status = "Error", Message = ex.Message, Data = (object)null });
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogWarning(ex, "Resource not found in: {ActionDescription}", actionDescription);
                return NotFound(new { Status = "Error", Message = ex.Message, Data = (object)null });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Internal server error in: {ActionDescription}", actionDescription);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "An internal server error occurred", Data = (object)null });
            }
        }

        protected async Task<IActionResult> HandleRequestAsync(Func<Task> func, ILogger logger, string actionDescription)
        {
            try
            {
                logger.LogInformation("Starting: {ActionDescription}", actionDescription);
                await func();
                logger.LogInformation("Completed: {ActionDescription}", actionDescription);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "Validation error in: {ActionDescription}", actionDescription);
                return BadRequest(new { Status = "Error", Message = ex.Message, Data = (object)null });
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogWarning(ex, "Resource not found in: {ActionDescription}", actionDescription);
                return NotFound(new { Status = "Error", Message = ex.Message, Data = (object)null });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Internal server error in: {ActionDescription}", actionDescription);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "An internal server error occurred", Data = (object)null });
            }
        }
    }
}
