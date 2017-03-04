using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Sitecore.Feature.CognitiveServices.Models.Repository
{
   [Serializable]
   public class VerifyResponse
   {
      [JsonProperty("isIdentical")]
      public bool IsIdentical { get; set; }
      [JsonProperty("confidence")]
      public double Confidence { get; set; }
      [JsonProperty("error")]
      public Error Error { get; set; }
   }
}