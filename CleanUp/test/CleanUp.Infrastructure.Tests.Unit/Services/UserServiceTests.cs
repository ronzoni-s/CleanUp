using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Domain.Entities;
using CleanUp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CleanUp.Infrastructure.UnitTests.Authentication.Commands
{
    public class UserServiceTests
    {
        private readonly UserService sut;
        private readonly Mock<UserManager<CleanUpUser>> userManagerMock = new();
        private readonly Mock<RoleManager<CleanUpRole>> roleManagerMock = new();
        private readonly Mock<ILogger<UserService>> loggerMock = new();

        public UserServiceTests()
        {
            sut = new UserService(userManagerMock.Object, roleManagerMock.Object, loggerMock.Object);
        }

        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }
    }
}
