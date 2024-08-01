using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure
{
    public static class MediatRConfiguration
    {
        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            // Register MediatR services from all assemblies in the current domain
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            //otra opcion pero tendria que registrar todos
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).GetTypeInfo().Assembly));


            return services;
        }
    }
}
