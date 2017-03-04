using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.CognitiveServices.Configuration
{
   public interface ICognitiveServicesConfiguration
   {
      string ApiRoot { get; }
      string ApiKey { get; }
      string DetectApi { get; }
      string PersonApi { get; }
      string PersonGroupApi { get; }
      string IdentifyApi { get; }
      string PersonGroupId { get; }
   }
}