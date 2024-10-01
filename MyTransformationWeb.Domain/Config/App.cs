namespace MyTransformationWeb.Domain.Config;

public static class AppConfig
{
    public static readonly string Timezone = Environment.GetEnvironmentVariable("TIMEZONE");
}
