using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.WebApi.Sdk.Models
{
    public class WorkDay
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
