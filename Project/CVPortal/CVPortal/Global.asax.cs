using CVPortal.App_Code;
using CVPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;

namespace CVPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebSecurity.InitializeDatabaseConnection("MembershipConnection", "tbl_Users", "Id", "EmailAddress", autoCreateTables: true);

            using (CVPortalEntities dataContext = new CVPortalEntities())
            {
                #region Setup Roles and Users

                var roles = dataContext.webpages_Roles.ToList();
                if (!roles.Any(x => x.RoleName == "Admin"))
                {
                    dataContext.webpages_Roles.Add(new webpages_Roles()
                    {
                        RoleName = "Admin"
                    });
                    dataContext.SaveChanges();

                    var adminRole = dataContext.webpages_Roles.First(x => x.RoleName == "Admin");
                    WebSecurity.CreateUserAndAccount("admin@gmail.com", Utility.DefaultPassword, new
                    {
                        HAUSER = "1000",
                        HANAME = "test",
                        Dept_Code = "test"
                    });

                    Roles.AddUserToRole("admin@gmail.com", "Admin");
                }

                if (!roles.Any(x => x.RoleName == "InitiatorAdmin"))
                {
                    dataContext.webpages_Roles.Add(new webpages_Roles()
                    {
                        RoleName = "InitiatorAdmin"
                    });
                    dataContext.SaveChanges();
                }

                #endregion

                #region Add Roles

                if (!roles.Any(x => x.RoleName == "InitiatorDepartment"))
                {
                    dataContext.webpages_Roles.Add(new webpages_Roles()
                    {
                        RoleName = "InitiatorDepartment"
                    });
                    dataContext.SaveChanges();
                }

                if (!roles.Any(x => x.RoleName == "HODDepartment"))
                {
                    dataContext.webpages_Roles.Add(new webpages_Roles()
                    {
                        RoleName = "HODDepartment"
                    });
                    dataContext.SaveChanges();
                }

                if (!roles.Any(x => x.RoleName == "FinanceDepartment"))
                {
                    dataContext.webpages_Roles.Add(new webpages_Roles()
                    {
                        RoleName = "FinanceDepartment"
                    });
                    dataContext.SaveChanges();
                }

                if (!roles.Any(x => x.RoleName == "LegalDepartment"))
                {
                    dataContext.webpages_Roles.Add(new webpages_Roles()
                    {
                        RoleName = "LegalDepartment"
                    });
                    dataContext.SaveChanges();
                }

                if (!roles.Any(x => x.RoleName == "ITDepartment"))
                {
                    dataContext.webpages_Roles.Add(new webpages_Roles()
                    {
                        RoleName = "ITDepartment"
                    });
                    dataContext.SaveChanges();
                }

                #endregion
            }
        }

        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

        protected void Application_EndRequest()
        {
            var context = new HttpContextWrapper(Context);

            //Do a direct 401 unautorized
            if (Context.Response.StatusCode == 302 && context.Request.IsAjaxRequest())
            {
                Context.Response.Clear();
                Context.Response.StatusCode = 401;
            }
        }
    }
}
