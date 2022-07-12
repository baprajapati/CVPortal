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
        public ActionResult Login(string a)
        {
            var user = dataContext.tbl_Users.FirstOrDefault(x => x.IsActive && x.HAUSER == a);
            if (user != null && WebSecurity.Login(user.EmailAddress, user.Password, false))
            {
                Utility.UserCode = user.EmailAddress;
                Utility.UserId = user.Id;
                Session["UserFullName"] = user.HANAME;
                FormsAuthentication.SetAuthCookie(user.EmailAddress, false);

                string[] userRoles = Roles.GetRolesForUser(user.EmailAddress);

                Session["Role"] = userRoles.FirstOrDefault()?.ToString();

                if (userRoles.Contains("Admin"))
                {
                    return RedirectToAction("../Admin/User/UserIndex");
                }
                else
                {
                    return RedirectToAction("../Users/Vendor/VendorIndex");
                }
            }

            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        public ActionResult VendorLogin(int? id, string a)
        {
            return View(new VendorCustomerLoginViewModel()
            {
                Id = id
            });
        }

        [AllowAnonymous]
        public ActionResult CustomerLogin(int? id)
        {
            return View(new VendorCustomerLoginViewModel()
            {
                Id = id
            });
        }

        [AllowAnonymous]
        public JsonResult SendOTPVendor(string email)
        {
            try
            {
                var objVendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => !x.IsFinalApproved && x.Email == email) ??
                    dataContext.Vend_reg_tbl.FirstOrDefault(x => x.Email == email);
                if (objVendor != null)
                {
                    objVendor.OTP = "123456";//new Random().Next(111111, 999999).ToString();
                    dataContext.SaveChanges();

                    email = objVendor?.Email;
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
                    Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, objVendor.ID, EmailTypeEnum.OTPVendor, objVendor.ID);

                    return Json(new { status = true });
                }

                var objUser = dataContext.tbl_Users.FirstOrDefault(x => x.IsActive && (x.EmailAddress == email || x.HAUSER == email));
                if (objUser != null)
                {
                    objUser.OTP = "123456";//new Random().Next(111111, 999999).ToString();
                    dataContext.SaveChanges();
                    email = objUser?.EmailAddress;

                    string mailTo = email;
                    string CC = string.Empty;
                    string BCC = string.Empty;
                    string subject = "Your OTP details";

                    var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\SendOTP.html"));
                    string body = htmlContent.Replace("[OTP]", objUser.OTP);
                    body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                    body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                    string displayName = string.Empty;
                    string attachments = string.Empty;
                    Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, objUser.Id, EmailTypeEnum.OTPUser, objUser.Id);

                    return Json(new { status = true });
                }

                return Json(new { status = false, result = "Email inactivated or not exists in system." });
            }
            catch (Exception)
            {
                return Json(new { status = false, result = "Some error occured, please try again." });
            }
        }

        [AllowAnonymous]
        public JsonResult SendOTPCustomer(string email)
        {
            try
            {
                var objVendor = dataContext.Cust_reg_tbl.FirstOrDefault(x => !x.IsFinalApproved && x.Email == email) ??
                    dataContext.Cust_reg_tbl.FirstOrDefault(x => x.Email == email);
                if (objVendor != null)
                {
                    objVendor.OTP = "123456";//new Random().Next(111111, 999999).ToString();
                    dataContext.SaveChanges();
                    email = objVendor?.Email;

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
                    Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, objVendor.ID, EmailTypeEnum.OTPCustomer, objVendor.ID);

                    return Json(new { status = true });
                }

                var objUser = dataContext.tbl_Users.FirstOrDefault(x => x.IsActive && (x.EmailAddress == email || x.HAUSER == email));
                if (objUser != null)
                {
                    objUser.OTP = "123456";//new Random().Next(111111, 999999).ToString();
                    dataContext.SaveChanges();
                    email = objUser?.EmailAddress;

                    string mailTo = email;
                    string CC = string.Empty;
                    string BCC = string.Empty;
                    string subject = "Your OTP details";

                    var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\SendOTP.html"));
                    string body = htmlContent.Replace("[OTP]", objUser.OTP);
                    body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                    body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                    string displayName = string.Empty;
                    string attachments = string.Empty;
                    Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, objUser.Id, EmailTypeEnum.OTPUser, objUser.Id);

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
        public ActionResult VendorLogin(VendorCustomerLoginViewModel model)
        {
            try
            {
                var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => !x.IsFinalApproved && x.Email == model.Email) ??
                    dataContext.Vend_reg_tbl.FirstOrDefault(x => x.Email == model.Email);
                if (vendor != null && vendor.OTP == model.OTP)
                {
                    WebSecurity.Login(model.Email, Utility.DefaultPassword, false);
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    Utility.UserCode = vendor.Email;
                    Session["Role"] = string.Empty;

                    var stepViewName = vendor.Step4 ?? false ? "FinalForm" : vendor.Step3 ?? false ? "VendorStep4" : vendor.Step2 ?? false ? "VendorStep3" : vendor.Step1 ?? false ? "VendorStep2" : "VendorStep1";
                    return Json(new { status = true, result = Utility.UserCode.Equals(vendor.Email) ? stepViewName : "VendorStep1" });
                }

                var user = dataContext.tbl_Users.FirstOrDefault(x => x.IsActive && (x.EmailAddress == model.Email || x.HAUSER == model.Email));
                if (user != null && user.OTP == model.OTP)
                {
                    WebSecurity.Login(model.Email, Utility.DefaultPassword, false);
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    Utility.UserCode = user.EmailAddress;
                    Utility.UserId = user.Id;
                    Session["Role"] = Roles.GetRolesForUser(user.EmailAddress).First().ToString();

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

        [HttpPost]
        public ActionResult CustomerLogin(VendorCustomerLoginViewModel model)
        {
            try
            {
                var customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => !x.IsFinalApproved && x.Email == model.Email) ??
                    dataContext.Cust_reg_tbl.FirstOrDefault(x => x.Email == model.Email);
                if (customer != null && customer.OTP == model.OTP)
                {
                    WebSecurity.Login(model.Email, Utility.DefaultPassword, false);
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    Utility.UserCode = customer.Email;
                    Session["Role"] = string.Empty;

                    var stepViewName = customer.Step4 ?? false ? "FinalForm" : customer.Step3 ?? false ? "CustomerStep4" : customer.Step2 ?? false ? "CustomerStep3" : customer.Step1 ?? false ? "CustomerStep2" : "CustomerStep1";
                    return Json(new { status = true, result = Utility.UserCode.Equals(customer.Email) ? stepViewName : "CustomerStep1" });
                }

                var user = dataContext.tbl_Users.FirstOrDefault(x => x.IsActive && (x.EmailAddress == model.Email || x.HAUSER == model.Email));
                if (user != null && user.OTP == model.OTP)
                {
                    WebSecurity.Login(model.Email, Utility.DefaultPassword, false);
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    Utility.UserCode = user.EmailAddress;
                    Utility.UserId = user.Id;
                    Session["Role"] = Roles.GetRolesForUser(user.EmailAddress).First().ToString();

                    customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.ID == model.Id);

                    if (customer != null)
                    {
                        return Json(new { status = true, result = "CustomerIndex" });
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
                    Utility.UserCode = user.EmailAddress;
                    Utility.UserId = user.Id;
                    Session["UserFullName"] = user.HANAME;
                    FormsAuthentication.SetAuthCookie(model.Email, false);

                    string[] userRoles = Roles.GetRolesForUser(model.Email);

                    Session["Role"] = userRoles.FirstOrDefault()?.ToString();

                    if (userRoles.Contains("Admin"))
                    {
                        return RedirectToAction("../Admin/User/UserIndex");
                    }
                    else
                    {
                        return RedirectToAction("../Users/Vendor/VendorIndex");
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
            Utility.UserId = 0;
            return RedirectToAction("Login");
        }

        public ActionResult VendorLogout(int id)
        {
            FormsAuthentication.SignOut();
            Utility.UserCode = string.Empty;
            Utility.UserId = 0;
            return RedirectToAction("VendorLogin/" + id);
        }

        public ActionResult CustomerLogout(int id)
        {
            FormsAuthentication.SignOut();
            Utility.UserCode = string.Empty;
            Utility.UserId = 0;
            return RedirectToAction("CustomerLogin/" + id);
        }
    }
}