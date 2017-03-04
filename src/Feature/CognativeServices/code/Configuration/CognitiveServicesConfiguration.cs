using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Configuration;

namespace Sitecore.Feature.CognitiveServices.Configuration
{
   public class CognitiveServicesConfiguration : ICognitiveServicesConfiguration
   {
      public string ApiRoot => Settings.GetSetting("CognitiveServices.ApiRoot");
      public string ApiKey => Settings.GetSetting("CognitiveServices.ApiKey");
      public string DetectApi => Settings.GetSetting("CognitiveServices.DetectApi");
      public string PersonApi => Settings.GetSetting("CognitiveServices.PersonApi");
      public string PersonGroupApi => Settings.GetSetting("CognitiveServices.PersonGroupApi");
      public string IdentifyApi => Settings.GetSetting("CognitiveServices.IdentifyApi");
      public string PersonGroupId => Settings.GetSetting("CognitiveServices.PersonGroupId");
   }
}