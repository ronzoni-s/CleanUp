namespace CleanUp.Client.Models.Pages
{

    public class DishFilterCallback
    {
        public List<CategoryDTO> Categories { get; set; }

        public DishFilterCallback(List<CategoryDTO> categories, bool hideUnavailableProducts)
        {
            Categories = categories;
            HideUnavailableProducts = hideUnavailableProducts;
        }

        public bool HideUnavailableProducts { get; set; }
    }
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
