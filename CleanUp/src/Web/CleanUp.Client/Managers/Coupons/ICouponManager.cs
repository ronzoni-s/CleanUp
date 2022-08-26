using CleanUp.Client.Models.Api;
using CleanUp.WebApi.Sdk.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanUp.Client.Managers.Companys
{
    public interface ICouponManager : IManager
    {
        Task<ApiResult<List<OrderSourceCompanyCatalogCoupon>>> GetCartCoupons(int catalogId, DateTime date);
    }
}