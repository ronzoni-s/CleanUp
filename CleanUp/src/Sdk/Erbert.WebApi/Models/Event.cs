using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.WebApi.Sdk.Models.Events
{
    public class Event
    {
        public int Id { get; set; }
        public string ClassroomId { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }

        public Classroom Classroom { get; set; }
    }
}
