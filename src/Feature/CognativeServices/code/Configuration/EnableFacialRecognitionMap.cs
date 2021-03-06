﻿namespace Sitecore.Feature.CognitiveServices.Configuration
{
    using Glass.Mapper.Sc.Maps;
    using Sitecore.Feature.CognitiveServices.Models;
    using Sitecore.Foundation.Orm.Model;

    public class EnableFacialRecognitionMap : SitecoreGlassMap<IEnableFacialRecognition>
    {
        public override void Configure()
        {
            this.Map(config =>
            {
                this.ImportMap<IContentBase>();
                config.AutoMap();
            });
        }
    }
}