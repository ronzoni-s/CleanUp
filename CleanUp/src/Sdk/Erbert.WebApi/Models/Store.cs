using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.WebApi.Sdk.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PostalCode { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public bool IsActive { get; set; }

        public bool UseReceiptPrinterApi { get; set; }

        public int? ReceiptPrinterNumber { get; set; }
        public string ReceiptPrinterApi { get; set; }
        public string ReceiptPrinterIp { get; set; }
        public string PickingPrinterName { get; set; }
        public string LabelPrinterName { get; set; }

    }
}
