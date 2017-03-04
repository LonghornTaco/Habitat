namespace Sitecore.Feature.CognitiveServices.Services
{
   using System;
   using Microsoft.ProjectOxford.Face.Contract;

   public interface IFaceApiService
   {
      Face[] Detect(string base64Image);
      [Obsolete]
      string CreatePerson();
      string CreatePerson(string name, string base64Image);
      [Obsolete]
      void AddPhotoToPerson(string personId, string base64ImageString);
      string VerifyPerson(string base64ImageString);
   }
}
