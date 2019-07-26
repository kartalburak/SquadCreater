using RandomSquadCreater.Core;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RandomSquadCreater.UI.Infrastructure
{
    public class LoginAuthorize : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.Session["user"] == null)
            {
                httpContext.Response.Redirect("~/Home/Login");
            }
            else
            {
                Player player = HttpContext.Current.Session["user"] as Player;
                var roles = Roles.Split(',');

                if (player.PlayerIsAdmin)
                {
                    if (roles.Contains("Admin"))
                        return true;

                }
            }

            

            return base.AuthorizeCore(httpContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If they are authorized, handle accordingly
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                // Otherwise redirect to your specific authorized area
                filterContext.Result = new RedirectResult("~/Home/UnAuthorized");
            }
        }







    }
}