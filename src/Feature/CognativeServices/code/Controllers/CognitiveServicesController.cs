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
   using Sitecore.Foundation.Accounts.Attributes;
   using Sitecore.Foundation.Orm;
   using Sitecore.Foundation.Orm.Model;

   public class CognitiveServicesController : Controller
   {
      private readonly ISitecoreContext _sitecoreContext;
      private readonly IContextWrapper _contextWrapper;
      private readonly IPropertyBuilder _propertyBuilder;
      private readonly IFaceApiService _faceApiService;

      public CognitiveServicesController(ISitecoreContext sitecoreContext, IContextWrapper contextWrapper, IPropertyBuilder propertyBuilder, IFaceApiService faceApiService)
      {
         _sitecoreContext = sitecoreContext;
         _contextWrapper = contextWrapper;
         _propertyBuilder = propertyBuilder;
         _faceApiService = faceApiService;
      }

      [RedirectUnauthenticated]
      public ActionResult EnableFacialRecognition()
      {
         if (string.IsNullOrWhiteSpace(_contextWrapper.DataSource))
         {
            return View();
         }

         var viewModel = new EnableFacialRecognitionViewModel();
         var dataSourceItem = _sitecoreContext.GetItem<IEnableFacialRecognition>(Guid.Parse(_contextWrapper.DataSource));

         if (dataSourceItem == null)
         {
            return View();
         }

         viewModel.EnableFacialRecognitionLabel = _propertyBuilder.BuildHtmlString(dataSourceItem, x => x.EnableFacialRecognitionLabel);
         viewModel.InformationText = _propertyBuilder.BuildHtmlString(dataSourceItem, x => x.InformationText);
         viewModel.SaveButtonText = _propertyBuilder.BuildHtmlString(dataSourceItem, x => x.SaveButtonText);
         viewModel.TitleText = _propertyBuilder.BuildHtmlString(dataSourceItem, x => x.TitleText);
         viewModel.WebcamLabel = _propertyBuilder.BuildHtmlString(dataSourceItem, x => x.WebcamLabel);
         viewModel.EnableFacialRecognitionPlaceholderText = dataSourceItem.EnableFacialRecognitionPlaceholderText;
         viewModel.WebcamAccessWarningLabel = _propertyBuilder.BuildHtmlString(dataSourceItem, x => x.WebcamAccessWarning);

         return View(viewModel);
      }

      [RedirectAuthenticated]
      public ActionResult LoginWithFacialRecognition()
      {
         if (string.IsNullOrWhiteSpace(_contextWrapper.DataSource))
         {
            return View();
         }

         var viewModel = new LoginWithSitecoreHelloViewModel();
         var dataSourceItem = _sitecoreContext.GetItem<ILoginWithSitecoreHello>(Guid.Parse(_contextWrapper.DataSource));

         if (dataSourceItem == null)
         {
            return View();
         }

         viewModel.TitleText = _propertyBuilder.BuildHtmlString(dataSourceItem, x => x.TitleText);
         viewModel.InformationText = _propertyBuilder.BuildHtmlString(dataSourceItem, x => x.InformationText);
         viewModel.SearchingText = _propertyBuilder.BuildHtmlString(dataSourceItem, x => x.SearchingText);
         viewModel.LoginButtonText = dataSourceItem.UseStandardLoginLink?.Text ?? string.Empty;
         viewModel.UseStandardLoginUrl = dataSourceItem.UseStandardLoginLink?.Url ?? string.Empty;
         viewModel.CreateAccountText = dataSourceItem.CreateAccountLink?.Text ?? string.Empty;
         viewModel.CreateAccountUrl = dataSourceItem.CreateAccountLink?.Url ?? string.Empty;

         return View(viewModel);
      }

      public ActionResult LoginWithHelloMenu()
      {
         return View();
      }

      [HttpPost]
      public JsonResult SetPersonImage(EnableFacialRecognitionViewModel model)
      {
         if (!Context.IsLoggedIn)
         {
            return BadRequest(HttpStatusCode.Forbidden, "User not logged in");
         }

         if (model == null)
         {
            return BadRequest(HttpStatusCode.BadRequest, "The model was null");
         }

         if (!Tracker.Enabled)
         {
            return Json(new { Errors = "Analytics is disabled" }, JsonRequestBehavior.AllowGet);
         }

         var capturedImage = model.CapturedImage.Replace("data:image/jpeg;base64,", "");
         var personId = _faceApiService.CreatePerson(Context.User.LocalName, capturedImage);

         var contact = Tracker.Current.Contact;
         var data = contact.GetFacet<IContactSitecoreHello>("SitecoreHello");
         data.PersonId = personId;

         return Json(new { Data = "PersonId = " + data.PersonId }, JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      public JsonResult LoginWithHello(LoginWithSitecoreHelloViewModel model)
      {
         if (Context.IsLoggedIn)
         {
            return BadRequest(HttpStatusCode.OK, "User not logged in");
         }

         if (model == null)
         {
            return BadRequest(HttpStatusCode.BadRequest, "The model was null");
         }

         var personId = _faceApiService.VerifyPerson(model.CapturedImage);
         if (string.IsNullOrWhiteSpace(personId))
         {
            return BadRequest(HttpStatusCode.Forbidden, "Face not found");
         }

         return Json(new { Data = "PersonId = " + personId }, JsonRequestBehavior.AllowGet);
      }

      private JsonResult BadRequest(HttpStatusCode statusCode, string errorMessage)
      {
         Response.StatusCode = (int)statusCode;
         return new JsonResult
         {
            Data = new { success = false, errors = new List<string> { errorMessage } },
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
         };

      }
   }
}