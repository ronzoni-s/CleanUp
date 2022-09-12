namespace CleanUp.WebApi.Sdk.Endpoints
{
    public static class UserEndpoints
    {
        public static string Get(string id) => $"v1/user/{id}";
    }
}