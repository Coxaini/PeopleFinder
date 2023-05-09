
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PeopleFinder.Application.Common.Errors;
using System.Security.Claims;
using PeopleFinder.Domain.Common.Constants;

namespace PeopleFinder.Api.Controllers.Common
{
    [ApiController]
    [Authorize]
    public class ApiController : ControllerBase
    {

    protected int ProfileIdInClaims => Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    protected int UserIdInClaims => Convert.ToInt32(HttpContext.User.FindFirst(ProjectJwtRegisteredClaimNames.UserId)?.Value);
    

    protected IActionResult Problem(List<IError> errors)
        {
            if (errors.Count is 0)
            {
                return Problem();
            }

            if (errors.All(error => error is ValidationError))
            {
                return ValidationProblem(errors);
            }

            return Problem(errors[0]);
        }

        private IActionResult Problem(IError error)
        {
            var statusCode = error.Metadata["Type"] switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.AccessDenied => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(statusCode: statusCode, title: error.Message);
        }

        private IActionResult ValidationProblem(List<IError> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(
                    error.Metadata["Code"].ToString() ?? "Validation",
                    error.Message);
            }

            return ValidationProblem(modelStateDictionary);
        }
    }
}
