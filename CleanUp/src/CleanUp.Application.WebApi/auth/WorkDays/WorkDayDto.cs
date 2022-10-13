using CleanUp.Domain.Entities;
using fbognini.Core.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.WorkDays
{
    public class WorkDayDto : Mappable<WorkDayDto, WorkDay>
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
