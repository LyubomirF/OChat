using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OChat.Infrastructure.Exceptions;
using OChat.Services.Exceptions;
using OChatApp.Services;
using System;

namespace OChatApp.Controllers
{
    public class ErrorsController : ControllerBase
    {
        [Route("error")]
        public IActionResult GetError()
        {
            IExceptionHandlerFeature exceptionHandler = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            Exception exception = exceptionHandler?.Error;

            if (exception is NotFoundException)
                return NotFound(new ErrorResponse() { Status = 404, Description = exception.Message });
            if (exception is EmptyCollectionException)
                return BadRequest(new ErrorResponse() { Status = 400, Description = exception.Message });
            if (exception is FriendRequestException)
                return BadRequest(new ErrorResponse() { Status = 400, Description = exception.Message });

            return StatusCode(500);
        }
    }
}
