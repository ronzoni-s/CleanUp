using fbognini.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Domain.Entities
{
    public class CleaningOperation : AuditableEntityWithIdentity
    {
        public int EventId { get; set; }
        public string UserId { get; set; }
        public DateTime Start { get; set; }
        public TimeSpan Duration { get; set; }  // Calcolato in base a event duration, capacità, ...

        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; } // (TimeSpan.MaxValue)

        public Event Event { get; set; }
        public CleanUpUser User { get; set; }
    }
}
