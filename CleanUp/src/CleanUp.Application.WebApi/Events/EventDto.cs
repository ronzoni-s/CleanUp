﻿using CleanUp.Domain.Entities;
using fbognini.Core.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.Events
{
    public class EventDto : Mappable<EventDto, Event>
    {
        public int Id { get; set; }
        public string ClassroomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}
