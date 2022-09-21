namespace CleanUp.WebApi.Sdk.Endpoints
{
    public static class EventEndpoints
    {
        public static string Get(int id) => $"v1/event/{id}";
        public static string GetAll => $"v1/event";
        public static string Upload => $"v1/event/upload";
    }
}