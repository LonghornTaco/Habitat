using System.Web.Mvc;

namespace Sitecore.Feature.CognitiveServices.Controllers
{
    using System;
    using Glass.Mapper.Sc;
    using Sitecore.Analytics;
    using Sitecore.Feature.CognitiveServices.Facets;
    using Sitecore.Feature.CognitiveServices.Models;
    using Sitecore.Feature.CognitiveServices.Models.ViewModels;
    using Sitecore.Foundation.Orm;
    using Sitecore.Foundation.Orm.Model;

    public class CognitiveServicesController : Controller
    {
        private readonly ISitecoreContext sitecoreContext;
        private readonly IContextWrapper contextWrapper;
        private readonly IPropertyBuilder propertyBuilder;

        public CognitiveServicesController(ISitecoreContext sitecoreContext, IContextWrapper contextWrapper, IPropertyBuilder propertyBuilder)
        {
            this.sitecoreContext = sitecoreContext;
            this.contextWrapper = contextWrapper;
            this.propertyBuilder = propertyBuilder;
        }

        public ActionResult EnableFacialRecognition()
        {
            if (string.IsNullOrWhiteSpace(this.contextWrapper.DataSource))
            {
                return this.View();
            }

            var viewModel = new EnableFacialRecognitionViewModel();
            var dataSourceItem = this.sitecoreContext.GetItem<IEnableFacialRecognition>(Guid.Parse(this.contextWrapper.DataSource));

            if (dataSourceItem == null)
            {
                return this.View(viewModel);
            }

            viewModel.EnableFacialRecognitionLabel = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.EnableFacialRecognitionLabel);
            viewModel.InformationText = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.InformationText);
            viewModel.SaveButtonText = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.SaveButtonText);
            viewModel.TitleText = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.TitleText);
            viewModel.WebCamLabel = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.WebCamLabel);
            viewModel.EnableFacialRecognitionPlaceholderText = dataSourceItem.EnableFacialRecognitionPlaceholderText;

            return this.View(viewModel);
        }

        public ActionResult LoginWithFacialRecognition()
        {
            return this.View();
        }

        public JsonResult SetPersonImage()
        {
            if (!Tracker.Enabled)
            {
                return this.Json(new {Errors = "Analytics is disabled"}, JsonRequestBehavior.AllowGet);
            }

            var contact = Tracker.Current.Contact;
            var data = contact.GetFacet<IContactSitecoreHello>("SitecoreHello");
            data.PersonId = "";


            return this.Json(new { Data = "PersonId = " + data.PersonId }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerifyPerson()
        {
            return null;
        }
    }
}