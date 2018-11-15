using RCSEntities;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RCSSerivce
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,
     Inherited = true, AllowMultiple = true)]
    public class RBACAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            /*Create permission string based on the requested controller 
              name and action name in the format 'controllername-action'*/
            string requiredPermission = String.Format("{0}-{1}",
                   filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                   filterContext.ActionDescriptor.ActionName);
            if (Convert.ToInt32(HttpContext.Current.Session["RoleId"]) == 0)
            {
                RedirectToRouteResult redirectToRouteResult = new RedirectToRouteResult(
                                              new RouteValueDictionary {
                                                { "action", "login" },
                                                { "controller", "account" } });
                filterContext.Result = redirectToRouteResult;
            }
            else
            {
                string role = Convert.ToString(HttpContext.Current.Session["RoleId"]);
                RBACUser requestingUser = new RBACUser(role);
                //Check if the requesting user has the permission to run the controller's action
                if (!requestingUser.HasPermission(requiredPermission, role))
                {
                    /*User doesn't have the required permission and is not a SysAdmin, return our 
                      custom '401 Unauthorized' access error. Since we are setting 
                      filterContext.Result to contain an ActionResult page, the controller's 
                      action will not be run.
                      The custom '401 Unauthorized' access error will be returned to the 
                      browser in response to the initial request.*/
                    RouteValueDictionary routeValues = new RouteValueDictionary {
                                                { "action", "Index" },
                                                { "controller", "Unauthorised" } };
                    filterContext.Result = new RedirectToRouteResult(
                                                   routeValues);
                }

            }
            /*If the user has the permission to run the controller's action, then 
              filterContext.Result will be uninitialized and executing the controller's 
              action is dependant on whether filterContext.Result is uninitialized.*/
        }
    }

    public class AntiForgeryHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is HttpAntiForgeryException)
            {
                var url = string.Empty;
                if (!context.HttpContext.User.Identity.IsAuthenticated)
                {
                    var requestContext = new RequestContext(context.HttpContext, context.RouteData);
                    url = RouteTable.Routes.GetVirtualPath(requestContext, new RouteValueDictionary(new { Controller = "User", action = "Login" })).VirtualPath;
                }
                else
                {
                    context.HttpContext.Response.StatusCode = 200;
                    context.ExceptionHandled = true;
                    url = GetRedirectUrl(context);
                }
                context.HttpContext.Response.Redirect(url, true);
            }
            else
            {
                base.OnException(context);
            }
        }

        private string GetRedirectUrl(ExceptionContext context)
        {
            try
            {
                var requestContext = new RequestContext(context.HttpContext, context.RouteData);
                var url = RouteTable.Routes.GetVirtualPath(requestContext, new RouteValueDictionary(new { Controller = "User", action = "AlreadySignIn" })).VirtualPath;

                return url;
            }
            catch (Exception)
            {
                throw new NullReferenceException();
            }
        }
    }

    public class CS4HJ
    {
        public void CreatSession()
        {
            string str = "";
            Random ran = new Random();
            str = Convert.ToString(ran.Next(9999, 999999));
            HttpContext.Current.Session["Var"] = str.GetHashCode().ToString();
            HttpContext.Current.Session["CSRF"] = str.GetHashCode().ToString();
        }

        public int CheckSessionEveryCall()
        {
            if (HttpContext.Current.Session["Var"] == null || HttpContext.Current.Session["CSRF"] == null)
            {
                return 1;
            }
            // Compare session variables
            if (HttpContext.Current.Session["Var"] != HttpContext.Current.Session["CSRF"])
            {
                return 2;
            }
            HttpContext.Current.Session["Var"] = "";
            HttpContext.Current.Session["CSRF"] = "";
            return 0;
        }
    }
}
