using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.WebApi.Sdk.Requests
{
    public class ReceiptEodTaxRateRequest
    {
        public int Tax { get; set; }
        public int FinancialAmount => PartialAmount + CancellationAmount;
        public int NetFinancialAmount { get; set; }
        public int PartialAmount => NetPartialAmount + TaxAmount;
        public int NetPartialAmount { get; set; }
        public int TaxAmount { get; set; }
        public int CancellationAmount => (int)Math.Round((double)NetCancellationAmount * (100 + Tax) / 100);
        public int NetCancellationAmount { get; set; }
    }
}
