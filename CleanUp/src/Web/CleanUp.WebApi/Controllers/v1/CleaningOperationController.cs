using CleanUp.Application.WebApi.CleaningOperations;
using CleanUp.Application.WebApi.CleaningOperations.Queries;
using fbognini.WebFramework.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUp.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    [AllowAnonymous]
    public class CleaningOperationController : ApiController
    {
        //[Authorize(Policy = Permissions.CleaningOperation.View)]
        [HttpGet]
        [Route("")]
        public async Task<ApiResult<List<CleaningOperationDto>>> GetAll([FromRoute] GetCleaningOperationsQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
