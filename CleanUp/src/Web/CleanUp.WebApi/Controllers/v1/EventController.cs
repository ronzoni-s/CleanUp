﻿using CleanUp.Application.WebApi.Events;
using CleanUp.Application.WebApi.Events.Queries;
using fbognini.WebFramework.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanUp.Application.WebApi.Events.Commands;
using CleanUp.Application.Common.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUp.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    [AllowAnonymous]
    public class EventController : ApiController
    {
        //[Authorize(Policy = Permissions.Event.View)]
        [HttpGet]
        [Route("{id}")]
        public async Task<ApiResult<EventDto>> GetById([FromRoute] int id)
        {
            return await Mediator.Send(new GetEventQuery(id));
        }

        //[Authorize(Policy = Permissions.Event.Manage)]
        [HttpPost]
        [Route("")]
        public async Task<ApiResult<EventDto>> Create([FromBody] CreateEventCommand command)
        {
            return await Mediator.Send(command);
        }

        //[Authorize(Policy = Permissions.Event.Manage)]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ApiResult<EventDto>> Delete([FromRoute] int id)
        {
            return await Mediator.Send(new DeleteEventCommand(id));
        }
    }
}