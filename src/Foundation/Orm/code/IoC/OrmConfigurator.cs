namespace Sitecore.Foundation.Orm.IoC
{
    using Glass.Mapper.Sc;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using Sitecore.Foundation.Orm.Model;

    public class OrmConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IContextWrapper, SitecoreContextWrapper>();
            serviceCollection.AddSingleton<IPropertyBuilder, PropertyBuilder>();
            serviceCollection.AddTransient<IGlassHtml, GlassHtml>();
            serviceCollection.AddTransient<ISitecoreContext, SitecoreContext>();
        }
    }
}