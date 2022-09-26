//using CleanUp.Application.Common.Interfaces.Repositorys;
//using CleanUp.Application.WebApi.Authentication.Commands;
//using CleanUp.Domain.Entities;
//using CleanUp.Infrastructure.Services;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace CleanUp.Infrastructure.UnitTests.Authentication.Commands
//{
//    public class UserServiceTests
//    {
//        private readonly UserService sut;
//        private readonly Mock<UserManager<CleanUpUser>> userManagerMock = new();
//        private readonly Mock<RoleManager<CleanUpRole>> roleManagerMock = new();
//        private readonly Mock<ILogger<UserService>> loggerMock = new();

//        public UserServiceTests()
//        {
//            sut = new UserService(userManagerMock.Object, roleManagerMock.Object, loggerMock.Object);
//        }

//        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
//        {
//            var store = new Mock<IUserStore<TUser>>();
//            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
//            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
//            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

//            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
//            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
//            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

//            return mgr;
//        }

//        [Fact]
//        public async Task Login_ShouldReturnToken_WhenCorrectCredentials()
//        {
//            // Arrange
//            var 
            
//            repositoryMock
//                .Setup(x => x.GetAllAsync<Event>(null, CancellationToken.None))
//                .ReturnsAsync(new List<Event>());

//            repositoryMock
//                .Setup(x => x.GetAllAsync<Classroom>(null, CancellationToken.None))
//                .ReturnsAsync(new List<Classroom>());

//            var command = new UploadEventCommand
//            {
//                File = GetFormFile("export-eventi.xlsx"),
//            };
//            var handler = new UploadEventCommand.UploadEventCommandHandler(repositoryMock.Object, loggerMock.Object);

//            //Act
//            var result = await sut.

//            //Assert
//            Assert.Equal(Unit.Value, result);
//        }

//        [Fact]
//        public async Task UploadCustomerCommandHandler_WhenSmallFileCorrectFormat()
//        {
//            // Arrange
//            var events = new List<Event>()
//            {
//                new Event()
//                {
//                    Name = "DISPOSITIVI MEDICALI E DIAGNOSTICI",
//                    ClassroomId = "A203 [Edificio A - Dalmine]",
//                    StartTime = new DateTime(2022, 09, 20, 8, 30, 0),
//                    EndTime = new DateTime(2022, 09, 20, 10, 30, 0),
//                    Type = "Lezione",
//                    Teacher = "Andrea Remuzzi, Chiara Emma CAMPIGLIO",
//                    IsActive = true,
//                },
//                new Event()
//                {
//                    Name = "MODULO DI TECNOLOGIA ELEMENTI COSTRUTTIVI E BUILDING INFORMATION MODELING (BIM)",
//                    ClassroomId = "Lab. Galvani - piano1 [Laboratori - Dalmine]",
//                    StartTime = new DateTime(2022, 09, 20, 8, 30, 0),
//                    EndTime = new DateTime(2022, 09, 20, 10, 30, 0),
//                    Type = "Lezione",
//                    Teacher = "Giuseppe Ruscica",
//                    IsActive = true,
//                },
//                new Event()
//                {
//                    Name = "DOTTORATO ISA ''Sistemi di rinforzo attivi negli edifici esistenti in c.a.' Prof. Giuriani",
//                    ClassroomId = "B101 [Edificio B - Dalmine]",
//                    StartTime = new DateTime(2022, 10, 01, 9, 30, 0),
//                    EndTime = new DateTime(2022, 10, 01, 13, 30, 0),
//                    Type = "Dottorato",
//                    Teacher = null,
//                    IsActive = false,
//                }
//            };
//            var classrooms = new List<Classroom>()
//            {
//                new Classroom
//                {
//                    Id = "A203 [Edificio A - Dalmine]"
//                },
//                new Classroom
//                {
//                    Id = "Lab. Galvani - piano1 [Laboratori - Dalmine]"
//                },
//                new Classroom
//                {
//                    Id = "B101 [Edificio B - Dalmine]"
//                }
//            };

//            List<Event> createdEvents = null;
//            List<Classroom> createdClassrooms = null;

//            repositoryMock
//                .Setup(x => x.GetAllAsync<Event>(null, CancellationToken.None))
//                .ReturnsAsync(new List<Event>());

//            repositoryMock
//                .Setup(x => x.GetAllAsync<Classroom>(null, CancellationToken.None))
//                .ReturnsAsync(new List<Classroom>());

//            repositoryMock
//                .Setup(x => x.CreateRange(It.IsAny<List<Event>>()))
//                .Callback<IEnumerable<Event>>(x => createdEvents = x.ToList());

//            repositoryMock
//                .Setup(x => x.CreateRange(It.IsAny<List<Classroom>>()))
//                .Callback<IEnumerable<Classroom>>(x => createdClassrooms = x.ToList());

//            var command = new UploadEventCommand
//            {
//                File = GetFormFile("export-eventi-2.xlsx"),
//            };
//            var handler = new UploadEventCommand.UploadEventCommandHandler(repositoryMock.Object, loggerMock.Object);

//            //Act
//            var result = await handler.Handle(command, CancellationToken.None);

//            //Assert
//            repositoryMock.Verify(x => x.CreateRange<Classroom>(It.IsAny<List<Classroom>>()), Times.Once);
//            AssertObjectEqual(classrooms, createdClassrooms);

//            repositoryMock.Verify(x => x.Update<Event>(It.IsAny<Event>()), Times.Never);

//            repositoryMock.Verify(x => x.CreateRange<Event>(It.IsAny<List<Event>>()), Times.Once);
//            AssertObjectEqual(events, createdEvents);

//            repositoryMock.Verify(x => x.Save(), Times.Once);

//            Assert.Equal(Unit.Value, result);
//        }

//        private void AssertObjectEqual<T>(T expected, T actual)
//        {
//            var expectedSerialized = JsonConvert.SerializeObject(expected);
//            var actualSerialized = JsonConvert.SerializeObject(actual);
//            Assert.Equal(expectedSerialized, actualSerialized);
//        }
//    }
//}
