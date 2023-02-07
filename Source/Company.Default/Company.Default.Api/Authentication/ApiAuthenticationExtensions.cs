using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiAuthenticationExtensions
    {
        /// <summary>
        /// Use this settings if you want basic Azure AD protection for you API
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddAuthenticationAzureDefault(this IServiceCollection services, IConfiguration configuration) =>
            services.AddMicrosoftIdentityWebApiAuthentication(configuration);

        /// <summary>
        /// Use this setting only if you want to secure your API with allowed clients.
        /// It is necessary to add in the project settings the **AllowedClients** key with the application registration ids separated by semicolons.
        /// For more information, visit:
        /// <see href="https://docs.microsoft.com/azure/active-directory/develop/access-tokens#validate-the-user-has-permission-to-access-this-data">
        /// Microsoft identity platform access tokens
        /// </see>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public static void AddAuthenticationAzureAdWithValidClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(options =>
            {
                configuration.Bind("AzureAd", options);
                options.Events = new JwtBearerEvents();
                options.Events.OnTokenValidated = async context =>
                {
                    string[] allowedClientApps = configuration.GetValue<string>("AllowedClients").Split(";");

                    string clientappId = context?.Principal?.Claims.FirstOrDefault(x => x.Type == "azp" || x.Type == "appid")?.Value;

                    if (!allowedClientApps.Contains(clientappId))
                        throw new UnauthorizedAccessException("The client app is not permitted to access this API");

                    await Task.CompletedTask;
                };

            }, options =>
            {
                configuration.Bind("AzureAd", options);
            });
        }

        /// <summary>
        /// Use this setting if your API use Microsoft Graph on
        /// <see href="https://learn.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-on-behalf-of-flow">On-Behalf-Of flow</see>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddAuthenticationAzureWithMicrosoftGraph(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddMicrosoftIdentityWebApi(configuration, "AzureAd")
                 .EnableTokenAcquisitionToCallDownstreamApi()
                 .AddMicrosoftGraph(configuration.GetSection("Graph"))
                 .AddInMemoryTokenCaches();
        }

        /// <summary>
        /// Use this setting if your API calls another API. Click 
        /// <see href="https://learn.microsoft.com/en-us/azure/active-directory/develop/scenario-web-api-call-api-app-configuration?tabs=aspnetcore#option-2-call-a-downstream-web-api-other-than-microsoft-graph">
        /// here
        /// </see>
        /// for more information
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="apiName"></param>
        /// <param name="graphSectionKey"></param>
        public static void AddAuthenticationAzureWithDownstreamApi(this IServiceCollection services, IConfiguration configuration, string apiName, string graphSectionKey)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration, "AzureAd")
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddDownstreamWebApi(apiName, configuration.GetSection(graphSectionKey))
            .AddInMemoryTokenCaches();
        }
        
    }
}
