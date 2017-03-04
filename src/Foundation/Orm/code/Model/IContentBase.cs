namespace Sitecore.Foundation.Orm.Model
{
    using System;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;

    public interface IContentBase
    {
        Guid Id { get; }
        [SitecoreInfo(SitecoreInfoType.Name)]
        string Name { get; }
        string DisplayName { get; }
        string Path { get; }
        string Url { get; set; }
        string FullPath { get; }
        string TemplateName { get; }
        Guid TemplateId { get; }
        int Sortorder { get; }
    }
}
