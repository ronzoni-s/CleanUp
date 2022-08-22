using ErbertPranzi.Application.Interfaces.Repositories;
using ErbertPranzi.Application.Models;
using ErbertPranzi.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErbertPranzi.Infrastructure.Repositories
{
    

    public class OrderRepository : IOrderRepository
    {
        private readonly IRepositoryAsync<Order, int> orderRepository;

        public OrderRepository(IRepositoryAsync<Order, int> orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        /// <summary>
        /// Returns the order weight. If the order is completed, returns the sum of the product at the time.
        /// If the order is not completed, returns the sum of actual product weights.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<double> GetOrderWeight(int orderId)
        {
            var order = await orderRepository.Entities
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found");

            return order.OrderProducts.Sum(op => op.Quantity * op.ProductWeight);
        }

        public async Task<bool> IsOrderCompleted(int orderId)
        {
            return await orderRepository.Entities
                .Where(o => o.Id == orderId)
                .AnyAsync(o => o.CompletionDateTime != null);
        }

        /// <summary>
        /// Returns the maximum weight per bag only for completed orders. 
        /// The value returned is not the actual one, but is the value at the time the order has been completed
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>Weight per bag when the order has been completed. If the order is not complete, returns null</returns>
        public async Task<double?> GetCompletedOrderWeightPerBag(int orderId)
        {
            double? result = null;
            if (await IsOrderCompleted(orderId))
            {
                result = orderRepository.Entities.Where(o => o.Id == orderId).Select(o => o.WeightPerBag).FirstOrDefault();
            }
            return result;
        }

        public async Task<List<OrderPolibox>> GetOrderPoliboxs(int orderId)
        {
            var order = await orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new Exception("Order not found");

            var completionDateTime = order.CompletionDateTime ?? DateTime.Now;

            var previousOrders = orderRepository.Entities
                                    .Where(o => o.CustomerAddressId == order.CustomerAddressId
                                            && o.OrderDate.Date == order.OrderDate.Date
                                            //&& (!o.CancellationDateTime.HasValue
                                            //    //|| o.CancellationDateTime.Value > completionDateTime
                                            //    )
                                            && o.CompletionDateTime.HasValue
                                            && o.CompletionDateTime < completionDateTime
                                    )
                                    .OrderBy(o => o.CompletionDateTime)
                                    .ToList();

            int previousPoliboxs = 0;
            int previousPoliboxRemainingBags = 0;
            int previousOrderBagsPerPolibox = 0;
            foreach (var previousOrder in previousOrders)
            {
                var usedBags = previousOrder.UsedBags ?? previousOrder.Bags;

                //var previousOrderPoliboxs = (double)(usedBags - previousPoliboxRemainingBags) / previousOrder.BagsPerPolibox;
                //if (previousOrderPoliboxs < 0)
                //    previousOrderPoliboxs = 0;

                //if (previousOrderPoliboxs > 1)
                //{
                //    previousOrderBagsPerPolibox = previousOrder.BagsPerPolibox;
                //    previousPoliboxs += (int)Math.Ceiling(previousOrderPoliboxs);

                //}

                //if (previousPoliboxRemainingBags == 0)
                //{
                //    previousOrderBagsPerPolibox = previousOrder.BagsPerPolibox;
                //    previousPoliboxs += (int)Math.Ceiling((double)usedBags / previousOrder.BagsPerPolibox);
                //    previousPoliboxRemainingBags = usedBags % previousOrder.BagsPerPolibox;
                //}
                //else 
                if(usedBags > previousPoliboxRemainingBags)
                {
                    previousOrderBagsPerPolibox = previousOrder.BagsPerPolibox;
                    previousPoliboxs += (int)Math.Ceiling((double)(usedBags - previousPoliboxRemainingBags) / previousOrder.BagsPerPolibox);
                    previousPoliboxRemainingBags = previousOrder.BagsPerPolibox - ((usedBags - previousPoliboxRemainingBags) % previousOrder.BagsPerPolibox);
                }
                else
                {
                    previousPoliboxRemainingBags = previousPoliboxRemainingBags - usedBags;
                }

                    //previousPoliboxRemainingBags = previousOrder.BagsPerPolibox - (((previousPoliboxRemainingBags == 0 ? previousOrder.BagsPerPolibox : previousPoliboxRemainingBags) - usedBags) % previousOrder.BagsPerPolibox);
            }

            var bags = order.UsedBags ?? order.Bags;

            int bagNumber = 1;
            var poliboxs = new List<OrderPolibox>();
            if (previousPoliboxs > 0)
            {
                var previousPolibox = new OrderPolibox(previousPoliboxs);

                var notRemainingBags = previousOrderBagsPerPolibox - previousPoliboxRemainingBags;

                for (int i = 0; i < notRemainingBags; i++)
                {
                    previousPolibox.Bags.Add(new OrderBag(bagNumber++, BagStatus.Full));
                }

                for (int i = notRemainingBags; i < previousOrderBagsPerPolibox; i++)
                {
                    previousPolibox.Bags.Add(new OrderBag(bagNumber++, bags-- > 0 ? BagStatus.Order : BagStatus.Empty));
                }

                poliboxs.Add(previousPolibox);
            }

            int poliboxIndex = 1;
            while (bags > 0)
            {
                var polibox = new OrderPolibox(poliboxIndex++ + previousPoliboxs);
                for (int i = 0; i < order.BagsPerPolibox; i++)
                {
                    polibox.Bags.Add(new OrderBag(bagNumber++, bags-- > 0 ? BagStatus.Order : BagStatus.Empty));
                }

                poliboxs.Add(polibox);
            }

            return poliboxs;
        }


        public int GetUsedPoliboxs(int orderId, out int? previousPoliboxRemainingBags, out int? previousOrderBagsPerPolibox)
        {
            var order = orderRepository.GetByIdAsync(orderId).GetAwaiter().GetResult();
            if (order == null)
                throw new Exception("Order not found");

            var previousOrders = orderRepository.Entities
                                    .Where(o => o.CustomerAddressId == order.CustomerAddressId
                                            && o.OrderDate.Date == order.OrderDate.Date
                                            && o.CompletionDateTime.HasValue
                                            && o.CompletionDateTime < (order.CompletionDateTime ?? DateTime.Now)
                                    )
                                    .OrderBy(o => o.CompletionDateTime)
                                    .ToList();

            int poliboxs = 0;
            previousPoliboxRemainingBags = null;
            previousOrderBagsPerPolibox = null;
            foreach (var previousOrder in previousOrders)
            {
                int previousOrderPoliboxs = (previousOrder.Bags - previousPoliboxRemainingBags ?? 0) / previousOrder.BagsPerPolibox;
                if (previousOrderPoliboxs < 0)
                    previousOrderPoliboxs = 0;

                if (previousOrderPoliboxs > 0)
                    previousOrderBagsPerPolibox = previousOrder.BagsPerPolibox;

                poliboxs += previousOrderPoliboxs;
                previousPoliboxRemainingBags = (previousOrder.Bags - previousPoliboxRemainingBags) % previousOrder.BagsPerPolibox;
            }

            return poliboxs;
        }

        public int GetUsedPlacesInPreviousPolibox(int orderId, out int poliboxUsed)
        {
            var order = orderRepository.Entities
                            .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                throw new Exception();
            }

            var previousOrders = orderRepository.Entities
                                    .Where(o => o.CustomerAddressId == order.CustomerAddressId
                                            && o.OrderDate.Date == order.OrderDate.Date
                                            && o.CompletionDateTime != null
                                            && o.CompletionDateTime < (order.CompletionDateTime ?? DateTime.Now)
                                    )
                                    .OrderBy(o => o.CompletionDateTime)
                                    .ToList();

            int placesUsed = 0;
            poliboxUsed = 1;
            foreach (var previousOrder in previousOrders)
            {

                if (placesUsed >= previousOrder.BagsPerPolibox)
                {
                    placesUsed = 0;
                    poliboxUsed++;
                }

                for (int i = 0; i < previousOrder.Bags; i++)
                {
                    placesUsed++;
                    if (placesUsed >= previousOrder.BagsPerPolibox)
                    {
                        placesUsed = 0;
                        poliboxUsed++;
                    }
                }
            }

            return placesUsed;
        }
    }
}