using Microsoft.AspNetCore.Authentication;

namespace Mango.Service.ShoppingCartAPI.Utility
{
    public class BackendApiAuthencationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public BackendApiAuthencationHttpClientHandler(IHttpContextAccessor accessor) 
        {
            httpContextAccessor = accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }

    }
}
