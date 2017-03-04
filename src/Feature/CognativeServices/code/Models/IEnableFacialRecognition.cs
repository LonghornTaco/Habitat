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

        string WebcamLabel { get; set; }

        string EnableFacialRecognitionPlaceholderText { get; set; }

        string WebcamAccessWarning { get; set; }

        string SaveErrorText { get; set; }

        string SaveSuccessText { get; set; }
    }
}