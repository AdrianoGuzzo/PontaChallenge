using System.Text.Json.Serialization;

namespace ApiGateway.Models
{
    public record Token
    {
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; init; }
        [JsonPropertyName("expires_in")]
        public required decimal ExpiresIn { get; init; }
        [JsonPropertyName("token_type")]
        public required string TokenType { get; init; }
    }
}
