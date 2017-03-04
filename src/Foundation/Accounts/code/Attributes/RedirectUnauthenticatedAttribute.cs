namespace Sitecore.Foundation.Accounts.Attributes
{
    using System.Web.Mvc;
    using Sitecore.Foundation.Accounts.Services;

    public class RedirectUnauthenticatedAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        private readonly IGetRedirectUrlService getRedirectUrlService;

        public RedirectUnauthenticatedAttribute() : this(new GetRedirectUrlService())
        {
        }

        private RedirectUnauthenticatedAttribute(IGetRedirectUrlService getRedirectUrlService)
        {
            this.getRedirectUrlService = getRedirectUrlService;
        }

        public void OnAuthorization(AuthorizationContext context)
        {
            if (Context.User.IsAuthenticated)
                return;
            var link = this.getRedirectUrlService.GetRedirectUrl(AuthenticationStatus.Unauthenticated, context.HttpContext.Request.RawUrl);
            context.Result = new RedirectResult(link);
        }
    }
}