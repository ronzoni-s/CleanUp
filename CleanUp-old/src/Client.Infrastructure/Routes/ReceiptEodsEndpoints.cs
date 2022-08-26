using System.Linq;

namespace CleanUp.Client.Infrastructure.Routes
{
    public static class ReceiptEodsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/receipteod?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            if (orderBy?.Any() == true)
            {
                foreach (var orderByPart in orderBy)
                {
                    url += $"{orderByPart},";
                }
                url = url[..^1]; // loose training ,
            }
            return url;
        }

        public static string PrintReceiptEod = "api/v1/receipteod/print-receipt-eod";
    }
}