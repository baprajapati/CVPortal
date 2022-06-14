using System.Web.Mvc;

namespace CVPortal.Areas.InitiatorAdmin
{
    public class InitiatorAdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "InitiatorAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "InitiatorAdmin_default",
                "InitiatorAdmin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}