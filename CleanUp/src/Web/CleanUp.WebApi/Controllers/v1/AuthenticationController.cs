using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CleanUp.Application.Common.Models;
using CleanUp.Application.WebApi.Authentication.Commands;
using fbognini.WebFramework.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUp.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    [AllowAnonymous]
    public class AuthenticationController : ApiController
    {
        [HttpPost]
        [Route("login")]
        public async Task<ApiResult<LoginResponse>> Login([FromBody] LoginCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<ApiResult<LoginResponse>> Refresh([FromBody] RefreshLoginCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
