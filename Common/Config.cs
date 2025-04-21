namespace FFBDraftAPI.Common
{
    public static class Config
    {
        public static string FFBDraftdbConnectionString
        {
            get
            {
                var connectionString = GetConfigValue("AppSettings:FFBDraftdbConnectionString");
                if (!string.IsNullOrEmpty(connectionString))
                {
                    return connectionString;
                }
                return Environment.GetEnvironmentVariable("FFBDraftdbConnectionString") ?? "Not Found";
            }
        }

        public static string UndraftedTeamId
        {
            get
            {
                var connectionString = GetConfigValue("AppSettings:UndraftedTeamId");
                if (!string.IsNullOrEmpty(connectionString))
                {
                    return connectionString;
                }
                return Environment.GetEnvironmentVariable("UndraftedTeamId") ?? " ";
            }
        }

        static IConfiguration? _cashedConfig;
        private static IConfiguration Configuration
        {
            get
            {
                if (_cashedConfig == null)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                    _cashedConfig = builder.Build();
                }
                return _cashedConfig;
            }
        }
        private static string? GetConfigValue(string environmentVariable)
        {
            var result = Configuration[environmentVariable];
            return result;
        }
    }
}
