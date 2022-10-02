using CleanUp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.Common.Services
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

    public class SchedulerService
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
                for (i = 0; i < orderedEvents.Count() -1; i++)
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
                    AvailableTo = orderedEvents[i].EndTime.Date + new TimeSpan(19,0,0),
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
            var scheduledWithOp = new Dictionary<int, List<CleaningSlot>>();
            scheduledWithOp.Add(operatorUsed, new List<CleaningSlot>());

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

            return (operatorUsed, scheduledWithOp) ;


            //var assignedCleaningSlots = new List<CleaningSlot>() 
            //{
            //    new CleaningSlot 
            //    {
            //        AvailableFrom = new DateTime(10,30,0),
            //        AvailableTo = new DateTime(10,40,0),
            //        CleaningDuration = new TimeSpan(0,10,0),
            //        EventId = 1,
            //        OperatorAssigned = "",
            //        OperationStart = new DateTime(10,30,0)
            //    },
            //    new CleaningSlot
            //    {
            //        AvailableFrom = new DateTime(11,30,0),
            //        AvailableTo = new DateTime(11,40,0),
            //        CleaningDuration = new TimeSpan(0,10,0),
            //        EventId = 1,
            //        OperatorAssigned = "",
            //        OperationStart = new DateTime(11,30,0)
            //    }
            //};
            ////Dictionary<int, List<CleaningSlot>> operatorSlots = new();


            //foreach (var slot in orderedSlots)
            //{
            //    //slot = new CleaningSlot
            //    //{
            //    //    AvailableFrom = new DateTime(10, 0, 0),
            //    //    AvailableTo = new DateTime(10, 40, 0),
            //    //    CleaningDuration = new TimeSpan(0, 10, 0),
            //    //    EventId = 1,
            //    //}

            //    int i;
            //    for (i = 0; i < assignedCleaningSlots.Count; i++)
            //    {
            //        if (! (slot.AvailableFrom < assignedCleaningSlots[i].OperationStart))
            //        {
            //            continue;
            //        }


            //        if (i == assignedCleaningSlots.Count - 1)
            //        {
            //            if (slot.AvailableFrom <= assignedCleaningSlots[i-1].OperationStart && slot.AvailableTo >= assignedCleaningSlots[i - 1].OperationStart + assignedCleaningSlots[i - 1].CleaningDuration)
            //            {
            //                // qui dobbiamo controllare se dopo operation start dell'evento precedente siamo ancora nel limite del nostro AvailableTo.
            //                // TODO
            //            }
            //            else
            //            {
            //                // aggiungi in coda senza nessuna condizione
            //            }

            //            break;
            //        }


            //        if (assignedCleaningSlots[i].OperationStart - Math.Max(slot.AvailableFrom, assignedCleaningSlots[i - 1].OperationStart + assignedCleaningSlots[i - 1].CleaningDuration) >= slot.CleaningDuration)
            //        {

            //        }
            //    }
            //}
        }
    }
}
