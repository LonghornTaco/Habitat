namespace Sitecore.Feature.CognitiveServices.Models
{
    using Glass.Mapper.Sc.Fields;
    using Sitecore.Foundation.Orm.Model;

    public interface ILoginWithSitecoreHello : IContentBase
    {
        string TitleText { get; set; }
        string InformationText { get; set; }
        string SearchingText { get; set; }
        string LoginButtonText { get; set; }
        Link UseStandardLoginUrl { get; set; }
    }
}
