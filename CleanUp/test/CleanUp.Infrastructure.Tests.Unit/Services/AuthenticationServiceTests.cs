using CleanUp.Application;
using CleanUp.Application.Common.Interfaces.Repositorys;
using CleanUp.Application.Common.Requests;
using CleanUp.Domain.Entities;
using CleanUp.Infrastructure.Services;
using fbognini.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CleanUp.Infrastructure.UnitTests.Authentication.Commands
{
    public class AuthenticationServiceTests
    {
        private readonly AuthenticationService sut;
        private readonly Mock<UserManager<CleanUpUser>> userManagerMock = new Mock<UserManager<CleanUpUser>>(new Mock<IUserStore<CleanUpUser>>().Object, null, null, null, null, null, null, null, null);
        private readonly Mock<RoleManager<CleanUpRole>> roleManagerMock = new Mock<RoleManager<CleanUpRole>>(new Mock<IRoleStore<CleanUpRole>>().Object, null, null, null, null);
        private readonly Mock<ILogger<AuthenticationService>> loggerMock = new();

        public AuthenticationServiceTests()
        {
            var options = Options.Create(new AuthenticationSettings() 
            {
                Secret = "S0M3RAN0MS3CR3T!1!MAG1C!1!"
            });
            sut = new AuthenticationService(userManagerMock.Object, roleManagerMock.Object, loggerMock.Object, options);
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

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCorrectCredentials()
        {
            // Arrange
            string email = "mario.rossi@email.com";
            var user = new CleanUpUser()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Mario",
                LastName = "Rossi",
                Email = email,
                EmailConfirmed = true
            };
            string password = "P@ssword1";

            userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user)
                .Verifiable();
            userManagerMock.Setup(x => x.CheckPasswordAsync(It.Is<CleanUpUser>(x => x.Email == email), password))
                .ReturnsAsync(true)
                .Verifiable();
            userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<CleanUpUser>()))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<CleanUpUser>()))
                .ReturnsAsync(new List<Claim>());
            userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<CleanUpUser>()))
                .ReturnsAsync(new List<string>());

            roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new CleanUpRole());
            roleManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<CleanUpRole>()))
                .ReturnsAsync(new List<Claim>());

            var command = new LoginRequest 
            {
                Email = user.Email,
                Password = password
            };

            //Act
            var result = await sut.LoginAsync(command);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.UserId);
            Assert.NotEmpty(result.Token);
            Assert.True(result.RefreshTokenExpiryTime >= DateTime.Now);
        }

        [Fact]
        public async Task Login_ShouldThrowException_WhenIncorrectPassword()
        {
            // Arrange
            string email = "mario.rossi@email.com";
            var user = new CleanUpUser()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Mario",
                LastName = "Rossi",
                Email = email,
                EmailConfirmed = true
            };
            string password = "P@ssword1";

            userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user)
                .Verifiable();
            userManagerMock.Setup(x => x.CheckPasswordAsync(It.Is<CleanUpUser>(x => x.Email == email), password))
                .ReturnsAsync(false)
                .Verifiable();
            userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<CleanUpUser>()))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<CleanUpUser>()))
                .ReturnsAsync(new List<Claim>());
            userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<CleanUpUser>()))
                .ReturnsAsync(new List<string>());

            roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new CleanUpRole());
            roleManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<CleanUpRole>()))
                .ReturnsAsync(new List<Claim>());

            var command = new LoginRequest
            {
                Email = user.Email,
                Password = password
            };

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => sut.LoginAsync(command));
        }

        [Fact]
        public async Task Login_ShouldThrowException_WhenEmailNotConfirmed()
        {
            // Arrange
            string email = "mario.rossi@email.com";
            var user = new CleanUpUser()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Mario",
                LastName = "Rossi",
                Email = email,
                EmailConfirmed = false
            };
            string password = "P@ssword1";

            userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user)
                .Verifiable();
            userManagerMock.Setup(x => x.CheckPasswordAsync(It.Is<CleanUpUser>(x => x.Email == email), password))
                .ReturnsAsync(true)
                .Verifiable();
            userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<CleanUpUser>()))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<CleanUpUser>()))
                .ReturnsAsync(new List<Claim>());
            userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<CleanUpUser>()))
                .ReturnsAsync(new List<string>());

            roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new CleanUpRole());
            roleManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<CleanUpRole>()))
                .ReturnsAsync(new List<Claim>());

            var command = new LoginRequest
            {
                Email = user.Email,
                Password = password
            };

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => sut.LoginAsync(command));
        }
    }
}
