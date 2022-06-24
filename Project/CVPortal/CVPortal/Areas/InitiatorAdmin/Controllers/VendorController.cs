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
            catch (Exception)
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
            catch (Exception)
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
                    if (!vendor.IsNewVendor && string.IsNullOrEmpty(vendor.VendorCode))
                    {
                        ModelState.AddModelError(nameof(vendor.VendorCode), "Please enter vendor code.");
                        return View(vendor);
                    }

                    var objVendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.Email == vendor.Email);
                    if (vendor.IsNewVendor && objVendor != null)
                    {
                        ModelState.AddModelError(nameof(vendor.Email), "Email already exist.");
                        return View(vendor);
                    }

                    objVendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.Email == vendor.Email && !x.IsFinalApproved);
                    if (!vendor.IsNewVendor && objVendor != null)
                    {
                        ModelState.AddModelError(nameof(vendor.VendorCode), "Vendor is still not approved.");
                        return View(vendor);
                    }

                    var data = new Vend_reg_tbl()
                    {
                        Org_Sts = "1",
                        vend_name = vendor.vend_name,
                        Email = vendor.Email,
                        VendorCode = vendor.VendorCode,
                        IsNewVendor = vendor.IsNewVendor,
                        CreatedById = WebSecurity.CurrentUserId,
                        CreatedByDate = DateTime.Now
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
            catch (Exception)
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
                var vendors = new List<VendorListModel>();

                data.ForEach(item =>
                {
                    vendors.Add(new VendorListModel()
                    {
                        Id = item.ID,
                        Email = item.Email,
                        vend_name = item.vend_name,
                        VendorCode = item.VendorCode,
                        NewExistingVendor = item.IsNewVendor ? "New" : "Existing",
                        Status = item.IsFinalApproved ? "Approved" : "Pending"
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
            catch (Exception)
            {
            }

            return result;
        }

        [HttpGet]
        public JsonResult GetVendorDetails(string vendorCode)
        {
            try
            {
                var objVendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.VendorCode == vendorCode);
                var vendorDetails = new VendorListModel()
                {
                    vend_name = objVendor?.vend_name,
                    Email = objVendor?.Email
                };

                return Json(new { status = true, result = vendorDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = false, result = "Some error occured, please try again." });
            }
        }
    }
}