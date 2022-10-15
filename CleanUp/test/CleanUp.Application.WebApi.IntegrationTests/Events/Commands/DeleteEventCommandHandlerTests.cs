using AutoMapper;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Application.WebApi.Events;
using CleanUp.Application.WebApi.Events.Commands;
using CleanUp.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CleanUp.Application.WebApi.UnitTests.Events
{
    public class DeleteEventCommandHandlerTests
    {
        private readonly Mock<ICleanUpRepositoryAsync> repositoryMock = new();
        private readonly Mock<ILogger<DeleteEventCommand>> loggerMock = new();
        private readonly Mock<IMapper> mapperMock = new();

        public DeleteEventCommandHandlerTests()
        {
        }

        private void Mock(Event eventEntity, EventDto eventDto)
        {
            repositoryMock
                .Setup(x => x.DeleteByIdAsync<Event>(eventEntity.Id, CancellationToken.None))
                .ReturnsAsync(eventEntity);
            mapperMock
                .Setup(x => x.Map<EventDto>(eventEntity))
                .Returns(eventDto);
        }

        [Fact]
        public async Task DeleteCustomerCommandHandler_WhenEventExits()
        {
            // Arrange
            int eventId = 5;
            var eventEntity = new Event 
            {
                Id = eventId,
                ClassroomId = "A001",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(2),
                IsActive = true,
                Name = "Test event",
                Type =  "Type"
            };
            var eventDto = new EventDto
            {
                Id = eventId,
                ClassroomId = "A001",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(2),
                IsActive = true,
                Name = "Test event",
                Type = "Type"
            };

            Mock(eventEntity, eventDto);

            var command = new DeleteEventCommand(eventId);
            var handler = new DeleteEventCommand.DeleteEventCommandHandler(repositoryMock.Object, mapperMock.Object, loggerMock.Object);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(eventId, result.Id);
        }
    }
}
