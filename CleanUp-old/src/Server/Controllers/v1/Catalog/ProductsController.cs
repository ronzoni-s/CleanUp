using CleanUp.Application.Features.Products.Commands.AddEdit;
using CleanUp.Application.Features.Products.Commands.Delete;
using CleanUp.Application.Features.Products.Commands.Update;
using CleanUp.Application.Features.Products.Queries.Export;
using CleanUp.Application.Features.Products.Queries.GetAllPaged;
using CleanUp.Application.Features.Products.Queries.GetOrderProductsPaged;
using CleanUp.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanUp.Server.Controllers.v1.Catalog
{
    public class ProductsController : BaseApiController<ProductsController>
    {
        /// <summary>
        /// Get All Products
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="hideNotActive"></param>
        /// <param name="orderId"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, bool hideNotActive, string orderBy = null, int? orderId = null)
        {
            var products = await _mediator.Send(new GetAllProductsQuery(pageNumber, pageSize, searchString, orderBy, hideNotActive, orderId));
            return Ok(products);
        }

        /// <summary>
        /// Get Order Products
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderId"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.View)]
        [HttpGet]
        [Route("order-products")]
        public async Task<IActionResult> GetOrderProducts(int pageNumber, int pageSize, string searchString, string orderBy = null, int? orderId = null)
        {
            var products = await _mediator.Send(new GetOrderProductsQuery(pageNumber, pageSize, searchString, orderBy, orderId));
            return Ok(products);
        }

        /// <summary>
        /// Add/Edit a Product
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditProductCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Update Products
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.Create)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(UpdateProductCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Products.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteProductCommand { Id = id }));
        }

        /// <summary>
        /// Search Products and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportProductsQuery(searchString)));
        }
    }
}