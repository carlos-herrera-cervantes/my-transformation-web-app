namespace MyTransformationWeb.Domain.Config;

public static class ExternalServicesConfig
{
    public static class GatewayConfig
    {
        public static readonly string CoreApiHost = Environment.GetEnvironmentVariable("GATEWAY_HOST");

        public static readonly string CaloriesApiHost = Environment.GetEnvironmentVariable("CALORIES_API_HOST");
    }
}
