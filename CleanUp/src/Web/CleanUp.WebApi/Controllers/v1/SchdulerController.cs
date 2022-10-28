using CleanUp.Application.Authorization;
using CleanUp.Application.WebApi.CleaningOperations;
using CleanUp.Application.WebApi.CleaningOperations.Commands;
using CleanUp.Application.WebApi.CleaningOperations.Queries;
using CleanUp.Application.WebApi.WorkDays;
using CleanUp.Application.WebApi.WorkDays.Commands;
using fbognini.WebFramework.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUp.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    public class SchedulerController : ApiController
    {
        [Authorize(Policy = Permissions.Scheduler.View)]
        [HttpGet]
        [Route("")]
        public async Task<ApiResult<List<CleaningOperationDto>>> GetAll([FromQuery] GetCleaningOperationsQuery query)
        {
            return await Mediator.Send(query);
        }

        [Authorize(Policy = Permissions.Scheduler.View)]
        [HttpPost]
        [Route("schedule")]
        public async Task<ApiResult> Schedule([FromQuery] ScheduleCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Create a work day
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.User.Manage)]
        [HttpPost]
        [Route("work-day")]
        public async Task<ApiResult<WorkDayDto>> CreateWorkDays([FromBody] CreateWorkDayCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Update a work day
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.User.Manage)]
        [HttpPut]
        [Route("work-day/{id}")]
        public async Task<ApiResult<WorkDayDto>> UpdateWorkDays([FromRoute] int id, [FromBody] UpdateWorkDayCommand command)
        {
            command.SetId(id);
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Update a work day
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.User.View)]
        [HttpGet]
        [Route("work-day/{id}")]
        public async Task<ApiResult<List<WorkDayDto>>> GetWorkDays([FromRoute] string id, [FromQuery] GetWorkDaysQuery query)
        {
            query.SetId(id);
            return await Mediator.Send(query);
        }
    }
}
