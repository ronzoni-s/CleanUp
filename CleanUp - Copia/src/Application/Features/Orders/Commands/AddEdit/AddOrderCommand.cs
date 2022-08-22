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
using System.Collections.Generic;
using ErbertPranzi.Shared.Constants.Application;

namespace ErbertPranzi.Application.Features.Orders.Commands.AddEdit
{
    public class AddOrderProductCommand
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Weight { get; set; }
        public int Tax { get; set; }
        /// <summary>
        /// In cents
        /// </summary>
        public int Price { get; set; }
        public int? MenuId { get; set; }
    }

    public class AddOrderMenuCommand
    {
        public int Id { get; set; }
        /// <summary>
        /// In cents
        /// </summary>
        public int Price { get; set; }
    }


    public partial class AddOrderCommand : IRequest<Result<int>>
    {
        [Required]
        public int OrderNumber { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string CustomerAddress { get; set; }
        [Required]
        public string ContactName { get; set; }
        public string ContactPhoneNumber { get; set; }
        public DateTime OrderDate { get; set; }

        public IList<AddOrderMenuCommand> OrderMenus { get; set; }
        public IList<AddOrderProductCommand> OrderProducts { get; set; }
    }

    internal class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IParameterRepository parameterRepository;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddOrderCommandHandler> _localizer;

        public AddOrderCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddOrderCommandHandler> localizer, IParameterRepository parameterRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            this.parameterRepository = parameterRepository;
        }

        public async Task<Result<int>> Handle(AddOrderCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (await _unitOfWork.Repository<Order>().Entities.AnyAsync(p => p.OrderNumber == command.OrderNumber, cancellationToken))
                {
                    return await Result<int>.FailAsync(_localizer["OrderNumber already exists."], "OrderAlreadyExists");
                }

                CustomerAddress customerAddress = await GetCustomerAddress(command, cancellationToken);

                List<OrderProduct> orderProducts = GetOrderProducts(command);
                List<OrderMenu> orderMenus = GetOrderMenus(command);


                // Calculate final prices
                var orderProductsByMenu = orderProducts.Where(orderProduct => orderProduct.MenuId != null)
                                                    .GroupBy(orderProduct => orderProduct.MenuId.Value)
                                                    .ToList();
                double finalPricesSum;
                foreach (var ops in orderProductsByMenu)
                {
                    finalPricesSum = 0;
                    var totalFullPrice = ops.Sum(op => op.Price * op.Quantity);
                    var orderMenu = orderMenus.Where(om => om.MenuId == ops.Key).FirstOrDefault();
                    if (orderMenu == null)
                    {
                        continue;
                    }
                    var menuPrice = orderMenu.Price;

                    var opsOrdered = ops.OrderByDescending(field => field.Quantity).ToList();
                    for (int i = 0; i < opsOrdered.Count - 1; i++)
                    {
                        opsOrdered[i].FinalPrice = Math.Round(orderMenu.Price * opsOrdered[i].Price / totalFullPrice, 2);
                        finalPricesSum += Math.Round(opsOrdered[i].FinalPrice * opsOrdered[i].Quantity, 2);
                    }

                    opsOrdered[^1].FinalPrice = Math.Round((orderMenu.Price - finalPricesSum) / opsOrdered[^1].Quantity, 2);
                }

                var weightPerBag = await parameterRepository.GetWeightPerBag();

                var order = new Order
                {
                    OrderNumber = command.OrderNumber,
                    ContactName = command.ContactName,
                    ContactPhoneNumber = command.ContactPhoneNumber,
                    OrderDate = command.OrderDate,
                    CustomerAddress = customerAddress,
                    OrderMenus = orderMenus,
                    OrderProducts = orderProducts,
                    Bags = GetBags(orderProducts, weightPerBag),
                    WeightPerBag = weightPerBag,
                    BagsPerPolibox = await parameterRepository.GetBagsPerPolibox(),
                    Weight = orderProducts.Sum(x => x.ProductWeight * x.Quantity),
                    Price = orderProducts.Sum(x => x.Price * x.Quantity),
                    FinalPrice = orderMenus.Sum(x => x.Price)
                        + orderProducts.Where(x => x.MenuId is null).Sum(x => x.Price * x.Quantity)

                };
                await _unitOfWork.Repository<Order>().AddAsync(order);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(order.Id, _localizer["Order Saved"]);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task<CustomerAddress> GetCustomerAddress(AddOrderCommand command, CancellationToken cancellationToken)
        {
            CustomerAddress customerAddress = await _unitOfWork.Repository<CustomerAddress>().Entities
                                                        .Include(c => c.Customer)
                                                        .Where(c => c.Address == command.CustomerAddress && c.Customer.Name == command.CustomerName)
                                                        .FirstOrDefaultAsync(cancellationToken);
            if (customerAddress != null)
            {
                return customerAddress;
            }

            var customer = await _unitOfWork.Repository<Customer>().Entities.Where(c => c.Name == command.CustomerName).FirstOrDefaultAsync(cancellationToken);
            if (customer == null)
            {
                customer = new Customer
                {
                    Name = command.CustomerName,
                    CustomerAddresss = new List<CustomerAddress>
                            {
                                (customerAddress = new CustomerAddress { Address = command.CustomerAddress })
                            }
                };
                await _unitOfWork.Repository<Customer>().AddAsync(customer);
            }
            else
            {
                customerAddress = new CustomerAddress
                {
                    CustomerId = customer.Id,
                    Address = command.CustomerAddress
                };
                await _unitOfWork.Repository<CustomerAddress>().AddAsync(customerAddress);
            }

            return customerAddress;
        }

        private List<OrderMenu> GetOrderMenus(AddOrderCommand command)
        {
            var orderMenus = new List<OrderMenu>();
            if (command.OrderMenus == null)
            {
                return orderMenus;
            }

            foreach (var om in command.OrderMenus)
            {
                orderMenus.Add(new OrderMenu
                {
                    MenuId = om.Id,
                    Price = Math.Round((double)om.Price / 100.0, 2),
                });
            }

            return orderMenus;
        }

        private List<OrderProduct> GetOrderProducts(AddOrderCommand command)
        {
            var orderProducts = new List<OrderProduct>();
            if (command.OrderProducts == null)
            {
                return orderProducts;
            }

            foreach (var op in command.OrderProducts)
            {
                var product = _unitOfWork.Repository<Product>().Entities.Where(p => p.Code == op.ProductCode).FirstOrDefault();
                if (product == null)
                {
                    product = new Product
                    {
                        Code = op.ProductCode,
                        IsActive = true
                    };
                }
                product.Name = op.Name;
                product.Price = Math.Round((double)op.Price / 100, 2);
                product.Tax = op.Tax;
                product.Weight = op.Weight;

                orderProducts.Add(new OrderProduct
                {
                    Product = product,
                    Quantity = op.Quantity,
                    ProductWeight = op.Weight,
                    Tax = op.Tax,
                    Price = Math.Round((double)op.Price / 100.0, 2),
                    FinalPrice = Math.Round((double)op.Price / 100.0, 2),
                    MenuId = op.MenuId
                });
            }

            return orderProducts;
        }

        private static int GetBags(List<OrderProduct> orderProducts, double weightPerBag)
        {
            int bags = 0;
            double weightCumulative = weightPerBag;

            var orderProductWeights = new List<double>();
            foreach (var orderProduct in orderProducts)
            {
                for (int i = 0; i < orderProduct.Quantity; i++)
                {
                    orderProductWeights.Add(orderProduct.ProductWeight);
                }
            }

            foreach (var orderProductWeight in orderProductWeights)
            {
                weightCumulative += orderProductWeight;
                if (weightCumulative > weightPerBag)
                {
                    bags++;
                    weightCumulative = orderProductWeight;
                }
            }

            return bags;
        }
    }
}