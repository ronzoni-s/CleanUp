using CleanUp.Application.Interfaces;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Application.SearchCriterias;
using CleanUp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Infrastructure.Services
{
    public class CleaningSlot
    {
        public int Capacity { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; } // (TimeSpan.MaxValue)
        public TimeSpan CleaningDuration { get; set; }  // Calcolato in base a event duration, capacità, ...
        public int EventId { get; set; }

        public int? OperatorAssigned { get; set; }
        public DateTime? OperationStart { get; set; }

        public CleaningSlot Clone()
        {
            return new CleaningSlot
            {
                AvailableFrom = this.AvailableFrom,
                AvailableTo = this.AvailableTo,
                Capacity = this.Capacity,
                CleaningDuration = this.CleaningDuration,
                EventId = this.EventId,
                OperationStart = this.OperationStart,
                OperatorAssigned = this.OperatorAssigned,
            };
        }
    }

    public class SortBasedSchedulerService : ISchedulerService
    {
        private readonly ICleanUpRepositoryAsync repository;
        public SortBasedSchedulerService(ICleanUpRepositoryAsync repository)
        {
            this.repository = repository;
        }

        public async Task<string> Reschedule(DateTime date)
        {
            var criteria = new EventSearchCriteria() { FromDate = date, ToDate = date };
            criteria.Includes.Add(x => x.Classroom);
            var events = await repository.GetAllAsync<Event>(criteria);
            var res = await Schedule(events, null);
            return JsonConvert.SerializeObject(res.ScheduledWithOp);
        }

        public async Task<(int Operators, Dictionary<int, List<CleaningSlot>> ScheduledWithOp)> Schedule(List<Event> events, List<CleanUpUser> operators)
        {
            List<CleaningSlot> cleaningInterventions = new();

            foreach (var classromEvents in events.GroupBy(e => e.ClassroomId))
            {
                cleaningInterventions.AddRange(BuildCleaningSlots(classromEvents.ToList()));
            }

            return await Schedule(cleaningInterventions, operators);

            // GetRanges in pyhton
            IEnumerable<CleaningSlot> BuildCleaningSlots(List<Event> classroomEvents)
            {
                var orderedEvents = classroomEvents.OrderBy(ce => ce.StartTime).ToList();
                int i;
                for (i = 0; i < orderedEvents.Count() - 1; i++)
                {
                    var duration = CalculateCleaningDuration(orderedEvents[i]);
                    if (orderedEvents[i + 1].StartTime - orderedEvents[i].EndTime < duration)
                    {
                        continue;
                    }
                    yield return new CleaningSlot
                    {
                        AvailableFrom = orderedEvents[i].EndTime,
                        AvailableTo = orderedEvents[i + 1].StartTime,
                        Capacity = orderedEvents[i].Classroom.Capacity,
                        CleaningDuration = duration,
                        EventId = orderedEvents[i].Id,
                        OperationStart = null,
                        OperatorAssigned = null
                    };
                }
                yield return new CleaningSlot
                {
                    AvailableFrom = orderedEvents[i].EndTime,
                    AvailableTo = orderedEvents[i].EndTime.Date + new TimeSpan(21, 0, 0),
                    Capacity = orderedEvents[i].Classroom.Capacity,
                    CleaningDuration = CalculateCleaningDuration(orderedEvents[i]),
                    EventId = orderedEvents[i].Id,
                    OperationStart = null,
                    OperatorAssigned = null
                };
            }
        }

        public TimeSpan CalculateCleaningDuration(Event e)
        {
            // TODO: da implementare
            if (e.EndTime - e.StartTime <= new TimeSpan(1, 0, 0))
            {
                return new TimeSpan(0, 5, 0);
            }
            if (e.EndTime - e.StartTime <= new TimeSpan(2, 0, 0))
            {
                return new TimeSpan(0, 10, 0);
            }
            return new TimeSpan(0, 15, 0);
        }

        public async Task<(int Operators, Dictionary<int, List<CleaningSlot>> ScheduledWithOp)> Schedule(List<CleaningSlot> cleaningSlots, List<CleanUpUser> operators)
        {
            var orderedSlots = cleaningSlots.OrderBy(x => x.AvailableTo).ThenBy(x => x.AvailableTo - x.AvailableFrom - x.CleaningDuration).ToList();
            int operatorUsed = 1;
            var scheduledWithOp = new Dictionary<int, List<CleaningSlot>>
            {
                { operatorUsed, new List<CleaningSlot>() }
            };

            var temp = orderedSlots.First().Clone();
            temp.OperationStart = temp.AvailableFrom;
            temp.OperatorAssigned = operatorUsed;
            scheduledWithOp[operatorUsed].Add(temp);

            bool scheduled;
            foreach (var slot in orderedSlots.Skip(1))
            {
                scheduled = false;

                foreach (var operatorOrderedSlots in scheduledWithOp)
                {
                    if (scheduled)
                    {
                        break;
                    }
                    for (int i = 0; i < operatorOrderedSlots.Value.Count; i++)
                    {
                        if (operatorOrderedSlots.Value[i].OperationStart >= slot.AvailableTo)
                        {
                            break;
                        }
                        //if (i != operatorOrderedSlots.Value.Count - 1 && operatorOrderedSlots.Value[i].OperationStart + slot.CleaningDuration < slot.AvailableFrom)
                        //{
                        //    continue;
                        //}
                        if (i == 0 && slot.AvailableFrom + slot.CleaningDuration <= operatorOrderedSlots.Value[i].OperationStart)
                        {
                            temp = slot.Clone();
                            temp.OperationStart = slot.AvailableFrom;
                            temp.OperatorAssigned = operatorOrderedSlots.Key;
                            operatorOrderedSlots.Value.Insert(0, temp);
                            scheduled = true;
                            break;
                        }
                        var end = operatorOrderedSlots.Value[i].OperationStart + operatorOrderedSlots.Value[i].CleaningDuration;
                        var maxStart = end > slot.AvailableFrom ? end : slot.AvailableFrom;
                        if (i == operatorOrderedSlots.Value.Count - 1 && operatorOrderedSlots.Value[i].OperationStart + operatorOrderedSlots.Value[i].CleaningDuration + slot.CleaningDuration <= slot.AvailableTo)
                        {
                            temp = slot.Clone();
                            temp.OperationStart = maxStart;
                            temp.OperatorAssigned = operatorOrderedSlots.Key;
                            operatorOrderedSlots.Value.Add(temp);
                            scheduled = true;
                            break;
                        }
                        if (i != operatorOrderedSlots.Value.Count - 1 && (slot.AvailableFrom <= operatorOrderedSlots.Value[i].OperationStart || (slot.AvailableFrom >= operatorOrderedSlots.Value[i].OperationStart && slot.AvailableFrom <= operatorOrderedSlots.Value[i+1].OperationStart)) && (operatorOrderedSlots.Value[i + 1].OperationStart - maxStart) >= slot.CleaningDuration)
                        {
                            temp = slot.Clone();
                            temp.OperationStart = maxStart;
                            temp.OperatorAssigned = operatorOrderedSlots.Key;
                            operatorOrderedSlots.Value.Insert(i+1, temp);
                            scheduled = true;
                            break;
                        }
                    }
                }
                if (!scheduled)
                {
                    scheduledWithOp.Add(++operatorUsed, new List<CleaningSlot>());
                    temp = slot.Clone();
                    temp.OperationStart = temp.AvailableFrom;
                    temp.OperatorAssigned = operatorUsed;
                    scheduledWithOp[operatorUsed].Add(temp);
                }
            }

            return (operatorUsed, scheduledWithOp);
        }
    }
}
