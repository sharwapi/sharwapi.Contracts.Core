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
        /// Performs advanced dependency validation or initialization logic based on loaded plugins.
        /// <para>Return <c>true</c> if the plugin can be loaded; otherwise, <c>false</c>.</para>
        /// <para>This is called after basic dependency checks pass.</para>
        /// </summary>
        /// <param name="loadedPluginVersions">A dictionary of plugins (name → version) that have passed the basic declarative dependency check (Stage 1). This is a subset of all loaded plugins.</param>
        /// <returns>Whether this plugin should effectively start.</returns>
        bool ValidateDependency(IReadOnlyDictionary<string, string> loadedPluginVersions) => true;

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
        /// <param name="configuration">The plugin-specific configuration, loaded from <c>config/{PluginName}.json</c>.</param>
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
        /// Gets the plugin-specific data directory path.
        /// <para>Defaults to <c>{BaseDir}/data/{Name}</c>.</para>
        /// <para>Override this property to customize the data directory location.</para>
        /// </summary>
        string DataDirectory => Path.Combine(AppContext.BaseDirectory, "data", Name);
        /// <summary>
        /// Gets the full path for a file within the plugin's data directory.
        /// <para>
        /// If <paramref name="relativePath"/> is a relative path, it is resolved against <see cref="DataDirectory"/>,
        /// resulting in <c>{DataDirectory}/{relativePath}</c>.
        /// </para>
        /// <para>
        /// If <paramref name="relativePath"/> is an absolute path, it is returned as-is,
        /// allowing plugins to store files at a custom location (e.g., a mounted HSM path)
        /// by setting an absolute path in their configuration file.
        /// </para>
        /// </summary>
        /// <param name="relativePath">The relative path within the plugin's data directory (e.g., <c>"keys/private.pem"</c>), or an absolute path to override the data directory entirely.</param>
        /// <returns>The resolved absolute path.</returns>
        string GetDataPath(string relativePath) => Path.Combine(DataDirectory, relativePath);
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
