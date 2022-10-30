using AutoMapper;
using CleanUp.Application;
using CleanUp.Application.Authorization;
using CleanUp.Application.Interfaces;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Application.SearchCriterias;
using CleanUp.Domain.Entities;
using fbognini.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.Data;

namespace CleanUp.Application.WebApi.CleaningOperations.Queries
{
    public class GetDailyCleaningOperationsReportQuery : IRequest<byte[]>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public class GetDailyCleaningOperationsReportQueryHandler : IRequestHandler<GetDailyCleaningOperationsReportQuery, byte[]>
        {
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IUserService userService;
            private readonly ICurrentUserService currentUserService;
            private readonly IMapper mapper;
            private readonly ILogger<GetDailyCleaningOperationsReportQuery> logger;

            public GetDailyCleaningOperationsReportQueryHandler(
                ICleanUpRepositoryAsync repository
                , IUserService userService
                , ICurrentUserService currentUserService
                , IMapper mapper
                , ILogger<GetDailyCleaningOperationsReportQuery> logger
                )
            {
                this.repository = repository;
                this.userService = userService;
                this.currentUserService = currentUserService;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<byte[]> Handle(GetDailyCleaningOperationsReportQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var criteria = new CleaningOperationSearchCriteria
                    {
                        FromDate = request.FromDate,
                        ToDate = request.ToDate,
                    };
                    criteria.Includes.Add(x => x.User);
                    criteria.Includes.Add(x => x.Event);

                    var cleaningOperations = await repository.GetAllAsync(criteria, cancellationToken);

                    return BuildReport(cleaningOperations);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while getting CleaningOperations");
                    throw;
                }
            }

            private byte[] BuildReport(List<CleaningOperation> cleaningOperations)
            {
                DataTable dataSource = new DataTable();
                dataSource.Columns.Add("EventId");
                dataSource.Columns.Add("EventName");
                dataSource.Columns.Add("ClassroomId");
                dataSource.Columns.Add("UserId");
                dataSource.Columns.Add("UserFullName");
                dataSource.Columns.Add("Start");
                dataSource.Columns.Add("Duration");

                foreach (var cleaningOperation in cleaningOperations)
                {
                    dataSource.Rows.Add(new object[] 
                    { 
                        cleaningOperation.EventId
                        , cleaningOperation.Event.Name
                        , cleaningOperation.Event.ClassroomId
                        , cleaningOperation.UserId
                        , cleaningOperation.User.FullName
                        , cleaningOperation.Start
                        , cleaningOperation.Duration 
                    });
                }
                
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report operazioni pulizia");

                    worksheet.Cells["A1"].LoadFromDataTable(dataSource, true, OfficeOpenXml.Table.TableStyles.Medium1);

                    using var stream = new MemoryStream();
                    package.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
