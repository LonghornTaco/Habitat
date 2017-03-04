namespace Sitecore.Feature.CognitiveServices.Models.ViewModels
{
    using System.Web;

    public class LoginWithSitecoreHelloViewModel
    {
        public IHtmlString TitleText { get; set; }
        public IHtmlString InformationText { get; set; }
        public IHtmlString SearchingText { get; set; }
        public string LoginButtonText { get; set; }
        public string UseStandardLoginUrl { get; set; }
        public string CreateAccountText { get; set; }
        public string CreateAccountUrl { get; set; }
        public IHtmlString WebcamAccessWarningLabel { get; set; }
        public IHtmlString LoginFailedLabel { get; set; }
    }
}