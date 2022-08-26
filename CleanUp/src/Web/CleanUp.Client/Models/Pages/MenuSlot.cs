using CleanUp.WebApi.Sdk.Models;

namespace CleanUp.Client.Models.Pages
{
    public class MenuSlot
    {
        public int GroupId { get; set; }
        public int SelectedMenuSlotProduct { get; set; } = 0;
        public List<MenuSlotProduct> MenuSlotProducts { get; set; }

        public Group Group { get; set; }

        public MenuSlotProduct Selected
        {
            get => MenuSlotProducts[SelectedMenuSlotProduct];
        }

        public bool LoadingDetails { get; set; } = false;
    }

    public class MenuSlotProduct
    {

        public int ProductId { get; set; }

        public Product Product { get; set; }
        public double PriceDifference { get; set; } = 0.0;

    }
}
