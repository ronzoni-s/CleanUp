using fbognini.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Domain.Entities
{
    public class WorkDay : AuditableEntityWithIdentity<int>
    {
        public string UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public CleanUpUser User { get; set; }
    }
}
