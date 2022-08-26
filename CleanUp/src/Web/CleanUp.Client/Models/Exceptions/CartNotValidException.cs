using CleanUp.WebApi.Sdk.Models.AdditionalData;

namespace CleanUp.Client.Models.Exceptions
{
    public class CartNotValidException : ApplicationException
    {
        public IList<ProductNotValid> ProductNotValids { get; set; }
        public IList<MenuNotValid> MenuNotValids { get; set; }

        public CartNotValidException(List<ProductNotValid> productNotValids, List<MenuNotValid> menuNotValids)
            : base()
        {
            this.ProductNotValids = productNotValids;
            this.MenuNotValids = menuNotValids;
        }
    }
}
