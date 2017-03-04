namespace Sitecore.Feature.CognitiveServices.Services
{
    public interface IFaceApiService
    {
        string CreatePerson();
        void AddPhotoToPerson();
        string VerifyPerson();
    }
}
