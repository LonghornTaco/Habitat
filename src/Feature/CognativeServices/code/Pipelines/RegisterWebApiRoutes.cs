namespace Sitecore.Feature.CognitiveServices.Pipelines
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Sitecore.Pipelines;

    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Feature.CognitiveServices.Api", "api/cognitiveservices/{action}", new
            {
                controller = "CognitiveServices"
            });
        }
    }
}