using CVPortal.App_Code;
using CVPortal.Models;
using CVPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace CVPortal.Areas.InitiatorAdmin.Controllers
{
    public class VendorController : Controller
    {
        CVPortalEntities dataContext = new CVPortalEntities();

        public VendorController()
        {
        }

        public SelectList GetRoles(string[] selectedValue)
        {
            var roles = dataContext.webpages_Roles.Where(x => x.RoleName != "Admin").ToList();

            List<SelectListItem> list = new List<SelectListItem>();

            roles.ForEach(item =>
            {
                list.Add(new SelectListItem()
                {
                    Text = item.RoleName,
                    Value = item.RoleName,
                    Selected = selectedValue != null && selectedValue.Contains(item.RoleName)
                });
            });

            return new SelectList(list, "Value", "Text");
        }

        [HttpGet]
        public ActionResult AddVendor()
        {
            try
            {
                if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                    return RedirectToAction("../../Account/Login");

                ViewBag.RoleList = GetRoles(null);

                return View(new VendorViewModel());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult VendorIndex()
        {
            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../../Account/Login");

            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVendor(VendorViewModel vendor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = new Vend_reg_tbl()
                    {
                        Org_Sts = "1",
                        vend_name = vendor.vend_name,
                        Email = vendor.Email
                    };

                    dataContext.Vend_reg_tbl.Add(data);
                    dataContext.SaveChanges();

                    string mailTo = vendor.Email;
                    string CC = string.Empty;
                    string BCC = string.Empty;
                    string subject = "Your vendor form details";
                    string body = "Your vendor form created.";

                    string displayName = string.Empty;
                    string attachments = string.Empty;
                    Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true);

                    return RedirectToAction("VendorIndex");
                }
            }
            catch (Exception ex)
            {
            }

            return View(vendor);
        }

        public ActionResult GetVendor()
        {
            var result = new JsonResult();
            try
            {
                var data = dataContext.Vend_reg_tbl.ToList();
                var vendors = new List<VendorStep1>();

                data.ForEach(item =>
                {
                    vendors.Add(new VendorStep1()
                    {
                        Id = item.ID,
                        Email = item.Email,
                        vend_name = item.vend_name
                    });
                });

                result = this.Json(new
                {
                    draw = Convert.ToInt32(Request.Form.GetValues("draw")[0]),
                    recordsTotal = (vendors.Count > 0) ? vendors.Count : 0,
                    recordsFiltered = (vendors.Count > 0) ? vendors.Count : 0,
                    data = vendors
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }

            return result;
        }
    }
}