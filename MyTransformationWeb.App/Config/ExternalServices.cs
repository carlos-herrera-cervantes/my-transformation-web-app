namespace MyTransformationWeb.App.Config;

public static class ExternalServicesConfig
{
    public static class GatewayConfig
    {
        public static readonly string Host = Environment.GetEnvironmentVariable("GATEWAY_HOST");
    }
}
