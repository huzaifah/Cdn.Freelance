using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;
using Cdn.Freelance.Api.Controllers.Users.Commands;

namespace Cdn.Freelance.Api.Controllers.Users
{
    /// <summary>
    /// User's endpoint.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [Tags("User Management")]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// User's endpoint.
        /// </summary>
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="userInput">New user information.</param>
        /// <returns>Newly created user identifier.</returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(200, typeof(UserIdentifierExample))]
        public async Task<UserIdentifier> CreateAsync([FromBody, Required] UserInput userInput)
        {
            return await _mediator.Send(new AddUser.Command(userInput));
        }
    }
}
