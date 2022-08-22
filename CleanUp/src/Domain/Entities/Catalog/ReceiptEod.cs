using CleanUp.Domain.Contracts;
using System;

namespace CleanUp.Domain.Entities.Catalog
{
    public class ReceiptEod : AuditableEntity<string>
    {
        public int ReceiptPrinterNumber { get; set; }
        public int ReceiptCashDesk { get; set; }
        public bool ReceiptStatus { get; set; }
        public DateTime ReceiptTime { get; set; }
        public int ReceiptNumber { get; set; }
        public double FinancialAmount { get; set; }
    }
}