using Chaos.Services.Interfaces;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;

namespace Chaos.Services
{

    public class Api : IApi
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public Api(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration)
        {
            _tokenAcquisition=tokenAcquisition;
            _httpClient=httpClient;
            _configuration=configuration;
        }

        public async Task<string> GetToken()
        {
            string token = await GetAndAddApiAccessTokenToAuthorizationHeaderAsync();
            return token;

        }

        private async Task<string> GetAndAddApiAccessTokenToAuthorizationHeaderAsync()
        {
            string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { $"https://microsoft.onmicrosoft.com/e006f82c-3e58-4bdb-9f8d-6ac589840704/access_as_user" });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return accessToken;
        }
    }
}
