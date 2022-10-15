using CleanUp.Application.WebApi.Classrooms;
using fbognini.WebFramework.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanUp.Application.Authorization;
using CleanUp.Application.WebApi.Classrooms.Queries;

namespace CleanUp.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    [AllowAnonymous]
    public class ClassroomController : ApiController
    {
        //[Authorize(Policy = Permissions.Classroom.View)]
        [HttpGet]
        [Route("")]
        public async Task<ApiResult<List<ClassroomDto>>> GetAll([FromRoute] GetClassroomsQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
