using System.Web.Mvc;

namespace Sitecore.Feature.CognitiveServices.Controllers
{
    public class CognitiveServicesController : Controller
    {
        public ActionResult EnableFacialRecognition()
        {
            return View();
        }

        public ActionResult LoginWithFacialRecognition()
        {
            return View();
        }

        public JsonResult CreatePerson()
        {
            return null;
        }

        public JsonResult VerifyPerson()
        {
            return null;
        }
    }
}