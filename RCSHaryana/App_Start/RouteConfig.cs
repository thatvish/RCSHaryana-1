using System.Web.Mvc;
using System.Web.Routing;

namespace RCSHaryana
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.RouteExistingFiles = true;

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "js", // Route name  
               url: "Content/assets/js/libs",
               defaults: new { controller = "Account", action = "Login", file = UrlParameter.Optional }//,// Parameter defaults  
           );

            routes.MapRoute(
               name: "js1", // Route name  
               url: "Content/assets/js/core",
               defaults: new { controller = "Account", action = "Login", file = UrlParameter.Optional }//,// Parameter defaults  
           );

            routes.MapRoute(
             name: "js2", // Route name  
             url: "Content/~/",
             defaults: new { controller = "Account", action = "Login", file = UrlParameter.Optional }//,// Parameter defaults  
         );
        }
    }
}
