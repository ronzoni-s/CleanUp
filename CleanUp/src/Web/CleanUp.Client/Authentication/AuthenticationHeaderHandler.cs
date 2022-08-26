using Blazored.LocalStorage;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using CleanUp.Client.Constants.Storage;

namespace CleanUp.Client.Authentication
{
    public class AuthenticationHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorageService localStorage;

        public AuthenticationHeaderHandler(ILocalStorageService localStorage)
            => this.localStorage = localStorage;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization?.Scheme != "Bearer")
            {
                var savedToken = await this.localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);

                if (!string.IsNullOrWhiteSpace(savedToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
                }
            }

            AppendOrderSourceQueryParameter(request);

            return await base.SendAsync(request, cancellationToken);
        }

        private void AppendOrderSourceQueryParameter(HttpRequestMessage request)
        {
            var uriBuilder = new UriBuilder(request.RequestUri);
            string orderSource = "B2BPortale";
            if (string.IsNullOrEmpty(uriBuilder.Query))
            {
                uriBuilder.Query = $"orderSource={orderSource}";
            }
            else
            {
                uriBuilder.Query = $"{uriBuilder.Query}&orderSource={orderSource}";
            }

            request.RequestUri = uriBuilder.Uri;
        }
    }
}