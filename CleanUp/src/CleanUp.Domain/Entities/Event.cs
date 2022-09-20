using fbognini.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Domain.Entities
{
    public class Event : AuditableEntityWithIdentity<int>
    {
        public string ClassroomId { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }

        public Classroom Classroom { get; set; }
    }
}
