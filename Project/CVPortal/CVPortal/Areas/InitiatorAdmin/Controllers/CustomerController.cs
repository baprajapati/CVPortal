using CVPortal.App_Code;
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
using WebMatrix.WebData;

namespace CVPortal.Areas.InitiatorAdmin.Controllers
{
    public class CustomerController : Controller
    {
        CVPortalEntities dataContext = new CVPortalEntities();

        public CustomerController()
        {
        }

        [HttpGet]
        public ActionResult AddCustomer()
        {
            try
            {
                if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                    return RedirectToAction("../../Account/Login");

                return View(new CustomerViewModel());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult CustomerIndex()
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
        public ActionResult AddCustomer(CustomerViewModel customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var objCustomer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.Email == customer.Email);
                    if (objCustomer != null)
                    {
                        ModelState.AddModelError(nameof(customer.Email), "Email already exist.");
                        return View(customer);
                    }

                    var data = new Cust_reg_tbl()
                    {
                        Org_Sts = "1"
                    };

                    data.Cust_name = customer.Cust_name;
                    data.Email = customer.Email;
                    data.CreatedById = WebSecurity.CurrentUserId;
                    data.CreatedByDate = DateTime.Now;

                    dataContext.Cust_reg_tbl.Add(data);
                    dataContext.SaveChanges();

                    var newCustomerId = 0;
                    using (CVPortalEntities portalEntities = new CVPortalEntities())
                    {
                        var customerList = portalEntities.Cust_reg_tbl.OrderByDescending(x => x.CreatedByDate).ToList();
                        newCustomerId = customerList.FirstOrDefault().ID;
                    }

                    string mailTo = customer.Email;
                    string CC = string.Empty;
                    string BCC = string.Empty;
                    string subject = "Your customer form details";

                    var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\CustomerRequest.html"));
                    string body = htmlContent.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/CustomerCustomerLogin/{newCustomerId}");
                    body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                    body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                    string displayName = string.Empty;
                    string attachments = string.Empty;
                    Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, WebSecurity.CurrentUserId, EmailTypeEnum.Customer, newCustomerId);

                    return RedirectToAction("CustomerIndex");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(customer);
        }

        public ActionResult GetCustomer(CustomerListModel model)
        {
            var result = new JsonResult();
            try
            {
                var data = new List<Cust_reg_tbl>();

                if (!string.IsNullOrEmpty(model.CustomerCode))
                {
                    data = dataContext.Cust_reg_tbl.Where(x => x.CustomerCode != null && x.CustomerCode.ToLower().Contains(model.CustomerCode.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Email))
                {
                    data = (!string.IsNullOrEmpty(model.CustomerCode))
                        ? data.Where(x => x.Email != null && x.Email.ToLower().Contains(model.Email.ToLower())).ToList()
                        : dataContext.Cust_reg_tbl.Where(x => x.Email != null && x.Email.ToLower().Contains(model.Email.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Cust_name))
                {
                    data = (!string.IsNullOrEmpty(model.CustomerCode)) || (!string.IsNullOrEmpty(model.Email))
                        ? data.Where(x => x.Cust_name != null && x.Cust_name.ToLower().Contains(model.Cust_name.ToLower())).ToList()
                        : dataContext.Cust_reg_tbl.Where(x => x.Cust_name != null && x.Cust_name.ToLower().Contains(model.Cust_name.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Status))
                {
                    if ((!string.IsNullOrEmpty(model.CustomerCode)) || (!string.IsNullOrEmpty(model.Email)) || (!string.IsNullOrEmpty(model.Cust_name)))
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
                            data = new List<Cust_reg_tbl>();
                        }
                    }
                    else
                    {
                        if ("Approved".Contains(model.Status))
                        {
                            data = dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved).ToList();
                        }
                        else if ("Pending".Contains(model.Status))
                        {
                            data = dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved).ToList();
                        }
                        else
                        {
                            data = new List<Cust_reg_tbl>();
                        }
                    }
                }

                if (string.IsNullOrEmpty(model.CustomerCode) && string.IsNullOrEmpty(model.Email)
                    && string.IsNullOrEmpty(model.Cust_name) && string.IsNullOrEmpty(model.Status))
                {
                    data = dataContext.Cust_reg_tbl.ToList();
                }

                var customerApprovers = dataContext.CustomerApprovals.Where(x => !x.IsDeleted).ToList();
                var customers = new List<CustomerListModel>();

                data.ForEach(item =>
                {
                    customers.Add(new CustomerListModel()
                    {
                        Id = item.ID,
                        Email = item.Email,
                        Cust_name = item.Cust_name,
                        CustomerCode = item.CustomerCode,
                        Status = item.IsFinalApproved ? "Approved" : "Pending",
                        Owner = item.tbl_Users.HANAME,
                        NextApprover = $"{customerApprovers.Where(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.tbl_Users.HANAME} ({customerApprovers.Where(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        InitiatorDepartment = $"{customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.InitiatorDepartment.ToString())?.tbl_Users.HANAME} ({customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.InitiatorDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        HODDepartment = $"{customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.HODDepartment.ToString())?.tbl_Users.HANAME} ({customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.HODDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        LegalDepartment = $"{customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())?.tbl_Users.HANAME} ({customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        FinanceDepartment = $"{customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.FinanceDepartment.ToString())?.tbl_Users.HANAME} ({customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.FinanceDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        ITDepartment = $"{customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.ITDepartment.ToString())?.tbl_Users.HANAME} ({customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.ITDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})"
                    });
                });

                result = this.Json(new
                {
                    draw = Convert.ToInt32(Request.Form.GetValues("draw")[0]),
                    recordsTotal = (customers.Count > 0) ? customers.Count : 0,
                    recordsFiltered = (customers.Count > 0) ? customers.Count : 0,
                    data = customers
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
            }

            return result;
        }

        [HttpGet]
        public JsonResult GetCustomerDetails(string customerCode)
        {
            try
            {
                var objCustomer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.CustomerCode == customerCode);
                var customerDetails = new CustomerListModel()
                {
                    Cust_name = objCustomer?.Cust_name,
                    Email = objCustomer?.Email
                };

                return Json(new { status = true, result = customerDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = false, result = "Some error occured, please try again." });
            }
        }

        [HttpPost]
        public JsonResult CustomerPrint(int id)
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

                var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\Documents\\Customer.html"));
                var customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.ID == id);
                if (customer != null)
                {
                    htmlContent = htmlContent.Replace("[CustomerName]", customer.Cust_name);
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