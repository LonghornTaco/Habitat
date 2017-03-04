namespace Sitecore.Feature.CognitiveServices.Models
{
    using Sitecore.Foundation.Orm.Model;

    public interface IEnableFacialRecognition : IContentBase
    {
        string InformationText { get; set; }

        string TitleText { get; set; }

        string SaveButtonText { get; set; }

        string CancelButtonText { get; set; }

        string EnableFacialRecognitionLabel { get; set; }

        string WebCamLabel { get; set; }

        string EnableFacialRecognitionPlaceholderText { get; set; }
    }
}