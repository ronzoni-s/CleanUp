namespace CleanUp.WebApi.Sdk.Endpoints
{
    public static class UserEndpoints
    {
        public static string Get(string id) => $"v1/user/{id}";
        public static string GetAll => $"v1/user";
        public static string GetWorkDays(string id, DateTime fromDate, DateTime toDate) => $"v1/user/work-day/{id}?fromDate={fromDate.ToString("yyyy-MM-dd")}&toDate={toDate.ToString("yyyy-MM-dd")}";
        public static string CreateWorkDay() => $"v1/user/work-day";
    }
}