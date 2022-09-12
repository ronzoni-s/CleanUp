using fbognini.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Domain.Entities
{
    public class Classroom : AuditableEntityWithIdentity<string>
    {
        /// <summary>
        /// Numero di persone
        /// </summary>
        public int Capacity { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
