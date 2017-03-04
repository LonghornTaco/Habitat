using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.CognitiveServices.Models.Repository
{
   using Microsoft.ProjectOxford.Face.Contract;
   using Newtonsoft.Json;

   [Serializable]
   public class IdentifyResponse
   {
      [JsonProperty("faceId")]
      public string FaceId { get; set; }

      [JsonProperty("candidates")]
      public List<Candidate> Candidates { get; set; }
   }
}