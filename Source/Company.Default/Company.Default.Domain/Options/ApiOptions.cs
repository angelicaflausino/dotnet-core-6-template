using Microsoft.Extensions.Configuration;

namespace Company.Default.Domain.Options
{
    public static class ApiOptions
    {
        private static IConfiguration _config;
        private static string _apiOptionsKey = "ApiOptions";
        private static string _defaultPageKey = "DefaultPage";
        private static string _defaultPageSizeKey = "DefaultPageSize";
        private static string _defaultSortByKey = "DefaultSortBy";

        public static int DefaultPage { get; private set; }
        public static int DefaultPageSize { get; private set; }
        public static string DefaultSortBy { get; private set; }

        public static void ConfigureApiOptions(IConfiguration configuration)
        {
            if(configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _config = configuration;

            DefaultPage = _config.GetValue<int>($"{_apiOptionsKey}:{_defaultPageKey}");
            DefaultPageSize = _config.GetValue<int>($"{_apiOptionsKey}:{_defaultPageSizeKey}");
            DefaultSortBy = _config.GetValue<string>($"{_apiOptionsKey}:{_defaultSortByKey}");
        }
    }
}
