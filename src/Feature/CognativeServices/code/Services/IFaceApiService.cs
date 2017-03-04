namespace Sitecore.Feature.CognitiveServices.Services
{
    public interface IFaceApiService
    {
        string CreatePerson();
        void AddPhotoToPerson(string personId, string base64ImageString);
        string VerifyPerson(string base64ImageString);
    }
}
