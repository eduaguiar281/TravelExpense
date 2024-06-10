//Add Comment
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TravelExpense.Core.Communications;

namespace TravelExpense.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddAplication(this IServiceCollection service)
        {
            service.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            service.AddScoped<IMediatorHandler, MediatorHandler>();
            return service;
        }
    }
}
