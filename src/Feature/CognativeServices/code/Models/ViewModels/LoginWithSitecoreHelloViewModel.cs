namespace Sitecore.Feature.CognitiveServices.Models.ViewModels
{
    using System.Web;

    public class LoginWithSitecoreHelloViewModel
    {
        public IHtmlString TitleText { get; set; }
        public IHtmlString InformationText { get; set; }
        public IHtmlString SearchingText { get; set; }
        public IHtmlString LoginButtonText { get; set; }
        public string UseStandardLoginUrl { get; set; }
    }
}