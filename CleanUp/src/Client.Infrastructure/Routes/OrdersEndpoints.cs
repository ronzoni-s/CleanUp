using System.Linq;

namespace CleanUp.Client.Infrastructure.Routes
{
    public static class OrdersEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy, bool hideCompleted, bool hideVoided)
        {
            var url = $"api/v1/orders?pageNumber={pageNumber}&pageSize={pageSize}" +
                $"&searchString={searchString}" +
                $"&hideCompleted={hideCompleted}&hideVoided={hideVoided}" +
                $"&orderBy=";

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

        public static string GetById(int id)
        {
            return $"api/v1/orders/{id}";
        }
        public static string GetNextOrderId(int id)
        {
            return $"api/v1/orders/{id}/next-order";
        }
        public static string GetPreviousOrderId(int id)
        {
            return $"api/v1/orders/{id}/previous-order";
        }

        public static string GetCount = "api/v1/orders/count";

        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string PrintPoliboxLabel = "api/v1/orders/print-polibox-label";
        public static string PrintProducts = "api/v1/orders/print-products";
        public static string Complete = "api/v1/orders/complete";
        public static string Void = "api/v1/orders/void";
        public static string Manage = "api/v1/orders/manage";

        public static string Save = "api/v1/orders";
        public static string Delete = "api/v1/orders";
        public static string Export = "api/v1/orders/export";
    }
}