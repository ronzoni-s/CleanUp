﻿using CleanUp.Application.Models;
using CleanUp.Domain.Entities.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanUp.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<double> GetOrderWeight(int orderId);

        Task<bool> IsOrderCompleted(int orderId);

        Task<double?> GetCompletedOrderWeightPerBag(int orderId);

        Task<List<OrderPolibox>> GetOrderPoliboxs(int orderId);

        int GetUsedPoliboxs(int orderId, out int? previousPoliboxRemainingBags, out int? previousOrderBagsPerPolibox);

        int GetUsedPlacesInPreviousPolibox(int orderId, out int poliboxUsed);
    }
}