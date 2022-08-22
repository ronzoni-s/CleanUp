using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ErbertPranzi.Application.Interfaces.Repositories;
using ErbertPranzi.Application.Interfaces.Services;
using ErbertPranzi.Application.Requests;
using ErbertPranzi.Domain.Entities.Catalog;
using ErbertPranzi.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Collections.Generic;

namespace ErbertPranzi.Application.Features.Products.Commands.Update
{
    public partial class UpdateProductCommand : IRequest<Result<int>>
    {
        [Required]
        public string URL { get; set; }
        [Required]
        public UploadRequest UploadRequest { get; set; }
    }

    internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<UpdateProductCommandHandler> _localizer;
        private readonly IExcelService excelService;

        public UpdateProductCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<UpdateProductCommandHandler> localizer, IExcelService excelService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            this.excelService = excelService;
        }

        public async Task<Result<int>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var uploadRequest = command.UploadRequest;
            if (uploadRequest == null)
            {
                return await  Result<int>.FailAsync(_localizer["Document Not Found!"]);
            }
            uploadRequest.FileName = $"D-{Guid.NewGuid()}{uploadRequest.Extension}";
            
            var result = await excelService.ImportAsync<Product>(uploadRequest.Data, new Dictionary<int, string> {
                { 0, nameof(Product.Code)},
                { 1, nameof(Product.Name)},
                { 2, nameof(Product.Weight)},
                { 3, nameof(Product.Tax)},
                { 4, nameof(Product.Price)},
            }, true);

            await _unitOfWork.Repository<Product>().Entities.ForEachAsync(p => p.IsActive = false);
            foreach (var product in result)
            {
                var found = _unitOfWork.Repository<Product>().Entities.Where(p => p.Code == product.Code).FirstOrDefault();
                if (found == null)
                {
                    product.IsActive = true;
                    await _unitOfWork.Repository<Product>().AddAsync(product);
                }
                else
                {
                    found.Name = product.Name;
                    found.Weight = product.Weight;
                    found.Price = product.Price;
                    found.Tax = product.Tax;
                    found.IsActive = true;
                    await _unitOfWork.Repository<Product>().UpdateAsync(found);
                }
            }

            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(_localizer["Products Updated"]);
        }
    }
}