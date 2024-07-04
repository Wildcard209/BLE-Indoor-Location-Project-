using Microsoft.AspNetCore.Authentication;
using System.Drawing;
using System.Net.Http.Headers;

namespace WebClient.Handler
{
    public class HttpClientAuthorizationHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpClientAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(_httpContextAccessor.HttpContext == null)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            string? accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
