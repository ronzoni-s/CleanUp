using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CleanUp.Application.Common.Authorization;
using CleanUp.Application.Common.Models;
using CleanUp.Application.WebApi.Users;
using CleanUp.Application.WebApi.Authentication.Commands;
using CleanUp.Application.WebApi.Users.Queries;
using fbognini.WebFramework.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanUp.Application.WebApi.Roles;
using CleanUp.Application.WebApi.Roles.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUp.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    [Authorize]
    public class UserController : ApiController
    {
        [Authorize(Policy = Permissions.User.View)]
        [HttpGet]
        [Route("{id}")]
        public async Task<ApiResult<UserDto>> GetById([FromRoute] string id)
        {
            return await Mediator.Send(new GetUserQuery(id));
        }

        /// <summary>
        /// Get User Roles By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.User.View)]
        [HttpGet("{id}/roles")]
        public async Task<ApiResult<List<RoleDto>>> GetRolesAsync(string id)
        {
            return await Mediator.Send(new GetRolesByUserIdQuery(id));
        }
    }
}
