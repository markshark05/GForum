using System.Web.Mvc;

namespace GForum.Web.Helpers
{
    public class AjaxAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                var urlHelper = new UrlHelper(context.RequestContext);
                context.Result = new JsonResult
                {
                    Data = new
                    {
                        success = false,
                        error = "You msut be authorized to perform this action."
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(context);
            }
        }
    }
}