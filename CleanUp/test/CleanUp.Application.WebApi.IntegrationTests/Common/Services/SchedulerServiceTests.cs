using CleanUp.Application.WebApi.Common.Services;
using CleanUp.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CleanUp.Application.WebApi.IntegrationTests.Common.Services
{
    public class SchedulerServiceTests
    {
        private readonly Mock<SchedulerService> sut = new();

        public SchedulerServiceTests()
        {

        }

        [Fact]
        public async Task BuildCleaningSlots_ShouldReturnSlots_WhenCorrect()
        {
            var events = new List<Event>
            {
                new Event()
                {
                    Id = 15,
                    ClassroomId = "aula-1",
                    StartTime = new DateTime(2022, 09, 27, 9, 0, 0),
                    EndTime = new DateTime(2022, 09, 27, 10, 0, 0),
                    IsActive = true,
                    Classroom = new Classroom() {Capacity = 100}
                },
                new Event()
                {
                    Id = 15,
                    ClassroomId = "aula-1",
                    StartTime = new DateTime(2022, 09, 27, 11, 0, 0),
                    EndTime = new DateTime(2022, 09, 27, 12, 0, 0),
                    IsActive = true,
                    Classroom = new Classroom() {Capacity = 100}
                },
                new Event()
                {
                    Id = 10,
                    ClassroomId = "aula-2",
                    StartTime = new DateTime(2022, 09, 27, 9, 10, 0),
                    EndTime = new DateTime(2022, 09, 27, 10, 10, 0),
                    IsActive = true,
                    Classroom = new Classroom() {Capacity = 100}
                },
                new Event()
                {
                    Id = 10,
                    ClassroomId = "aula-2",
                    StartTime = new DateTime(2022, 09, 27, 10, 20, 0),
                    EndTime = new DateTime(2022, 09, 27, 11, 20, 0),
                    IsActive = true,
                    Classroom = new Classroom() {Capacity = 100}
                },

            };
            //sut.Setup(x => x.CalculateCleaningDuration(It.IsAny<Event>()))
            //    .Returns(new TimeSpan(0, 15, 0));

            var result = await sut.Object.Schedule(events, null);

            Assert.Equal(2, result.Operators);
            //Assert.True(true);
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
                    AvailableTo = new DateTime(2022, 10, 08, 11, 00, 0),
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
            };
            //sut.Setup(x => x.CalculateCleaningDuration(It.IsAny<Event>()))
            //    .Returns(new TimeSpan(0, 15, 0));

            var result = await sut.Object.Schedule(cleaningSlots, null);

            Assert.Equal(1, result.Operators);
            //Assert.True(true);
        }
    }
}
