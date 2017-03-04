namespace Sitecore.Feature.CognitiveServices.IoC
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using Sitecore.Feature.CognitiveServices.Controllers;

    public class CognitiveServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<CognitiveServicesController>();
        }
    }
}