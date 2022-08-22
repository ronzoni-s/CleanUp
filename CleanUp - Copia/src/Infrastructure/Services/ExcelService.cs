using ErbertPranzi.Application.Interfaces.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.IO;
using AutoMapper;

namespace ErbertPranzi.Infrastructure.Services
{
    public class ExcelService : IExcelService
    {
        private readonly IStringLocalizer<ExcelService> _localizer;
        private readonly IMapper _mapper;

        public ExcelService(IStringLocalizer<ExcelService> localizer, IMapper mapper)
        {
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<string> ExportAsync<TData>(IEnumerable<TData> data
            , Dictionary<string, Func<TData, object>> mappers
            , string sheetName = "Sheet1")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var p = new ExcelPackage();
            p.Workbook.Properties.Author = "BlazorHero";
            p.Workbook.Worksheets.Add(_localizer["Audit Trails"]);
            var ws = p.Workbook.Worksheets[0];
            ws.Name = sheetName;
            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";

            var colIndex = 1;
            var rowIndex = 1;

            var headers = mappers.Keys.Select(x => x).ToList();

            foreach (var header in headers)
            {
                var cell = ws.Cells[rowIndex, colIndex];

                var fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightBlue);

                var border = cell.Style.Border;
                border.Bottom.Style =
                    border.Top.Style =
                        border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                cell.Value = header;

                colIndex++;
            }

            var dataList = data.ToList();
            foreach (var item in dataList)
            {
                colIndex = 1;
                rowIndex++;

                var result = headers.Select(header => mappers[header](item));

                foreach (var value in result)
                {
                    ws.Cells[rowIndex, colIndex++].Value = value;
                }
            }

            using (ExcelRange autoFilterCells = ws.Cells[1, 1, dataList.Count + 1, headers.Count])
            {
                autoFilterCells.AutoFilter = true;
                autoFilterCells.AutoFitColumns();
            }

            var byteArray = await p.GetAsByteArrayAsync();
            return Convert.ToBase64String(byteArray);
        }

        public async Task<IEnumerable<TData>> ImportAsync<TData>(byte[] byteArray, Dictionary<int, string> mappers, bool skipFirstRow = false) where TData : class, new()
        {
            var ms = new MemoryStream(byteArray);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var p = new ExcelPackage(ms);
            
            var columnsToTake = mappers.Keys.Select(x => x).ToList();
            var data = new List<TData>();
            foreach (var worksheet in p.Workbook.Worksheets)
            {
                var rowCount = worksheet.Dimension.End.Row;
                for (int rowIndex = (skipFirstRow ? 2 : 1); rowIndex <= rowCount; rowIndex++)
                {
                    var obj = new TData();
                    var objType = obj.GetType();
                    foreach (var colToTake in columnsToTake)
                    {
                        Console.WriteLine(worksheet.Cells[rowIndex, 1].Value.ToString().Trim());
                        var property = objType.GetProperty(mappers[colToTake]);
                        var type = property.PropertyType;
                        property.SetValue(obj, Convert.ChangeType(worksheet.Cells[rowIndex, colToTake+1].Value, type), null);
                    }
                    data.Add(obj);
                }
            }

            return data;
        }

    }
}