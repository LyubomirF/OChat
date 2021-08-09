using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OChatApp.Models;
using OChatApp.Services;
using OChatApp.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            return StatusCode(500);
        }
    }
}
