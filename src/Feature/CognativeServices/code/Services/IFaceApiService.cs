namespace Sitecore.Feature.CognitiveServices.Services
{
   using System;
   using Microsoft.ProjectOxford.Face.Contract;

   public interface IFaceApiService
   {
      Face[] Detect(string base64Image);
      string CreatePerson(string name, string base64Image);
      string VerifyPerson(string base64ImageString);
   }
}
