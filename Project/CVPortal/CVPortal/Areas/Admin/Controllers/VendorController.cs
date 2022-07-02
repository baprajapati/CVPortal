using CVPortal.App_Code;
using CVPortal.Models;
using CVPortal.ViewModels;
using NReco.PdfGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace CVPortal.Areas.Admin.Controllers
{
    public class VendorController : Controller
    {
        CVPortalEntities dataContext = new CVPortalEntities();

        public VendorController()
        {
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

        public ActionResult GetVendor(VendorListModel model)
        {
            var result = new JsonResult();
            try
            {
                var data = new List<Vend_reg_tbl>();

                if (!string.IsNullOrEmpty(model.VendorCode))
                {
                    data = dataContext.Vend_reg_tbl.Where(x => x.VendorCode != null && x.VendorCode.ToLower().Contains(model.VendorCode.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Email))
                {
                    data = (!string.IsNullOrEmpty(model.VendorCode))
                        ? data.Where(x => x.Email != null && x.Email.ToLower().Contains(model.Email.ToLower())).ToList()
                        : dataContext.Vend_reg_tbl.Where(x => x.Email != null && x.Email.ToLower().Contains(model.Email.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.vend_name))
                {
                    data = (!string.IsNullOrEmpty(model.VendorCode)) || (!string.IsNullOrEmpty(model.Email))
                        ? data.Where(x => x.vend_name != null && x.vend_name.ToLower().Contains(model.vend_name.ToLower())).ToList()
                        : dataContext.Vend_reg_tbl.Where(x => x.vend_name != null && x.vend_name.ToLower().Contains(model.vend_name.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Status))
                {
                    if ((!string.IsNullOrEmpty(model.VendorCode)) || (!string.IsNullOrEmpty(model.Email)) || (!string.IsNullOrEmpty(model.vend_name)))
                    {
                        if ("Approved".Contains(model.Status))
                        {
                            data = data.Where(x => x.IsFinalApproved).ToList();
                        }
                        else if ("Pending".Contains(model.Status))
                        {
                            data = data.Where(x => !x.IsFinalApproved).ToList();
                        }
                        else
                        {
                            data = new List<Vend_reg_tbl>();
                        }
                    }
                    else
                    {
                        if ("Approved".Contains(model.Status))
                        {
                            data = dataContext.Vend_reg_tbl.Where(x => x.IsFinalApproved).ToList();
                        }
                        else if ("Pending".Contains(model.Status))
                        {
                            data = dataContext.Vend_reg_tbl.Where(x => !x.IsFinalApproved).ToList();
                        }
                        else
                        {
                            data = new List<Vend_reg_tbl>();
                        }
                    }
                }

                if (string.IsNullOrEmpty(model.VendorCode) && string.IsNullOrEmpty(model.Email)
                    && string.IsNullOrEmpty(model.vend_name) && string.IsNullOrEmpty(model.Status))
                {
                    data = dataContext.Vend_reg_tbl.ToList();
                }

                var vendorApprovers = dataContext.VendorApprovals.Where(x => !x.IsDeleted).ToList();
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
                        Status = item.IsFinalApproved ? "Approved" : "Pending",
                        Owner = item.tbl_Users.HANAME,
                        NextApprover = $"{vendorApprovers.Where(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.tbl_Users.HANAME} ({vendorApprovers.Where(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        InitiatorDepartment = $"{vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.InitiatorDepartment.ToString())?.tbl_Users.HANAME} ({vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.InitiatorDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        HODDepartment = $"{vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.HODDepartment.ToString())?.tbl_Users.HANAME} ({vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.HODDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        LegalDepartment = $"{vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())?.tbl_Users.HANAME} ({vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        FinanceDepartment = $"{vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.FinanceDepartment.ToString())?.tbl_Users.HANAME} ({vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.FinanceDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        ITDepartment = $"{vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.ITDepartment.ToString())?.tbl_Users.HANAME} ({vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.ITDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})"
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

        [HttpPost]
        public JsonResult VendorPrint(int id)
        {
            try
            {
                var htmlToPdf = new HtmlToPdfConverter();
                htmlToPdf.Margins.Top = 15;
                htmlToPdf.Margins.Bottom = 15;
                htmlToPdf.Margins.Left = 8;
                htmlToPdf.Margins.Right = 8;

                var pageNumberHtml = $@"<div style='width:100%;text-align:center;padding-top:15px;'>Page <span class=""page""></span> of <span class=""topage""></span></div>";
                htmlToPdf.PageFooterHtml = pageNumberHtml;

                var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\Documents\\Vendor.html"));
                var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == id);
                if (vendor != null)
                {
                    htmlContent = htmlContent.Replace("[VendorName]", vendor.vend_name);
                }

                return Json(htmlToPdf.GeneratePdf(htmlContent, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}