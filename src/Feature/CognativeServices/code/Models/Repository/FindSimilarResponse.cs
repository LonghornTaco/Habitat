using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Sitecore.Feature.CognitiveServices.Models.Repository
{
   [Serializable]
   public class FindSimilarResponse
   {
      [JsonProperty("persistedFaceId")]
      public Guid FaceId { get; set; }
      [JsonProperty("confidence")]
      public double Confidence { get; set; }
   }
}