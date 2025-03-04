using ApiGateway.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ApiGateway.Security
{
    public class UserService
    {
        private readonly HttpClient _client;
        private readonly IdentityServerConfig _identityServerConfig;
        public UserService(IOptions<IdentityServerConfig> identityServerConfig)
        {
            _identityServerConfig = identityServerConfig.Value;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_identityServerConfig.Url)
            };
        }
        public async Task<Token?> GetToken(string login, string password)
        {
            var client = new HttpClient();

            var url = "https://localhost:5001/connect/token";

            var data = new Dictionary<string, string>
        {
            { "grant_type", _identityServerConfig.GrantType },
            { "client_id", _identityServerConfig.ClientId },
            { "client_secret", _identityServerConfig.ClientSecret },
            { "username", login },
            { "password", password },
            { "scope", _identityServerConfig.Scope },
        };
            var content = new FormUrlEncodedContent(data);

            try
            {
                var response = await client.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Resposta: " + responseContent);
                if (response.IsSuccessStatusCode)
                {
                    var obj = JsonSerializer.Deserialize<Token>(responseContent);
                    return obj ?? null;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao fazer a requisição: " + ex.Message);
            }
            return null;

        }
    }
}
