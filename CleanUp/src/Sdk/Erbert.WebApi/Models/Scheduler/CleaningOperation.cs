using CleanUp.WebApi.Sdk.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.WebApi.Sdk.Models
{
    public class CleaningOperation
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string UserId { get; set; }
        public DateTime Start { get; set; }
        public TimeSpan Duration { get; set; }  // Calcolato in base a event duration, capacità, ...

        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; } // (TimeSpan.MaxValue)

        public Event Event { get; set; }
        //public User User { get; set; }
    }
}
