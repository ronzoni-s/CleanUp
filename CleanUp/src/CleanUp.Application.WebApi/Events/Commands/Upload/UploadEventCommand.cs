using AutoMapper;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Domain.Entities;
using EFCore.BulkExtensions;
using fbognini.Core.Data;
using fbognini.Core.Entities;
using fbognini.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace CleanUp.Application.WebApi.Events.Commands
{
    public class UploadEventCommand : IRequest<Unit>
    {
        public IFormFile File { get; set; }

        public class UploadEventCommandHandler : IRequestHandler<UploadEventCommand, Unit>
        {
            private readonly ICleanUpRepositoryAsync repository;
            private readonly ILogger<UploadEventCommand> logger;

            private class EventImported
            {
                public string Name { get; set; }
                public string Classroom { get; set; }
                public DateTime Date { get; set; }
                public TimeSpan StartHour { get; set; }
                public TimeSpan EndHour { get; set; }
                public string Teacher { get; set; }
                public string Type { get; set; }
                public string State { get; set; }

                public DateTime StartTime => Date.Date.Add(StartHour);
                public DateTime EndTime => Date.Date.Add(EndHour);
            }

            public UploadEventCommandHandler(
                ICleanUpRepositoryAsync repository
                , ILogger<UploadEventCommand> logger
                )
            {
                this.repository = repository;
                this.logger = logger;
            }

            public async Task<Unit> Handle(UploadEventCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var eventsImported = await ReadFile(request.File, cancellationToken);

                    var existingEvents = await repository.GetAllAsync<Event>();
                    var existingClassrooms = await repository.GetAllAsync<Classroom>();

                    // L'importazione andrà ad aggiornare l'aula per un determinato corso in una fascia oraria o ad aggiungerli se non presenti
                    var classroomsToAdd = new List<Classroom>();
                    var eventsToAdd = new List<Event>();
                    var eventsToUpdate = new List<Event>();
                    foreach (var eventImported in eventsImported)
                    {
                        var alreadyExistingEvent = existingEvents.FirstOrDefault(e => IsSameEvent(eventImported, e));
                        if (alreadyExistingEvent == null)
                        {
                            alreadyExistingEvent = new Event()
                            { 
                                Name = eventImported.Name,
                                StartTime = eventImported.StartTime,
                                EndTime = eventImported.EndTime,
                            };
                            eventsToAdd.Add(alreadyExistingEvent);
                        }
                        else
                        {
                            eventsToUpdate.Add(alreadyExistingEvent);
                        }

                        if (!existingClassrooms.Any(x => x.Id == eventImported.Classroom) && !classroomsToAdd.Any(x => x.Id == eventImported.Classroom))
                        {
                            classroomsToAdd.Add(new Classroom() 
                            {
                                Id = eventImported.Classroom,
                            });
                        }

                        alreadyExistingEvent.ClassroomId = eventImported.Classroom;
                        alreadyExistingEvent.Teacher = eventImported.Teacher;
                        alreadyExistingEvent.Type = eventImported.Type;
                        alreadyExistingEvent.IsActive = eventImported.State == "Confermato";
                    }

                    Save(classroomsToAdd, eventsToUpdate, eventsToAdd);

                    return Unit.Value;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while uploading events");
                    throw;
                }
            }

            private bool IsSameEvent(EventImported eventImported, Event e) => e.Name == eventImported.Name && e.StartTime == eventImported.StartTime && e.EndTime == eventImported.EndTime;

            private async Task<List<EventImported>> ReadFile(IFormFile file, CancellationToken cancellationToken)
            {
                var eventImported = new List<EventImported>();

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream, cancellationToken);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using var p = new ExcelPackage(stream);
                    var worksheet = p.Workbook.Worksheets.First();

                    var rowCount = GetLastUsedRow(worksheet);
                    for (int rowIndex = 5; rowIndex <= rowCount; rowIndex++)
                    {
                        try
                        {
                            eventImported.Add(LoadRow(worksheet, rowIndex));
                        }
                        catch (BadRequestException ex)
                        {
                            throw new BadRequestException($"Riga {rowIndex}: {ex.Message}");
                        }
                    }

                }

                return eventImported;

                
                EventImported LoadRow(ExcelWorksheet worksheet, int rowIndex)
                {
                    var eventImported = new EventImported();

                    eventImported.Name = GetName();
                    eventImported.Classroom = GetClassroom();
                    eventImported.Date = GetDate();
                    eventImported.StartHour = GetStartTime();
                    eventImported.EndHour = GetEndTime();
                    eventImported.Teacher = GetTeacher();
                    eventImported.Type = GetType();
                    eventImported.State = GetState();

                    return eventImported;

                    string GetName()
                    {
                        var code = worksheet.Cells[rowIndex, 1].Value.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(code))
                            throw new BadRequestException("Nome non valido");

                        return code;
                    }

                    string GetClassroom() => worksheet.Cells[rowIndex, 2].Value.ToString().Trim();

                    DateTime GetDate()
                    {
                        var dateString = worksheet.Cells[rowIndex, 3].Value.ToString().Trim();
                        if (!DateTime.TryParseExact(dateString, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime date))
                            throw new BadRequestException("Formato data non valido");

                        return date;
                    }

                    TimeSpan GetStartTime() => GetTime(0);

                    TimeSpan GetEndTime() => GetTime(1);

                    TimeSpan GetTime(int index)
                    {
                        var timeString = worksheet.Cells[rowIndex, 4].Value.ToString().Split("-")[index].Trim();
                        if (!TimeSpan.TryParseExact(timeString, @"hh\:mm", System.Globalization.CultureInfo.InvariantCulture, out TimeSpan time))
                            throw new BadRequestException("Formato orario non valido");

                        return time;
                    }

                    string GetType() => worksheet.Cells[rowIndex, 5].Value.ToString().Trim();

                    string GetTeacher() => worksheet.Cells[rowIndex, 6].Value?.ToString()?.Trim();
                    
                    string GetState() => worksheet.Cells[rowIndex, 7].Value.ToString().Trim();
                }
            }

            private int GetLastUsedRow(ExcelWorksheet sheet)
            {
                if (sheet.Dimension == null) { return 0; } // In case of a blank sheet
                var row = sheet.Dimension.End.Row;
                while (row >= 1)
                {
                    var range = sheet.Cells[row, 1, row, sheet.Dimension.End.Column];
                    if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                    {
                        break;
                    }
                    row--;
                }
                return row;
            }

            private void Save(List<Classroom> classroomsToAdd, List<Event> eventsToUpdate, List<Event> eventsToAdd)
            {
                if (classroomsToAdd.Count > 0)
                {
                    repository.CreateRange(classroomsToAdd);
                }

                if (eventsToUpdate.Count > 0)
                {
                    foreach (var eventToUpdate in eventsToUpdate)
                    {
                        repository.Update(eventToUpdate);
                    }
                }

                if (eventsToAdd.Count > 0)
                {
                    repository.CreateRange(eventsToAdd);
                }

                repository.Save();
            }
        }
    }
}
