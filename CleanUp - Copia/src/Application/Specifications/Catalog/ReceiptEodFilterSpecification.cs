using ErbertPranzi.Application.Specifications.Base;
using ErbertPranzi.Domain.Entities.Catalog;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ErbertPranzi.Application.Specifications.Catalog
{
    public class ReceiptEodFilterSpecification : HeroSpecification<ReceiptEod>
    {
        public ReceiptEodFilterSpecification(string searchString)
        {


            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = r => r.Id.Contains(searchString) ||
                                r.FinancialAmount.ToString().Contains(searchString) ||
                                r.ReceiptCashDesk.ToString().Contains(searchString) ||
                                r.ReceiptNumber.ToString().Contains(searchString) ||
                                r.ReceiptPrinterNumber.ToString().Contains(searchString) ||
                                r.ReceiptTime.ToString().Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}