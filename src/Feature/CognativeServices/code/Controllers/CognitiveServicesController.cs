using System.Web.Mvc;

namespace Sitecore.Feature.CognitiveServices.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Glass.Mapper.Sc;
    using Sitecore.Analytics;
    using Sitecore.Feature.CognitiveServices.Facets;
    using Sitecore.Feature.CognitiveServices.Models;
    using Sitecore.Feature.CognitiveServices.Models.ViewModels;
    using Sitecore.Feature.CognitiveServices.Services;
    using Sitecore.Foundation.Orm;
    using Sitecore.Foundation.Orm.Model;

    public class CognitiveServicesController : Controller
    {
        private readonly ISitecoreContext sitecoreContext;
        private readonly IContextWrapper contextWrapper;
        private readonly IPropertyBuilder propertyBuilder;
        private readonly IFaceApiService faceApiService;

        public CognitiveServicesController(ISitecoreContext sitecoreContext, IContextWrapper contextWrapper, IPropertyBuilder propertyBuilder, IFaceApiService faceApiService)
        {
            this.sitecoreContext = sitecoreContext;
            this.contextWrapper = contextWrapper;
            this.propertyBuilder = propertyBuilder;
            this.faceApiService = faceApiService;
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
                return this.View();
            }

            viewModel.EnableFacialRecognitionLabel = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.EnableFacialRecognitionLabel);
            viewModel.InformationText = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.InformationText);
            viewModel.SaveButtonText = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.SaveButtonText);
            viewModel.TitleText = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.TitleText);
            viewModel.WebcamLabel = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.WebcamLabel);
            viewModel.EnableFacialRecognitionPlaceholderText = dataSourceItem.EnableFacialRecognitionPlaceholderText;
            viewModel.WebcamAccessWarningLabel = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.WebcamAccessWarning);

            return this.View(viewModel);
        }

        public ActionResult LoginWithFacialRecognition()
        {
            if (string.IsNullOrWhiteSpace(this.contextWrapper.DataSource))
            {
                return this.View();
            }

            var viewModel = new LoginWithSitecoreHelloViewModel();
            var dataSourceItem = this.sitecoreContext.GetItem<ILoginWithSitecoreHello>(Guid.Parse(this.contextWrapper.DataSource));

            if (dataSourceItem == null)
            {
                return this.View();
            }

            viewModel.TitleText = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.TitleText);
            viewModel.InformationText = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.InformationText);
            viewModel.SearchingText = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.SearchingText);
            viewModel.LoginButtonText = this.propertyBuilder.BuildHtmlString(dataSourceItem, x => x.LoginButtonText);
            viewModel.UseStandardLoginUrl = dataSourceItem.UseStandardLoginUrl?.Url ?? string.Empty;

            return this.View(viewModel);
        }

        public JsonResult SetPersonImage(EnableFacialRecognitionViewModel model)
        {
            if (model == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult
                {
                    Data = new { success = false, errors = new List<string> { "The model was null" } },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            // TODO: add parameters in here
            var personId = this.faceApiService.CreatePerson();
            this.faceApiService.AddPhotoToPerson(personId, model.CapturedImage);

            if (!Tracker.Enabled)
            {
                return this.Json(new {Errors = "Analytics is disabled"}, JsonRequestBehavior.AllowGet);
            }

            var contact = Tracker.Current.Contact;
            var data = contact.GetFacet<IContactSitecoreHello>("SitecoreHello");
            data.PersonId = personId;

            return this.Json(new { Data = "PersonId = " + data.PersonId }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerifyPerson()
        {
            return null;
        }
    }
}