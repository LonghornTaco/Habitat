using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Sitecore.Feature.CognitiveServices.Models.Repository
{
   [Serializable]
   public class Error
   {
      [JsonProperty("code")]
      public string Code { get; set; }
      [JsonProperty("message")]
      public string Message { get; set; }
   }
}