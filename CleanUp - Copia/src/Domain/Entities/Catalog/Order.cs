using ErbertPranzi.Domain.Contracts;
using System;
using System.Collections.Generic;

namespace ErbertPranzi.Domain.Entities.Catalog
{
    public class Order : AuditableEntity<int>
    {
        public int OrderNumber { get; set; }
        public int CustomerAddressId { get; set; }
        public string ContactName { get; set; }
        public string ContactPhoneNumber { get; set; }
        public double Price { get; set; }
        public double FinalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public double WeightPerBag { get; set; }
        public double Weight { get; set; }
        public int BagsPerPolibox { get; set; }
        public int Bags { get; set; }
        public int? UsedBags { get; set; }
        public DateTime? CompletionDateTime { get; set; }
        public DateTime? CancellationDateTime { get; set; }

        public int? ReceiptPrinterNumber { get; set; }
        public int? ReceiptCashDesk { get; set; }
        public bool? ReceiptStatus { get; set; }
        public DateTime? ReceiptTime { get; set; }
        public int? ReceiptNumber { get; set; }
        public int? ReceiptEod { get; set; }
        public double? ReceiptTotal { get; set; }
        public string ReceiptErrorDescription { get; set; }

        public int? VoidReceiptPrinterNumber { get; set; }
        public int? VoidReceiptCashDesk { get; set; }
        public bool? VoidReceiptStatus { get; set; }
        public DateTime? VoidReceiptTime { get; set; }
        public int? VoidReceiptNumber { get; set; }
        public int? VoidReceiptEod { get; set; }
        public double? VoidReceiptTotal { get; set; }
        public string VoidReceiptErrorDescription { get; set; }


        public CustomerAddress CustomerAddress { get; set; }
        public IList<OrderMenu> OrderMenus { get; set; }
        public IList<OrderProduct> OrderProducts { get; set; }
    }
}