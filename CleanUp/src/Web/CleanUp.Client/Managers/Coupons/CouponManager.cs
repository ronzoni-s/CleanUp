using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Endpoints;
using CleanUp.Client.Models.Api;
using CleanUp.Client.Extensions;

namespace CleanUp.Client.Managers.Companys
{
    public class CouponManager : ICouponManager
    {
        private readonly HttpClient _httpClient;

        public CouponManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResult<List<OrderSourceCompanyCatalogCoupon>>> GetCartCoupons(int catalogId, DateTime date)
        {
            var response = await _httpClient.GetAsync(CouponEndpoints.GetCartCoupons(catalogId, date));
            return await response.ToResult<List<OrderSourceCompanyCatalogCoupon>>();
        }
    }
}