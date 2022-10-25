using CleanUp.Application.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanUp.Application.WebApi.CleaningOperations.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUp.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    [Authorize]
    public class ReportController : ApiController
    {
        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Report.View)]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> DailyCleaningOperations([FromQuery] GetDailyCleaningOperationsReportQuery query)
        {
            var bytes = await Mediator.Send(query);
            return File(bytes, "application/vnd.ms-excel", "report.xlsx");
        }
    }
}
