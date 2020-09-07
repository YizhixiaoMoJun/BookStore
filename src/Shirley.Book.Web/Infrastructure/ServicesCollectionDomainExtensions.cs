using Microsoft.Extensions.DependencyInjection;
using Shirley.Book.Service.AuthServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Shirley.Book.Web.Infrastructure
{
    /// <summary>
    /// domain services extensions
    /// </summary>
    public static class ServicesCollectionDomainExtensions
    {
        /// <summary>
        /// register domain services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<HttpGlobalExceptionHandler>();
            return services;
        }

        public static IServiceCollection AddDomainServicesByConversion(this IServiceCollection services, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var exportedServices = assembly.ExportedTypes
                    .Where(t => t.Name.EndsWith("Service") && !t.IsInterface && !t.IsAbstract)
                    .Select(t => (t, t.GetInterfaces().Where(i => i.Name.EndsWith("Service"))));

                foreach (var item in exportedServices)
                {
                    foreach (var service in item.Item2)
                    {
                        services.AddTransient(service, item.Item1);
                    }
                }
            }

            return services;
        }
    }
}
