namespace Sitecore.Foundation.Orm
{
    using Sitecore.Mvc.Presentation;

    public class SitecoreContextWrapper : IContextWrapper
    {
        public string DataSource => RenderingContext.Current.Rendering.DataSource;
    }
}