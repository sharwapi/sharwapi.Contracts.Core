using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sharwapi.Contracts.Core
{
    /// <summary>
    /// Defines the core interface for SharwAPI plugins.
    /// </summary>
    public interface IApiPlugin
    {
        /// <summary>
        /// Gets the unique identifier name of the plugin.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets the version of the plugin.
        /// </summary>
        string Version { get; }
        /// <summary>
        /// Gets the display name of the plugin.
        /// </summary>
        string DisplayName { get; }
        /// <summary>
        /// Gets the plugin dependencies.
        /// <para>Key: The unique identifier name of the dependent plugin.</para>
        /// <para>Value: The version requirement (e.g., "1.0.0", "[1.0, 2.0)", "*").</para>
        /// </summary>
        IReadOnlyDictionary<string, string> Dependencies => new Dictionary<string, string>();
        /// <summary>
        /// Gets a value indicating whether to enable automatic route prefixing.
        /// <para>Defaults to <c>false</c>, but enabling it is recommended.</para>
        /// </summary>
        bool UseAutoRoutePrefix 
        { 
            get => false;
        }
        /// <summary>
        /// Registers the plugin's services into the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The application configuration properties.</param>
        void RegisterServices(IServiceCollection services, IConfiguration configuration);
        /// <summary>
        /// Configures the HTTP request pipeline (middleware) for the plugin.
        /// </summary>
        /// <param name="app">The WebApplication instance.</param>
        void Configure(WebApplication app);
        /// <summary>
        /// Registers the plugin's business logic routes.
        /// </summary>
        /// <param name="app">The endpoint route builder.</param>
        /// <param name="configuration">The application configuration properties.</param>
        void RegisterRoutes(IEndpointRouteBuilder app, IConfiguration configuration);
        /// <summary>
        /// Gets the default configuration object provided by the plugin.
        /// </summary>
        object? DefaultConfig => null;
        /// <summary>
        /// Registers endpoints used for plugin management.
        /// <para><b>Note:</b> This is in the experimental prototyping stage and is not recommended for use.</para>
        /// </summary>
        /// <param name="managementGroup">The route group dedicated to management endpoints.</param>
        void RegisterManagementEndpoints(IEndpointRouteBuilder managementGroup)
        {
            managementGroup.MapGet("/", () =>
            Results.Ok(new
            {
                status = "Not Applicable",
                message = "This plugin does not have configurable management endpoints."
            })
        );
        }
    }
}
