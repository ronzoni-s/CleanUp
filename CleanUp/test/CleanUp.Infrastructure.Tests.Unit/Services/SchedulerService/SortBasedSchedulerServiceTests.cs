using CleanUp.Application.Interfaces;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Domain.Entities;
using CleanUp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CleanUp.Infrastructure.UnitTests.Services.SchedulerService
{
    public class SortBasedSchedulerServiceTests
    {
        private readonly ITestOutputHelper output;

        private readonly Mock<ICleanUpRepositoryAsync> repositoryMock = new();
        private readonly Mock<IUserService> userServiceMock = new();

        private readonly Mock<SortBasedSchedulerService> sut;

        public SortBasedSchedulerServiceTests(ITestOutputHelper output)
        {
            this.output = output;

            sut = new(repositoryMock.Object, userServiceMock.Object);
        }

        [Fact]
        public async Task BuildCleaningSlots_ShouldReturnSlots_WhenCorrect()
        {
            var events = new List<Event>
            {
                new Event()
                {
                    Id = 1,
                    ClassroomId = "aula-1",
                    StartTime = new DateTime(2022, 09, 27, 9, 0, 0),
                    EndTime = new DateTime(2022, 09, 27, 10, 0, 0),
                    IsActive = true,
                    Classroom = new Classroom() {Capacity = 100}
                },
                new Event()
                {
                    Id = 2,
                    ClassroomId = "aula-1",
                    StartTime = new DateTime(2022, 09, 27, 11, 0, 0),
                    EndTime = new DateTime(2022, 09, 27, 12, 0, 0),
                    IsActive = true,
                    Classroom = new Classroom() {Capacity = 100}
                },
                new Event()
                {
                    Id = 3,
                    ClassroomId = "aula-2",
                    StartTime = new DateTime(2022, 09, 27, 9, 10, 0),
                    EndTime = new DateTime(2022, 09, 27, 10, 10, 0),
                    IsActive = true,
                    Classroom = new Classroom() {Capacity = 100}
                },
                new Event()
                {
                    Id = 4,
                    ClassroomId = "aula-2",
                    StartTime = new DateTime(2022, 09, 27, 10, 20, 0),
                    EndTime = new DateTime(2022, 09, 27, 11, 20, 0),
                    IsActive = true,
                    Classroom = new Classroom() {Capacity = 100}
                },
                new Event()
                {
                    Id = 5,
                    ClassroomId = "aula-3",
                    StartTime = new DateTime(2022, 09, 27, 9, 0, 0),
                    EndTime = new DateTime(2022, 09, 27, 10, 0, 0),
                    IsActive = true,
                    Classroom = new Classroom() {Capacity = 100}
                },

            };

            var operators = new List<CleanUpUser>() 
            {
                new CleanUpUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "sronzoni99@gmail.com",
                },
                new CleanUpUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "mario.rossi@gmail.com",
                },
                new CleanUpUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "paolo.bianchi@gmail.com",
                },
            };

            //sut.Setup(x => x.CalculateCleaningDuration(It.IsAny<Event>()))
            //    .Returns(new TimeSpan(0, 15, 0));

            var result = await sut.Object.Schedule(events, operators);

            output.WriteLine(JsonConvert.SerializeObject(result));

            Assert.Equal(events.Count, result.Count);
        }

        [Fact]
        public async Task Schedule_ShouldReturnSlots_WhenCorrect()
        {
            var cleaningSlots = new List<CleaningSlot>
            {
                new CleaningSlot()
                {
                    EventId = 1,
                    AvailableFrom = new DateTime(2022, 10, 08, 10, 0, 0),
                    AvailableTo = new DateTime(2022, 10, 08, 11, 0, 0),
                    Capacity = 100,
                    CleaningDuration = new TimeSpan(0, 15, 0)
                },
                new CleaningSlot()
                {
                    EventId = 1,
                    AvailableFrom = new DateTime(2022, 10, 08, 10, 10, 0),
                    AvailableTo = new DateTime(2022, 10, 08, 10, 20, 0),
                    Capacity = 100,
                    CleaningDuration = new TimeSpan(0, 10, 0)
                },
                new CleaningSlot()
                {
                    EventId = 1,
                    AvailableFrom = new DateTime(2022, 10, 08, 10, 0, 0),
                    AvailableTo = new DateTime(2022, 10, 08, 19, 0, 0),
                    Capacity = 100,
                    CleaningDuration = new TimeSpan(0, 10, 0)
                },
                new CleaningSlot()
                {
                    EventId = 1,
                    AvailableFrom = new DateTime(2022, 10, 08, 12, 0, 0),
                    AvailableTo = new DateTime(2022, 10, 08, 19, 0, 0),
                    Capacity = 100,
                    CleaningDuration = new TimeSpan(0, 10, 0)
                },
                new CleaningSlot()
                {
                    EventId = 1,
                    AvailableFrom = new DateTime(2022, 10, 08, 12, 0, 0),
                    AvailableTo = new DateTime(2022, 10, 08, 19, 0, 0),
                    Capacity = 100,
                    CleaningDuration = new TimeSpan(0, 15, 0)
                },
            };
            //sut.Setup(x => x.CalculateCleaningDuration(It.IsAny<Event>()))
            //    .Returns(new TimeSpan(0, 15, 0));

            var result = await sut.Object.Schedule(cleaningSlots, null);

            //Assert.Equal(1, result.Operators);
            //Assert.True(true);
        }

        //[Fact]
        //public async Task Reschedule()
        //{
        //    await sut.Object.Reschedule();
        //}
    }
}
