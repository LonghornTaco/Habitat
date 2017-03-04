namespace Sitecore.Foundation.Orm.Maps
{
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Maps;
    using Sitecore.Foundation.Orm.Model;

    public class ContentBaseMap : SitecoreGlassMap<IContentBase> 
    {
        public override void Configure()
        {
            Map(config =>
            {
                config.AutoMap();
                config.Id(m => m.Id);
                config.Info(m => m.Name).InfoType(SitecoreInfoType.Name);
                config.Info(m => m.DisplayName).InfoType(SitecoreInfoType.DisplayName);
                config.Info(m => m.Path).InfoType(SitecoreInfoType.Path);
                config.Info(m => m.Url).InfoType(SitecoreInfoType.Url);
                config.Info(m => m.FullPath).InfoType(SitecoreInfoType.FullPath);
                config.Info(m => m.TemplateName).InfoType(SitecoreInfoType.TemplateName);
                config.Info(m => m.TemplateId).InfoType(SitecoreInfoType.TemplateId);
                config.Field(f => f.Sortorder).FieldName("__Sortorder");
            });
        }
    }
}