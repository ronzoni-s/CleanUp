namespace CleanUp.WebApi.Sdk.Endpoints
{
    public static class SchedulerEndpoints
    {
        public static string GetAll(DateTime fromDate, DateTime toDate) => $"v1/scheduler?fromDate={fromDate.ToString("yyyy-MM-dd")}&toDate={toDate.ToString("yyyy-MM-dd")}";
    }
}