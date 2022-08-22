using ErbertPranzi.Application.Interfaces.Repositories;
using ErbertPranzi.Application.Interfaces.Services;
using ErbertPranzi.Domain.Entities.Catalog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErbertPranzi.Application.Extensions;
using ErbertPranzi.Application.Specifications.Catalog;
using ErbertPranzi.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ErbertPranzi.Application.Features.Orders.Queries.Export
{
    public class ExportOrdersQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportOrdersQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportOrdersQueryHandler : IRequestHandler<ExportOrdersQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportOrdersQueryHandler> _localizer;

        public ExportOrdersQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportOrdersQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportOrdersQuery request, CancellationToken cancellationToken)
        {
            var orderFilterSpec = new OrderFilterSpecification(request.SearchString);
            var orders = await _unitOfWork.Repository<Order>().Entities
                .Specify(orderFilterSpec)
                .ToListAsync( cancellationToken);
            var data = await _excelService.ExportAsync(orders, mappers: new Dictionary<string, Func<Order, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["OrderNumber"], item => item.OrderNumber },
                { _localizer["ContactName"], item => item.ContactName },
                { _localizer["ContactPhoneNumber"], item => item.ContactPhoneNumber },
                { _localizer["Bags"], item => item.Bags },
                { _localizer["CompletionDateTime"], item => item.CompletionDateTime },
                { _localizer["WeightPerBag"], item => item.WeightPerBag },
                { _localizer["BagsPerPolibox"], item => item.BagsPerPolibox },
            }, sheetName: _localizer["Orders"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}