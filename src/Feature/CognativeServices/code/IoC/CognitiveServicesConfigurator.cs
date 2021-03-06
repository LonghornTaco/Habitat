﻿namespace Sitecore.Feature.CognitiveServices.IoC
{
   using Microsoft.Extensions.DependencyInjection;
   using Sitecore.DependencyInjection;
   using Sitecore.Feature.CognitiveServices.Configuration;
   using Sitecore.Feature.CognitiveServices.Controllers;
   using Sitecore.Feature.CognitiveServices.Services;

   public class CognitiveServicesConfigurator : IServicesConfigurator
   {
      public void Configure(IServiceCollection serviceCollection)
      {
         serviceCollection.AddTransient<CognitiveServicesController>();
         serviceCollection.AddTransient<ICognitiveServicesConfiguration, CognitiveServicesConfiguration>();
         serviceCollection.AddTransient<IFaceApiService, MicrosoftFaceApiService>();
      }
   }
}