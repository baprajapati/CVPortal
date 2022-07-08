﻿using CVPortal.App_Code;
using CVPortal.Models;
using CVPortal.ViewModels;
using NReco.PdfGenerator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebMatrix.WebData;

namespace CVPortal.Areas.Users.Controllers
{
    public class VendorController : Controller
    {
        CVPortalEntities dataContext = new CVPortalEntities();

        public VendorController()
        {
        }

        public SelectList GetVendorExistingOptions()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem()
            {
                Text = "-- Select --",
                Value = ""
            });

            list.Add(new SelectListItem()
            {
                Text = "Vendor Bank Details",
                Value = VendorExistingOptionEnum.VendorBankDetails.ToString()
            });

            list.Add(new SelectListItem()
            {
                Text = "Vendor Location",
                Value = VendorExistingOptionEnum.VendorLocation.ToString()
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

                ViewBag.VendorExistingOptionsList = GetVendorExistingOptions();

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

        public ActionResult VendorIndexApproved()
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
                ViewBag.VendorExistingOptionsList = GetVendorExistingOptions();

                if (ModelState.IsValid)
                {
                    if (!vendor.IsNewVendor && string.IsNullOrEmpty(vendor.VendorCode))
                    {
                        ModelState.AddModelError(nameof(vendor.VendorCode), "Please enter vendor code.");
                        return View(vendor);
                    }

                    if (!vendor.IsNewVendor && string.IsNullOrEmpty(vendor.ExistingReason))
                    {
                        ModelState.AddModelError(nameof(vendor.ExistingReason), "Please select any one option to change.");
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
                        Org_Sts = "1"
                    };

                    if (!vendor.IsNewVendor)
                    {
                        objVendor = dataContext.Vend_reg_tbl.Where(x => x.VendorCode == Convert.ToInt32(vendor.VendorCode)).OrderByDescending(x => x.CreatedByDate).FirstOrDefault();
                        data = objVendor;
                        data.IsFinalApproved = false;

                        if (vendor.ExistingReason == VendorExistingOptionEnum.VendorBankDetails.ToString())
                        {
                            data.Step3 = false;
                            data.Step4 = false;
                        }
                        else if (vendor.ExistingReason == VendorExistingOptionEnum.VendorLocation.ToString())
                        {
                            data.Step1 = false;
                            data.Step2 = false;
                            data.Step3 = false;
                            data.Step4 = false;
                        }
                    }

                    data.vend_name = vendor.vend_name;
                    data.Email = vendor.Email;
                    data.VendorCode = !string.IsNullOrEmpty(vendor.VendorCode) ? Convert.ToInt32(vendor.VendorCode) : (int?)null;
                    data.IsNewVendor = vendor.IsNewVendor;
                    data.ExistingReason = vendor.ExistingReason;
                    data.CreatedById = WebSecurity.CurrentUserId;
                    data.CreatedByDate = DateTime.Now;

                    dataContext.Vend_reg_tbl.Add(data);
                    dataContext.SaveChanges();

                    var oldVendorId = dataContext.Vend_reg_tbl.Where(x => x.VendorCode == data.VendorCode && x.IsFinalApproved).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.ID;
                    var objVendorFiles = new List<VendorFile>();
                    var vendorFiles = dataContext.VendorFiles.Where(x => x.VendorId == oldVendorId).ToList();

                    var newVendorId = 0;
                    using (CVPortalEntities portalEntities = new CVPortalEntities())
                    {
                        var vendorList = portalEntities.Vend_reg_tbl.OrderByDescending(x => x.CreatedByDate).ToList();
                        newVendorId = vendorList.FirstOrDefault().ID;
                    }

                    foreach (var item in vendorFiles)
                    {
                        var sourcePath = Server.MapPath($"~/Content/FileUpload/Vendor/{oldVendorId}");
                        var destinationPath = Server.MapPath($"~/Content/FileUpload/Vendor/{newVendorId}");

                        Directory.CreateDirectory(destinationPath);
                        System.IO.File.Copy($"{sourcePath}/{item.Name}", $"{destinationPath}/{item.Name}");

                        item.VendorId = data.ID;
                        objVendorFiles.Add(item);
                    }

                    if (objVendorFiles.Any())
                    {
                        dataContext.VendorFiles.AddRange(objVendorFiles);
                        dataContext.SaveChanges();
                    }

                    string mailTo = vendor.Email;
                    string CC = string.Empty;
                    string BCC = string.Empty;
                    string subject = "Your vendor form details";

                    var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\VendorRequest.html"));
                    string body = htmlContent.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/VendorLogin/{newVendorId}");
                    body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                    body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                    string displayName = string.Empty;
                    string attachments = string.Empty;
                    Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, WebSecurity.CurrentUserId, EmailTypeEnum.Vendor, newVendorId);

                    return RedirectToAction("VendorIndex");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(vendor);
        }

        public ActionResult GetVendor(VendorListModel model)
        {
            var result = new JsonResult();
            try
            {
                var data = new List<Vend_reg_tbl>();

                if (!string.IsNullOrEmpty(model.VendorCode))
                {
                    data = dataContext.Vend_reg_tbl.Where(x => !x.IsFinalApproved && x.VendorCode != null && x.VendorCode.ToString().Contains(model.VendorCode.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Email))
                {
                    data = (!string.IsNullOrEmpty(model.VendorCode))
                        ? data.Where(x => x.Email != null && x.Email.ToLower().Contains(model.Email.ToLower())).ToList()
                        : dataContext.Vend_reg_tbl.Where(x => !x.IsFinalApproved && x.Email != null && x.Email.ToLower().Contains(model.Email.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.vend_name))
                {
                    data = (!string.IsNullOrEmpty(model.VendorCode)) || (!string.IsNullOrEmpty(model.Email))
                        ? data.Where(x => x.vend_name != null && x.vend_name.ToLower().Contains(model.vend_name.ToLower())).ToList()
                        : dataContext.Vend_reg_tbl.Where(x => !x.IsFinalApproved && x.vend_name != null && x.vend_name.ToLower().Contains(model.vend_name.ToLower())).ToList();
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
                            data = new List<Vend_reg_tbl>();
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
                    data = dataContext.Vend_reg_tbl.Where(x => !x.IsFinalApproved).ToList();
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
                        VendorCode = item.VendorCode?.ToString(),
                        Step4 = item.Step4 ?? false,
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

        public ActionResult GetVendorApproved(VendorListModel model)
        {
            var result = new JsonResult();
            try
            {
                var data = new List<Vend_reg_tbl>();

                if (!string.IsNullOrEmpty(model.VendorCode))
                {
                    data = dataContext.Vend_reg_tbl.Where(x => x.IsFinalApproved && x.VendorCode != null && x.VendorCode.ToString().Contains(model.VendorCode.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Email))
                {
                    data = (!string.IsNullOrEmpty(model.VendorCode))
                        ? data.Where(x => x.Email != null && x.Email.ToLower().Contains(model.Email.ToLower())).ToList()
                        : dataContext.Vend_reg_tbl.Where(x => x.IsFinalApproved && x.Email != null && x.Email.ToLower().Contains(model.Email.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.vend_name))
                {
                    data = (!string.IsNullOrEmpty(model.VendorCode)) || (!string.IsNullOrEmpty(model.Email))
                        ? data.Where(x => x.vend_name != null && x.vend_name.ToLower().Contains(model.vend_name.ToLower())).ToList()
                        : dataContext.Vend_reg_tbl.Where(x => x.IsFinalApproved && x.vend_name != null && x.vend_name.ToLower().Contains(model.vend_name.ToLower())).ToList();
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
                            data = new List<Vend_reg_tbl>();
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
                    data = dataContext.Vend_reg_tbl.Where(x => x.IsFinalApproved).ToList();
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
                        VendorCode = item.VendorCode?.ToString(),
                        Step4 = item.Step4 ?? false,
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

        [HttpGet]
        public JsonResult GetVendorDetails(string vendorCode)
        {
            try
            {
                int tempVendorCode = Convert.ToInt32(vendorCode);
                var objVendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.VendorCode == tempVendorCode);
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
                    htmlContent = htmlContent.Replace("[ORG_STS]", vendor.Org_Sts == "1" ? "Private Ltd" :
                        vendor.Org_Sts == "2" ? "Partnership/LLP" : vendor.Org_Sts == "3" ? "Proprietorship" :
                        vendor.Org_Sts == "4" ? "Public Ltd (Listed)" : "Others");
                    htmlContent = htmlContent.Replace("[VEND_NAME]", vendor.vend_name);
                    htmlContent = htmlContent.Replace("[CEO_NAME]", vendor.CEO_name);
                    htmlContent = htmlContent.Replace("[DESIGNATION]", vendor.Designation);
                    htmlContent = htmlContent.Replace("[CONTACT_NO]", vendor.Contact_no);
                    htmlContent = htmlContent.Replace("[EMAIL]", vendor.Email);
                    htmlContent = htmlContent.Replace("[ADDRESS1]", $"{vendor.Address1}, {vendor.Address1Pincode} - {vendor.Address1City}, {vendor.Address1State}, {vendor.Address1Country}");
                    htmlContent = htmlContent.Replace("[ADDRESS2]", $"{vendor.Address2}, {vendor.Address2Pincode} - {vendor.Address2City}, {vendor.Address2State}, {vendor.Address2Country}");
                    htmlContent = htmlContent.Replace("[AC_CONTACT_DESIG]", vendor.AC_contact_Desig);
                    htmlContent = htmlContent.Replace("[AC_CONTACT_NAME]", vendor.AC_contact_name);
                    htmlContent = htmlContent.Replace("[AC_CONTACT_PHNO]", vendor.AC_contact_Phno);
                    htmlContent = htmlContent.Replace("[AC_CONTACT_EMAIL]", vendor.AC_contact_Email);
                    htmlContent = htmlContent.Replace("[SPY_CONTACT_DESIG]", vendor.Spy_contact_Desig);
                    htmlContent = htmlContent.Replace("[SPY_CONTACT_NAME]", vendor.Spy_contact_name);
                    htmlContent = htmlContent.Replace("[SPY_CONTACT_PHNO]", vendor.Spy_contact_Phno);
                    htmlContent = htmlContent.Replace("[SPY_CONTACT_EMAIL]", vendor.Spy_contact_Email);
                    htmlContent = htmlContent.Replace("[CIN_NO]", vendor.CIN_No);
                    htmlContent = htmlContent.Replace("[PAN_NO]", vendor.PAN_No);
                    htmlContent = htmlContent.Replace("[TYPE_VEND_GST]", vendor.Type_vend_gst == "1" ? "Registered" :
                        vendor.Type_vend_gst == "2" ? "Unregistered" : "Composite");
                    htmlContent = htmlContent.Replace("[GST_REG_NO]", vendor.GST_Reg_no);
                    htmlContent = htmlContent.Replace("[ITEM_DESC]", vendor.Item_Desc);
                    htmlContent = htmlContent.Replace("[HSN_SAC_CODE]", vendor.HSN_SAC_code);
                    htmlContent = htmlContent.Replace("[MSME_NO]", vendor.MSME_no);
                    htmlContent = htmlContent.Replace("[ANNU_TURNOVER]", vendor.Annu_TurnOver.ToString());
                    htmlContent = htmlContent.Replace("[FINANCIALYEAR1]", vendor.FinancialYear1.ToString());
                    htmlContent = htmlContent.Replace("[FINANCIALYEAR2]", vendor.FinancialYear2.ToString());
                    htmlContent = htmlContent.Replace("[ISITRFILED1]", vendor.IsITRFiled1 ?? false ? "Yes" : "No");
                    htmlContent = htmlContent.Replace("[ISITRFILED2]", vendor.IsITRFiled2 ?? false ? "Yes" : "No");
                    htmlContent = htmlContent.Replace("[ACKNOWLEDGENO1]", vendor.AcknowledgeNo1);
                    htmlContent = htmlContent.Replace("[ACKNOWLEDGENO2]", vendor.AcknowledgeNo2);
                    htmlContent = htmlContent.Replace("[ITR_FIELD_DTL]", vendor.ITR_Field_dtl);
                    htmlContent = htmlContent.Replace("[BENIFICIARY_NAME]", vendor.Benificiary_name);
                    htmlContent = htmlContent.Replace("[BANK_NAME]", vendor.Bank_name);
                    htmlContent = htmlContent.Replace("[BRANCH_NAME_ADD]", vendor.Branch_name_Add);
                    htmlContent = htmlContent.Replace("[ACCOUNT_NO]", vendor.Account_no);
                    htmlContent = htmlContent.Replace("[MICR_CODE]", vendor.MICR_code.ToString());
                    htmlContent = htmlContent.Replace("[IFSC_RTGS_CODE]", vendor.IFSC_RTGS_code);
                    htmlContent = htmlContent.Replace("[DATE]", vendor.Date.ToString());
                }

                return Json(htmlToPdf.GeneratePdf(htmlContent, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult DownloadExcel(string vendorCode, string email, string vendorName, string status)
        {
            var data = new List<Vend_reg_tbl>();

            if (!string.IsNullOrEmpty(vendorCode))
            {
                data = dataContext.Vend_reg_tbl.Where(x => !x.IsFinalApproved && x.VendorCode != null && x.VendorCode.ToString().Contains(vendorCode.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(email))
            {
                data = (!string.IsNullOrEmpty(vendorCode))
                    ? data.Where(x => x.Email != null && x.Email.ToLower().Contains(email.ToLower())).ToList()
                    : dataContext.Vend_reg_tbl.Where(x => !x.IsFinalApproved && x.Email != null && x.Email.ToLower().Contains(email.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(vendorName))
            {
                data = (!string.IsNullOrEmpty(vendorCode)) || (!string.IsNullOrEmpty(email))
                    ? data.Where(x => x.vend_name != null && x.vend_name.ToLower().Contains(vendorName.ToLower())).ToList()
                    : dataContext.Vend_reg_tbl.Where(x => !x.IsFinalApproved && x.vend_name != null && x.vend_name.ToLower().Contains(vendorName.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(status))
            {
                if ((!string.IsNullOrEmpty(vendorCode)) || (!string.IsNullOrEmpty(email)) || (!string.IsNullOrEmpty(vendorName)))
                {
                    if ("Approved".Contains(status))
                    {
                        data = data.Where(x => x.IsFinalApproved).ToList();
                    }
                    else if ("Pending".Contains(status))
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
                    if ("Approved".Contains(status))
                    {
                        data = new List<Vend_reg_tbl>();
                    }
                    else if ("Pending".Contains(status))
                    {
                        data = dataContext.Vend_reg_tbl.Where(x => !x.IsFinalApproved).ToList();
                    }
                    else
                    {
                        data = new List<Vend_reg_tbl>();
                    }
                }
            }

            if (string.IsNullOrEmpty(vendorCode) && string.IsNullOrEmpty(email)
                && string.IsNullOrEmpty(vendorName) && string.IsNullOrEmpty(status))
            {
                data = dataContext.Vend_reg_tbl.Where(x => !x.IsFinalApproved).ToList();
            }

            var grid = new GridView();
            grid.DataSource = data;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("dd MMM yyyy") + ".xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View("MyView");
        }

        public ActionResult DownloadExcelApproved(string vendorCode, string email, string vendorName, string status)
        {
            var data = new List<Vend_reg_tbl>();

            if (!string.IsNullOrEmpty(vendorCode))
            {
                data = dataContext.Vend_reg_tbl.Where(x => x.IsFinalApproved && x.VendorCode != null && x.VendorCode.ToString().Contains(vendorCode.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(email))
            {
                data = (!string.IsNullOrEmpty(vendorCode))
                    ? data.Where(x => x.Email != null && x.Email.ToLower().Contains(email.ToLower())).ToList()
                    : dataContext.Vend_reg_tbl.Where(x => x.IsFinalApproved && x.Email != null && x.Email.ToLower().Contains(email.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(vendorName))
            {
                data = (!string.IsNullOrEmpty(vendorCode)) || (!string.IsNullOrEmpty(email))
                    ? data.Where(x => x.vend_name != null && x.vend_name.ToLower().Contains(vendorName.ToLower())).ToList()
                    : dataContext.Vend_reg_tbl.Where(x => x.IsFinalApproved && x.vend_name != null && x.vend_name.ToLower().Contains(vendorName.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(status))
            {
                if ((!string.IsNullOrEmpty(vendorCode)) || (!string.IsNullOrEmpty(email)) || (!string.IsNullOrEmpty(vendorName)))
                {
                    if ("Approved".Contains(status))
                    {
                        data = data.Where(x => x.IsFinalApproved).ToList();
                    }
                    else if ("Pending".Contains(status))
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
                    if ("Approved".Contains(status))
                    {
                        data = dataContext.Vend_reg_tbl.Where(x => x.IsFinalApproved).ToList();
                    }
                    else if ("Pending".Contains(status))
                    {
                        data = new List<Vend_reg_tbl>();
                    }
                    else
                    {
                        data = new List<Vend_reg_tbl>();
                    }
                }
            }

            if (string.IsNullOrEmpty(vendorCode) && string.IsNullOrEmpty(email)
                && string.IsNullOrEmpty(vendorName) && string.IsNullOrEmpty(status))
            {
                data = dataContext.Vend_reg_tbl.Where(x => x.IsFinalApproved).ToList();
            }

            var grid = new GridView();
            grid.DataSource = data;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("dd MMM yyyy") + ".xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View("MyView");
        }
    }
}