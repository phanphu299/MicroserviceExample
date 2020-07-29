
namespace CustomerApi.Misc.Extensions
{
    using CustomerApi.Message.Send.Sender;
    using CustomerApi.Repositories;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<ICustomerUpdateSender, CustomerUpdateSender>();
        }
    }
}
