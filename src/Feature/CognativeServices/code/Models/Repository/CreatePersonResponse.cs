using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Sitecore.Feature.CognitiveServices.Models.Repository
{
   [Serializable]
   public class CreatePersonResponse
   {
      [JsonProperty("personId")]
      public string PersonId { get; set; }
   }
}