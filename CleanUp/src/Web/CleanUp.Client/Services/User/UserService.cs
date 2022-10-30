using CleanUp.Client.Authentication;
using CleanUp.Client.Managers.Identity.Authentication;
using CleanUp.Client.Managers.Identity.Users;

namespace CleanUp.Client.Services.User
{
    public class UserService : IUserService
    {
        private readonly ClientStateProvider stateProvider;
        private readonly UserManager userManager;

        public UserService(ClientStateProvider stateProvider, UserManager userManager)
        {
            this.stateProvider = stateProvider;
            this.userManager = userManager;
        }
    }
}
