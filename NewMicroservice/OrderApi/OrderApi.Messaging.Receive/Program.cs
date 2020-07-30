
namespace OrderApi.Messaging.Receive
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using OrderApi.Infrastructure;
    using OrderApi.Messaging.Receive.Receiver;
    using OrderApi.Repositories;
    using System;
    using System.Threading.Tasks;

    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();
                Console.WriteLine("Starting host...");
                await host.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Host terminated unexpectedly : " + ex.Message);
                return 1;
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {

            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<OrderContext>(options =>
                    {
                        options.UseSqlServer("Server=PC628\\SQLEXPRESS;Database=MicroserviceOrderDemo;Trusted_Connection=True;MultipleActiveResultSets=true");
                    });
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddHostedService<CustomerFullNameUpdateReceiver>();
                })
                .UseDefaultServiceProvider((context, options) => options.ValidateScopes = false);
        }
    }
}
