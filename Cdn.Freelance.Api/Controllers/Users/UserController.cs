﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Cdn.Freelance.Api.Controllers.Users.Commands;
using Cdn.Freelance.Api.Controllers.Users.Queries;

namespace Cdn.Freelance.Api.Controllers.Users
{
    /// <summary>
    /// User's endpoint.
    /// </summary>
    [Authorize("cdn:freelance")]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/users")]
    [Produces("application/json")]
    [Tags("Users Management")]
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
        [HttpPost]
        [SwaggerRequestExample(typeof(UserInput), typeof(UserInputExample))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(200, typeof(UserIdentifierExample))]
        public async Task<UserIdentifier> CreateAsync([FromBody, Required] UserInput userInput)
        {
            return await _mediator.Send(new AddUser.Command(userInput));
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="identifier">User identifier.</param>
        /// <param name="updateUserInput">Updated user information.</param>
        [HttpPut, Route("{identifier}")]
        [SwaggerRequestExample(typeof(UpdateUserInput), typeof(UpdateUserInputExample))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromRoute, Required, MaxLength(50)] string identifier, [FromBody, Required] UpdateUserInput updateUserInput)
        {
            await _mediator.Send(new UpdateUser.Command(identifier, updateUserInput));
            return NoContent();
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <param name="limit">Limit to fetch.</param>
        /// <param name="offset">Records to offset.</param>
        /// <returns>Paginated result of users.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseExample(200, typeof(UsersOutputExample))]
        public async Task<LimitOffsetPagingResultModel<UserOutput>> GetUsersAsync(
            [FromQuery] [Range(1, 1000)] int limit = 20,
            [FromQuery] [Range(0, int.MaxValue)] int offset = 0)
        {
            return await _mediator.Send(new GetAllUsers.Query(limit, offset));
        }

        /// <summary>
        /// Get user by identifier.
        /// </summary>
        /// <param name="identifier">User identifier.</param>
        /// <returns>User information.</returns>
        [HttpGet, Route("{identifier}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseExample(200, typeof(UserOutputExample))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<UserOutput> GetUserAsync([FromRoute, Required, MaxLength(50)] string identifier)
        {
            return await _mediator.Send(new GetUserByIdentifier.Query(identifier));
        }

        /// <summary>
        /// Delete user by identifier.
        /// </summary>
        /// <param name="identifier">User identifier.</param>
        [HttpDelete, Route("{identifier}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserAsync([FromRoute, Required, MaxLength(50)] string identifier)
        {
            await _mediator.Send(new DeleteUser.Command(identifier));
            return NoContent();
        }
    }
}
