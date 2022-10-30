using CleanUp.Application.WebApi.Classrooms;
using CleanUp.Domain.Entities;
using fbognini.Core.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.Classrooms
{
    public class ClassroomDto : Mappable<ClassroomDto, Classroom>
    {
        public string Id { get; set; }
        public int Capacity { get; set; }
    }
}
