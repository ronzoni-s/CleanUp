using CleanUp.Application.Features.Orders.Commands;
using CleanUp.Application.Features.Orders.Commands.AddEdit;
using CleanUp.Application.Features.Orders.Commands.Complete;
using CleanUp.Application.Features.Orders.Commands.Delete;
using CleanUp.Application.Features.Orders.Commands.Void;
using CleanUp.Application.Features.Orders.Queries.Export;
using CleanUp.Application.Features.Orders.Queries.GetAllPaged;
using CleanUp.Application.Features.Orders.Queries.GetById;
using CleanUp.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CleanUp.Server.Controllers.v1.Catalog
{
    public class OrdersController : BaseApiController<OrdersController>
    {
        /// <summary>
        /// Get All Orders
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Orders.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, bool hideCompleted, bool hideVoided, string orderBy = null)
        {
            var orders = await _mediator.Send(new GetAllOrdersQuery(pageNumber, pageSize, searchString, orderBy, hideCompleted, hideVoided));
            return Ok(orders);
        }

        /// <summary>
        /// Get a Order By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Orders.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery() { Id = id });
            return Ok(order);
        }

        /// <summary>
        /// Get a Order By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Orders.View)]
        [HttpGet("{id}/next-order")]
        public async Task<IActionResult> GetNextOrderId(int id)
        {
            var order = await _mediator.Send(new GetNextOrderIdQuery() { Id = id });
            return Ok(order);
        }



        /// <summary>
        /// Get a Order By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Orders.View)]
        [HttpGet("{id}/previous-order")]
        public async Task<IActionResult> GetPreviousOrderId(int id)
        {
            var order = await _mediator.Send(new GetPreviousOrderIdQuery() { Id = id });
            return Ok(order);
        }

        /// <summary>
        /// Complete a Order
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Orders.Create)]
        [HttpPost]
        [Route("print-polibox-label")]
        public async Task<IActionResult> PrintPoliboxLabel(PrintPoliboxLabelCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        /// <summary>
        /// Complete a Order
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Orders.Create)]
        [HttpPost]
        [Route("complete")]
        public async Task<IActionResult> Complete(CompleteOrderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Void a Order
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Orders.Create)]
        [HttpPost]
        [Route("void")]
        public async Task<IActionResult> Void(VoidOrderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Add a Order
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Orders.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddOrderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Orders.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteOrderCommand { Id = id }));
        }

        /// <summary>
        /// Search Orders and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Orders.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportOrdersQuery(searchString)));
        }

    }
}