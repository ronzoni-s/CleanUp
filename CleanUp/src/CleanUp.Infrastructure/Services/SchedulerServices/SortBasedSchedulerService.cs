using CleanUp.Application.Common.Interfaces;
using CleanUp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Infrastructure.Services
{
    public class CleaningSlot : ISchedulerService
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

    public class SortBasedSchedulerService
    {
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
                    yield return new CleaningSlot
                    {
                        AvailableFrom = orderedEvents[i].EndTime,
                        AvailableTo = orderedEvents[i + 1].StartTime,
                        Capacity = orderedEvents[i].Classroom.Capacity,
                        CleaningDuration = CalculateCleaningDuration(orderedEvents[i]),
                        EventId = orderedEvents[i].Id,
                        OperationStart = null,
                        OperatorAssigned = null
                    };
                }
                yield return new CleaningSlot
                {
                    AvailableFrom = orderedEvents[i].EndTime,
                    AvailableTo = orderedEvents[i].EndTime.Date + new TimeSpan(19, 0, 0),
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

            return new TimeSpan(0, e.Id, 0);
        }

        public async Task<(int Operators, Dictionary<int, List<CleaningSlot>> ScheduledWithOp)> Schedule(List<CleaningSlot> cleaningSlots, List<CleanUpUser> operators)
        {
            var orderedSlots = cleaningSlots.OrderBy(x => x.AvailableFrom).ThenBy(x => x.AvailableTo).ToList();
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
                //CleaningSlot firstUsefulPosition = null;

                foreach (var operatorOrderedSlots in scheduledWithOp)
                {
                    if ((operatorOrderedSlots.Value.Last().OperationStart + operatorOrderedSlots.Value.Last().CleaningDuration) <= slot.AvailableFrom)
                    {
                        temp = slot.Clone();
                        temp.OperationStart = slot.AvailableFrom;
                        temp.OperatorAssigned = operatorOrderedSlots.Key;
                        operatorOrderedSlots.Value.Add(temp);

                        scheduled = true;
                        break;
                    }

                    if (operatorOrderedSlots.Value.Last().OperationStart + operatorOrderedSlots.Value.Last().CleaningDuration + slot.CleaningDuration <= slot.AvailableTo)
                    {
                        temp = slot.Clone();
                        temp.OperationStart = operatorOrderedSlots.Value.Last().OperationStart + operatorOrderedSlots.Value.Last().CleaningDuration;
                        temp.OperatorAssigned = operatorOrderedSlots.Key;
                        operatorOrderedSlots.Value.Add(temp);

                        scheduled = true;
                        break;
                    }
                }
                if (!scheduled)
                {
                    scheduledWithOp.Add(++operatorUsed, new List<CleaningSlot>());
                    temp = slot.Clone();
                    temp.OperatorAssigned = operatorUsed;
                    scheduledWithOp[operatorUsed].Add(temp);
                }
            }

            return (operatorUsed, scheduledWithOp);
        }

        public async Task<(int Operators, Dictionary<int, List<CleaningSlot>> ScheduledWithOp)> Schedule2(List<CleaningSlot> cleaningSlots, List<CleanUpUser> operators)
        {
            var orderedSlots = cleaningSlots.OrderBy(x => x.AvailableTo - x.AvailableFrom - x.CleaningDuration).ThenBy(x => x.AvailableFrom).ToList();
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
                //CleaningSlot firstUsefulPosition = null;

                foreach (var operatorOrderedSlots in scheduledWithOp)
                {
                    if ((operatorOrderedSlots.Value.Last().OperationStart + operatorOrderedSlots.Value.Last().CleaningDuration) <= slot.AvailableFrom)
                    {
                        temp = slot.Clone();
                        temp.OperationStart = slot.AvailableFrom;
                        temp.OperatorAssigned = operatorOrderedSlots.Key;
                        operatorOrderedSlots.Value.Add(temp);

                        scheduled = true;
                        break;
                    }

                    if (operatorOrderedSlots.Value.Last().OperationStart + operatorOrderedSlots.Value.Last().CleaningDuration + slot.CleaningDuration <= slot.AvailableTo)
                    {
                        temp = slot.Clone();
                        temp.OperationStart = operatorOrderedSlots.Value.Last().OperationStart + operatorOrderedSlots.Value.Last().CleaningDuration;
                        temp.OperatorAssigned = operatorOrderedSlots.Key;
                        operatorOrderedSlots.Value.Add(temp);

                        scheduled = true;
                        break;
                    }
                }
                if (!scheduled)
                {
                    scheduledWithOp.Add(++operatorUsed, new List<CleaningSlot>());
                    temp = slot.Clone();
                    temp.OperatorAssigned = operatorUsed;
                    scheduledWithOp[operatorUsed].Add(temp);
                }
            }

            return (operatorUsed, scheduledWithOp);
        }
    }
}
