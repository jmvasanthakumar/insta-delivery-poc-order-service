namespace InstaDelivery.OrderService.Api.Configuration;

public class SwaggerConfiguration
{
    public string ClientId { get; set; } = string.Empty;
    public string Scopes { get; set; } = string.Empty;
    public string TokenUrl { get; set; } = string.Empty;
    public string AuthorizeUrl { get; set; } = string.Empty;
}