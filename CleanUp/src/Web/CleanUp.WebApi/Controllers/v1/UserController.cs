﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CleanUp.Application.Authorization;
using CleanUp.Application.Models;
using CleanUp.Application.WebApi.Users;
using CleanUp.Application.WebApi.Authentication.Commands;
using CleanUp.Application.WebApi.Users.Queries;
using CleanUp.Application.WebApi.Users.Commands;
using fbognini.WebFramework.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanUp.Application.WebApi.Roles;
using CleanUp.Application.WebApi.Roles.Queries;
using CleanUp.Application.WebApi.WorkDays.Commands;
using CleanUp.Application.WebApi.WorkDays;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUp.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    [Authorize]
    public class UserController : ApiController
    {
        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.User.View)]
        [HttpGet]
        [Route("{id}")]
        public async Task<ApiResult<UserDto>> GetById([FromRoute] string id)
        {
            return await Mediator.Send(new GetUserQuery(id));
        }

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.User.View)]
        [HttpGet]
        [Route("")]
        public async Task<ApiResult<List<UserDto>>> All()
        {
            return await Mediator.Send(new GetUsersQuery());
        }

        /// <summary>
        /// Delete User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.User.Manage)]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ApiResult<UserDto>> DeleteByIdAsync([FromRoute] string id)
        {
            return await Mediator.Send(new DeleteUserCommand(id));
        }

        /// <summary>
        /// Update User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.User.Manage)]
        [HttpPut]
        [Route("{id}")]
        public async Task<ApiResult<UserDto>> UpdateAsync([FromRoute] string id, [FromBody] UpdateUserCommand command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Update User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.User.Manage)]
        [HttpPost]
        [Route("")]
        public async Task<ApiResult<UserDto>> Register([FromBody] CreateUserCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
