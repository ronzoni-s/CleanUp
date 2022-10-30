namespace CleanUp.WebApi.Sdk.Endpoints
{
    public static class UserEndpoints
    {
        public static string Get(string id) => $"v1/user/{id}";
        public static string GetAll => $"v1/user";
        public static string Register => $"v1/user";
        public static string Delete(string userId) => $"v1/user/{userId}";
        public static string GetWorkDays(string id, DateTime fromDate, DateTime toDate) => $"v1/scheduler/work-day/{id}?from={fromDate.ToString("yyyy-MM-dd")}&to={toDate.ToString("yyyy-MM-dd")}";
        public static string CreateWorkDay() => $"v1/scheduler/work-day";
    }
}