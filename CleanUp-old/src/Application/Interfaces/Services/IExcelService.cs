using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanUp.Application.Interfaces.Services
{
    public interface IExcelService
    {
        Task<string> ExportAsync<TData>(IEnumerable<TData> data
            , Dictionary<string, Func<TData, object>> mappers
            , string sheetName = "Sheet1");

        Task<IEnumerable<TData>> ImportAsync<TData>(byte[] byteArray, Dictionary<int, string> mappers, bool skipFirstRow = false) where TData : class, new();
    }
}