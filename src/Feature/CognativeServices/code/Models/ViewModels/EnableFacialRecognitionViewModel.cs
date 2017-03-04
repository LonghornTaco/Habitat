namespace Sitecore.Feature.CognitiveServices.Models.ViewModels
{
    using System.Web;

    public class EnableFacialRecognitionViewModel
    {
        public IHtmlString InformationText { get; set; }
        public IHtmlString TitleText { get; set; }
        public IHtmlString SaveButtonText { get; set; }
        public IHtmlString CancelButtonText { get; set; }
        public IHtmlString EnableFacialRecognitionLabel { get; set; }
        public IHtmlString WebcamLabel { get; set; }
        public IHtmlString WebcamAccessWarningLabel { get; set; }

        public string EnableFacialRecognitionPlaceholderText { get; set; }

        public bool EnableFacialRecognition { get; set; }
        public HttpPostedFile FaceImageFile { get; set; }
        public string CapturedImage { get; set; }
    }
}