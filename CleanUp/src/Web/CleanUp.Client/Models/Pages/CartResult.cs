using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Models.AdditionalData;
using CleanUp.Client.Models.Api;

namespace CleanUp.Client.Models.Pages
{
    public class CartResult
    {
        public ApiResult<PriceOrder> ApiResult { get; set; }
        public List<ProductNotValid> ProductNotValids { get; set; }
        public List<MenuNotValid> MenuNotValids { get; set; }
    }
}
