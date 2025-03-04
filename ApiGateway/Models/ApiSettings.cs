namespace ApiGateway.Models
{   
    public class IdentityServerConfig {
        public required string Url { get; init; }
        public required string GrantType { get; init; }
        public required string ClientId { get; init; }
        public required string ClientSecret { get; init; }
        public required string Scope { get; init; }

    }
}
