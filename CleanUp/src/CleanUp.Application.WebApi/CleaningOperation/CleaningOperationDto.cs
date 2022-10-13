using CleanUp.Application.WebApi.Classrooms;
using CleanUp.Application.WebApi.Events;
using CleanUp.Application.WebApi.Users;
using CleanUp.Domain.Entities;
using fbognini.Core.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.CleaningOperations
{
    public class CleaningOperationDto : Mappable<CleaningOperationDto, CleaningOperation>
    {
        public int EventId { get; set; }
        public string UserId { get; set; }
        public DateTime Start { get; set; }
        public TimeSpan Duration { get; set; }  // Calcolato in base a event duration, capacità, ...

        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; } // (TimeSpan.MaxValue)

        public EventDto Event { get; set; }
        public UserDto User { get; set; }
    }
}
