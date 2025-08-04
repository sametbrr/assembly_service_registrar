using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AssemblyServiceRegistrar
{
    public static class AssemblyServiceRegistrar
    {
        public static IServiceCollection AddServicesFromAssembly(this IServiceCollection services, Assembly assembly, ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));
            if (assembly is null) throw new ArgumentNullException(nameof(assembly));

            var types = assembly.GetTypes();

            var servicePairs = types
                .Where(t => t.IsClass && !t.IsAbstract)
                .SelectMany(impl => impl.GetInterfaces()
                    .Where(i => typeof(IService).IsAssignableFrom(i) && i != typeof(IService)),
                    (impl, iface) => new { Interface = iface, Implementation = impl });

            foreach (var pair in servicePairs)
            {
                var lifetime = ResolveLifetime(pair.Implementation, defaultLifetime);
                services.Add(new ServiceDescriptor(pair.Interface, pair.Implementation, lifetime));
            }

            return services;
        }

        private static ServiceLifetime ResolveLifetime(Type impl, ServiceLifetime defaultLifetime)
        {
            if (typeof(ISingletonService).IsAssignableFrom(impl)) return ServiceLifetime.Singleton;
            if (typeof(IScopedService).IsAssignableFrom(impl)) return ServiceLifetime.Scoped;
            if (typeof(ITransientService).IsAssignableFrom(impl)) return ServiceLifetime.Transient;
            return defaultLifetime;
        }
    }
}
