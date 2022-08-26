using CleanUp.Client.Authentication;
using CleanUp.Client.Managers.Identity.Authentication;
using CleanUp.Client.Managers.Identity.Users;
using CleanUp.Client.Managers.Orders;

namespace CleanUp.Client.Services.User
{
    public class UserService : IUserService
    {
        private readonly ClientStateProvider stateProvider;
        private readonly IUserManager userManager;

        /// <summary>
        /// Default company address id selected by user
        /// </summary>
        public int? DefaultCompanyAddressId { get; set; }

        /// <summary>
        /// Select company address id from cart
        /// </summary>
        public int? SelectedCompanyAddressId { get; set; }



        public UserService(ClientStateProvider stateProvider, IUserManager userManager)
        {
            this.stateProvider = stateProvider;
            this.userManager = userManager;
        }

        public async Task<int?> GetAddressId()
        {
            if ((await stateProvider.GetAuthenticationStateAsync()).User.Identity?.IsAuthenticated != true)
                return null;

            if (DefaultCompanyAddressId == null)
            {
                var userResponse = await userManager.GetAsync();
                if (!userResponse.IsSuccess)
                {
                    throw new Exception(userResponse.Message);
                }
                this.DefaultCompanyAddressId = userResponse.Response.DefaultCompanyAddressId;
            }
            return SelectedCompanyAddressId ?? DefaultCompanyAddressId;
        }
    }
}
