using CleanUp.Application.Authorization;
using CleanUp.Application.Interfaces;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Application.SearchCriterias;
using CleanUp.Domain.Entities;
using Newtonsoft.Json;
using System.CodeDom;

namespace CleanUp.Infrastructure.Services
{
    public class CleaningSlot
    {
        [JsonIgnore]
        public int Capacity { get; set; }
        [JsonIgnore]
        public DateTime AvailableFrom { get; set; }
        [JsonIgnore]
        public DateTime AvailableTo { get; set; } // (TimeSpan.MaxValue)
        [JsonIgnore]
        public TimeSpan CleaningDuration { get; set; }  // Calcolato in base a event duration, capacità, ...
        [JsonIgnore]
        public int EventId { get; set; }
        [JsonIgnore]
        public int? OperatorAssigned { get; set; }
        public DateTime? OperationStart { get; set; }
        public DateTime? OperationEnd { get; set; }

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
                OperationEnd = this.OperationEnd,
            };
        }
    }

    public class SortBasedSchedulerService : ISchedulerService
    {
        private readonly ICleanUpRepositoryAsync repository;
        private readonly IUserService userService;
        private readonly IPushNotificationService pushNotificationService;

        public SortBasedSchedulerService(ICleanUpRepositoryAsync repository
            , IUserService userService
            , IPushNotificationService pushNotificationService)
        {
            this.repository = repository;
            this.userService = userService;
            this.pushNotificationService = pushNotificationService;
        }

        public async Task Reschedule(DateTime date)
        {
            var events = await GetEventsOfTheDay();
            var operators = await GetAvailableOperators();
            
            var newCleaningOperations = await Schedule(events, operators);
            await UpdateCleaningOperations(newCleaningOperations);


            async Task<List<Event>> GetEventsOfTheDay()
            {
                var criteria = new EventSearchCriteria() { FromDate = date, ToDate = date };
                criteria.Includes.Add(x => x.Classroom);
                return await repository.GetAllAsync<Event>(criteria);
            }

            async Task<List<CleanUpUser>> GetAvailableOperators()
            {
                var users = await userService.GetAll(RoleConstants.OperatorRole);
                //if (false)
                //{
                //    foreach (var user in users)
                //    {
                //        var criteria = new WorkDaySearchCriteria()
                //        {
                //            FromDate = date.Date,
                //            ToDate = date.Date
                //        };
                //        var workDays = await repository.GetAllAsync(criteria);
                //        if (workDays == null || workDays.Count == 0)
                //        {
                //            users.Remove(user);
                //        }
                //        else
                //        {
                //            user.WorkDays = workDays;
                //        }
                //    }
                //}
                return users;
            }

            async Task UpdateCleaningOperations(List<CleaningOperation> scheduledOperations)
            {
                var cleaningOperationToCreate = new List<CleaningOperation>();
                var cleaningOperationToDelete = new List<CleaningOperation>();
                
                var criteria = new CleaningOperationSearchCriteria 
                {
                    FromDate = date,
                    ToDate = date,
                };
                var alreadyScheduledOperations = await repository.GetAllAsync(criteria);
                var toDelete = alreadyScheduledOperations.Where(x => !scheduledOperations.Any(y => y.EventId == x.EventId));
                repository.DeleteRange(toDelete);
                
                foreach (var schedule in scheduledOperations)
                {
                    var found = alreadyScheduledOperations.FirstOrDefault(x => x.EventId == schedule.EventId);
                    if (found == null)
                    {
                        cleaningOperationToCreate.Add(schedule);
                    }
                    else
                    {
                        found.Start = schedule.Start;
                        found.UserId = schedule.UserId;
                        repository.Update(found);
                    }
                }

                repository.CreateRange(cleaningOperationToCreate);
                repository.Save();
            }
        }

        public async Task<List<CleaningOperation>> Schedule(List<Event> events, List<CleanUpUser> operators)
        {
            List<CleaningSlot> cleaningInterventions = new();

            foreach (var classromEvents in events.GroupBy(e => e.ClassroomId))
            {
                cleaningInterventions.AddRange(BuildCleaningSlots(classromEvents.ToList()));
            }

            var schedule =  await Schedule(cleaningInterventions, operators);

            return BuildCleaningOperations(schedule.Scheduled, schedule.Unscheduled).ToList();

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

            IEnumerable<CleaningOperation> BuildCleaningOperations(Dictionary<string, List<CleaningSlot>> scheduled, List<CleaningSlot> unscheduled)
            {
                foreach (var key in scheduled.Keys)
                {
                    foreach (var slot in scheduled[key])
                    {
                        yield return new CleaningOperation
                        {
                            AvailableFrom = slot.AvailableFrom,
                            AvailableTo = slot.AvailableTo,
                            Duration = slot.CleaningDuration,
                            EventId = slot.EventId,
                            Start = slot.OperationStart.Value,
                            UserId = key
                        };
                    }
                }

                //foreach (var slot in unscheduled)
                //{
                //    yield return new CleaningOperation
                //    {
                //        AvailableFrom = slot.AvailableFrom,
                //        AvailableTo = slot.AvailableTo,
                //        Duration = slot.CleaningDuration,
                //        EventId = slot.EventId,
                //        Start = slot.OperationStart.Value,
                //        UserId = null
                //    };
                //}
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

        public async Task<(List<CleaningSlot> Unscheduled, Dictionary<string, List<CleaningSlot>> Scheduled)> Schedule(List<CleaningSlot> cleaningSlots, List<CleanUpUser> operators)
        {
            var orderedSlots = cleaningSlots.OrderBy(x => x.AvailableTo).ThenBy(x => x.AvailableTo - x.AvailableFrom - x.CleaningDuration).ToList();

            CleaningSlot temp;
            Dictionary<string, List<CleaningSlot>> scheduledWithOp = new Dictionary<string, List<CleaningSlot>>();
            for (int i = 0; i < operators.Count && i < cleaningSlots.Count; i++)
            {
                temp = orderedSlots[i].Clone();
                temp.OperationStart = temp.AvailableFrom;
                temp.OperationEnd = temp.OperationStart + temp.CleaningDuration;
                scheduledWithOp.Add(operators[i].Id, new List<CleaningSlot>() { temp });
            }

            bool scheduled;
            List<CleaningSlot> missed = new List<CleaningSlot>();
            foreach (var slot in orderedSlots.Skip(operators.Count))
            {
                scheduled = false;
                List<Tuple<string, int>> tempList = new List<Tuple<string, int>>();
                foreach(var op in scheduledWithOp)
                {
                    tempList.Add(new Tuple<string, int>(op.Key, op.Value.Count));
                }
                tempList = tempList.OrderBy(x => x.Item2).ToList();

                foreach (var op in tempList)
                {
                    //var operatorOrderedSlots = scheduledWithOp[op.Item1];
                    if (scheduled)
                    {
                        break;
                    }
                    for (int i = 0; i < scheduledWithOp[op.Item1].Count; i++)
                    {
                        if (scheduledWithOp[op.Item1][i].OperationStart >= slot.AvailableTo)
                        {
                            break;
                        }
                        if (i == 0 && slot.AvailableFrom + slot.CleaningDuration <= scheduledWithOp[op.Item1][i].OperationStart)
                        {
                            temp = slot.Clone();
                            temp.OperationStart = slot.AvailableFrom;
                            temp.OperationEnd = temp.OperationStart + temp.CleaningDuration;
                            scheduledWithOp[op.Item1].Insert(0, temp);
                            scheduled = true;
                            break;
                        }
                        var end = scheduledWithOp[op.Item1][i].OperationStart + scheduledWithOp[op.Item1][i].CleaningDuration;
                        var maxStart = end > slot.AvailableFrom ? end : slot.AvailableFrom;
                        if (i == scheduledWithOp[op.Item1].Count - 1 && scheduledWithOp[op.Item1][i].OperationStart + scheduledWithOp[op.Item1][i].CleaningDuration + slot.CleaningDuration <= slot.AvailableTo)
                        {
                            temp = slot.Clone();
                            temp.OperationStart = maxStart;
                            temp.OperationEnd = temp.OperationStart + temp.CleaningDuration;
                            scheduledWithOp[op.Item1].Add(temp);
                            scheduled = true;
                            break;
                        }
                        if (i != scheduledWithOp[op.Item1].Count - 1 && (slot.AvailableFrom <= scheduledWithOp[op.Item1][i].OperationStart || (slot.AvailableFrom >= scheduledWithOp[op.Item1][i].OperationStart && slot.AvailableFrom <= scheduledWithOp[op.Item1][i+1].OperationStart)) && (scheduledWithOp[op.Item1][i + 1].OperationStart - maxStart) >= slot.CleaningDuration)
                        {
                            temp = slot.Clone();
                            temp.OperationStart = maxStart;
                            temp.OperationEnd = temp.OperationStart + temp.CleaningDuration;
                            scheduledWithOp[op.Item1].Insert(i+1, temp);
                            scheduled = true;
                            break;
                        }
                    }
                }
                if (!scheduled)
                {
                    temp = slot.Clone();
                    missed.Add(temp);
                }
            }

            return (missed, scheduledWithOp);
        }
        public async Task<(int Operators, Dictionary<int, List<CleaningSlot>> ScheduledWithOp)> MinNumberOfOperatorsNeeded(List<CleaningSlot> cleaningSlots)
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
                        if (i != operatorOrderedSlots.Value.Count - 1 && (slot.AvailableFrom <= operatorOrderedSlots.Value[i].OperationStart || (slot.AvailableFrom >= operatorOrderedSlots.Value[i].OperationStart && slot.AvailableFrom <= operatorOrderedSlots.Value[i + 1].OperationStart)) && (operatorOrderedSlots.Value[i + 1].OperationStart - maxStart) >= slot.CleaningDuration)
                        {
                            temp = slot.Clone();
                            temp.OperationStart = maxStart;
                            temp.OperatorAssigned = operatorOrderedSlots.Key;
                            operatorOrderedSlots.Value.Insert(i + 1, temp);
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
