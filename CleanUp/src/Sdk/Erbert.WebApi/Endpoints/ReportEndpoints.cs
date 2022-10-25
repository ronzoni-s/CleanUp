namespace CleanUp.WebApi.Sdk.Endpoints
{
    public static class ReportEndpoints
    {
        public static string GetCleaningOperations(DateTime fromDate, DateTime toDate) => $"v1/report/daily-cleaning-operations?fromDate={fromDate.ToString("yyyy-MM-dd")}&toDate={toDate.ToString("yyyy-MM-dd")}";
    }
}