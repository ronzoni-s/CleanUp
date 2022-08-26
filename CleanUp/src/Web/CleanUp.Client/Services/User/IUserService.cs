namespace CleanUp.Client.Services.User
{
    public interface IUserService
    {
        int? DefaultCompanyAddressId { get; set; }
        int? SelectedCompanyAddressId { get; set; }

        Task<int?> GetAddressId();
    }
}
