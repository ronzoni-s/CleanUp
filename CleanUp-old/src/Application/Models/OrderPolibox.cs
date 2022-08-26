using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.Models
{
    public enum BagStatus
    {
        Full,
        Order,
        Empty,
    }

    public class OrderBag
    {
        public OrderBag(int number, BagStatus status)
        {
            Number = number;
            Status = status;
        }

        public int Number { get; set; }
        public BagStatus Status { get; set; }
    }

    public class OrderPolibox
    {

        public OrderPolibox(int number)
        {
            Number = number;
            Bags = new List<OrderBag>();
        }

        public int Number { get; set; }
        public List<OrderBag> Bags { get; set; }
    }
}
