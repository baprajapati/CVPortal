using CVPortal.App_Code;
using CVPortal.Models;
using CVPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace CVPortal.Controllers
{
    public class AccountController : Controller
    {
        CVPortalEntities dataContext = new CVPortalEntities();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        public ActionResult VendorCustomerLogin(int? id)
        {
            return View(new VendorCustomerLoginViewModel()
            {
                Id = id
            });
        }

        [AllowAnonymous]
        public JsonResult SendOTP(string email)
        {
            try
            {
                var objVendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => !x.IsFinalApproved && x.Email == email) ??
                    dataContext.Vend_reg_tbl.FirstOrDefault(x => x.Email == email);
                if (objVendor != null)
                {
                    objVendor.OTP = "123456";//new Random().Next(111111, 999999).ToString();
                    dataContext.SaveChanges();

                    string mailTo = email;
                    string CC = string.Empty;
                    string BCC = string.Empty;
                    string subject = "Your OTP details";

                    var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\SendOTP.html"));
                    string body = htmlContent.Replace("[OTP]", objVendor.OTP);
                    body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                    body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                    string displayName = string.Empty;
                    string attachments = string.Empty;
                    Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true);

                    return Json(new { status = true });
                }

                var objUser = dataContext.tbl_Users.FirstOrDefault(x => x.IsActive && (x.EmailAddress == email || x.HAUSER == email));
                if (objUser != null)
                {
                    objUser.OTP = "123456";//new Random().Next(111111, 999999).ToString();
                    dataContext.SaveChanges();

                    string mailTo = email;
                    string CC = string.Empty;
                    string BCC = string.Empty;
                    string subject = "Your OTP details";

                    var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\SendOTP.html"));
                    string body = htmlContent.Replace("[OTP]", objVendor.OTP);
                    body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                    body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                    string displayName = string.Empty;
                    string attachments = string.Empty;
                    Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true);

                    return Json(new { status = true });
                }

                return Json(new { status = false, result = "Email inactivated or not exists in system." });
            }
            catch (Exception)
            {
                return Json(new { status = false, result = "Some error occured, please try again." });
            }
        }

        [HttpPost]
        public ActionResult VendorCustomerLogin(VendorCustomerLoginViewModel model)
        {
            try
            {
                var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => !x.IsFinalApproved && x.Email == model.Email) ??
                    dataContext.Vend_reg_tbl.FirstOrDefault(x => x.Email == model.Email);
                if (vendor != null && vendor.OTP == model.OTP)
                {
                    WebSecurity.Login(model.Email, Utility.DefaultPassword, false);
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    Utility.UserCode = model.Email;

                    var stepViewName = vendor.Step4 ?? false ? "FinalForm" : vendor.Step3 ?? false ? "VendorStep4" : vendor.Step2 ?? false ? "VendorStep3" : vendor.Step1 ?? false ? "VendorStep2" : "VendorStep1";
                    return Json(new { status = true, result = Utility.UserCode.Equals(vendor.Email) ? stepViewName : "VendorStep1" });
                }

                var user = dataContext.tbl_Users.FirstOrDefault(x => x.EmailAddress == model.Email || x.HAUSER == model.Email);
                if (user != null && user.OTP == model.OTP)
                {
                    WebSecurity.Login(model.Email, Utility.DefaultPassword, false);
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    Utility.UserCode = model.Email;
                    Utility.UserId = user.Id;

                    vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == model.Id);

                    if (vendor != null)
                    {
                        return Json(new { status = true, result = "VendorIndex" });
                    }
                }

                return Json(new { status = false });
            }
            catch (Exception)
            {
                return Json(new { status = false });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = dataContext.tbl_Users.FirstOrDefault(x => x.IsActive && (x.EmailAddress == model.Email || x.HAUSER == model.Email));
                model.Email = user?.EmailAddress;

                if (user != null && WebSecurity.Login(model.Email, model.Password, false))
                {
                    Utility.UserCode = model.Email;
                    Session["UserFullName"] = user.HANAME;
                    FormsAuthentication.SetAuthCookie(model.Email, false);

                    string[] userRoles = Roles.GetRolesForUser(model.Email);

                    if (userRoles.Contains("Admin"))
                    {
                        return RedirectToAction("../Admin/User/UserIndex");
                    }
                    else if (userRoles.Contains("InitiatorAdmin"))
                    {
                        return RedirectToAction("../InitiatorAdmin/Vendor/VendorIndex");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Incorrect user name or password.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Incorrect user name or password.");
                }
            }

            return View(model);
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Utility.UserCode = string.Empty;
            return RedirectToAction("Login");
        }

        public ActionResult VendorCustomerLogout(int id)
        {
            FormsAuthentication.SignOut();
            Utility.UserCode = string.Empty;
            return RedirectToAction("VendorCustomerLogin/" + id);
        }
    }
}