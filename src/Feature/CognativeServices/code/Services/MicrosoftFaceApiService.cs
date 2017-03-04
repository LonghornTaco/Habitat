using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.CognitiveServices.Services
{
   public class MicrosoftFaceApiService : IFaceApiService
   {
      public string CreatePerson()
      {
         throw new NotImplementedException();
      }

      public void AddPhotoToPerson(string personId, string base64ImageString)
      {
         throw new NotImplementedException();
      }

      public string VerifyPerson(string base64ImageString)
      {
         throw new NotImplementedException();
      }
   }
}