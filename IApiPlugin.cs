using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sharwapi.Contracts.Core
{
    public interface IApiPlugin
    {
        //定义插件名称
        string Name { get; }
        //定义插件版本
        string Version { get; }
        //定义插件显示名称
        string DisplayName { get; }
        //定义插件注册服务方法
        void RegisterServices(IServiceCollection services, IConfiguration configuration);
        //定义插件中间件配置方法
        void Configure(WebApplication app);
        //定义插件路由注册方法
        void RegisterRoutes(IEndpointRouteBuilder app, IConfiguration configuration);
        //定义插件用于被管理的端点注册方法
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
