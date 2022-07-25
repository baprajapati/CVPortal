using CVPortal.App_Code;
using CVPortal.Models;
using CVPortal.ViewModels;
using NReco.PdfGenerator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

        public ActionResult CustomerIndexApproved()
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
                    string body = htmlContent.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/CustomerLogin/{newCustomerId}");
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
                    data = dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved && x.CustomerCode != null && x.CustomerCode.ToString().Contains(model.CustomerCode.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Email))
                {
                    data = (!string.IsNullOrEmpty(model.CustomerCode))
                        ? data.Where(x => x.Email != null && x.Email.ToLower().Contains(model.Email.ToLower())).ToList()
                        : dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved && x.Email != null && x.Email.ToLower().Contains(model.Email.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Cust_name))
                {
                    data = (!string.IsNullOrEmpty(model.CustomerCode)) || (!string.IsNullOrEmpty(model.Email))
                        ? data.Where(x => x.Cust_name != null && x.Cust_name.ToLower().Contains(model.Cust_name.ToLower())).ToList()
                        : dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved && x.Cust_name != null && x.Cust_name.ToLower().Contains(model.Cust_name.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Cust_CodeVehicles))
                {
                    data = (!string.IsNullOrEmpty(model.CustomerCode)) || (!string.IsNullOrEmpty(model.Email)) || (!string.IsNullOrEmpty(model.Cust_name))
                        ? data.Where(x => x.Cust_CodeVehicles != null && x.Cust_CodeVehicles.ToString().ToLower().Contains(model.Cust_CodeVehicles.ToLower())).ToList()
                        : dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved && x.Cust_CodeVehicles != null && x.Cust_CodeVehicles.ToString().ToLower().Contains(model.Cust_CodeVehicles.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Status))
                {
                    if ((!string.IsNullOrEmpty(model.CustomerCode)) || (!string.IsNullOrEmpty(model.Email)) || (!string.IsNullOrEmpty(model.Cust_name))
                        || !string.IsNullOrEmpty(model.Cust_CodeVehicles))
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
                            data = new List<Cust_reg_tbl>();
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
                    && string.IsNullOrEmpty(model.Cust_name) && string.IsNullOrEmpty(model.Status) && string.IsNullOrEmpty(model.Cust_CodeVehicles))
                {
                    data = dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved).ToList();
                }

                var customerApprovers = dataContext.CustomerApprovals.Where(x => !x.IsDeleted).ToList();
                var customers = new List<CustomerListModel>();

                data.ForEach(item =>
                {
                    var documents = new List<string>();
                    if (item.Step4 == true)
                    {
                        foreach (var document in item.CustomerFiles)
                        {
                            documents.Add($"<a href='/Customers/Download/{item.ID}?fileName={document.Name}' target='_blank'>{document.FileUploadType}</a>");
                        }
                    }

                    customers.Add(new CustomerListModel()
                    {
                        Id = item.ID,
                        Email = item.Email,
                        Cust_name = item.Cust_name,
                        CustomerCode = item.CustomerCode?.ToString(),
                        Cust_CodeVehicles = item.Cust_CodeVehicles?.ToString(),
                        Cust_CodeSpares = item.Cust_CodeSpares?.ToString(),
                        Cust_CodeSecurity = item.Cust_CodeSecurity?.ToString(),
                        Step4 = item.Step4 ?? false,
                        Status = item.IsFinalApproved ? "Approved" : "Pending",
                        Owner = item.tbl_Users.HANAME,
                        Documents = string.Join(" | ", documents),
                        NextApprover = item.NextApprover,
                        PreviousApprover = $"{customerApprovers.Where(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.tbl_Users.HANAME} ({customerApprovers.Where(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        LegalDepartment = string.IsNullOrEmpty(item.CINNo_LLPNo) ? (item.Step4 == true ? "Legal Department not required" : "") : $"{customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())?.tbl_Users.HANAME} ({customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
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

        public ActionResult GetCustomerApproved(CustomerListModel model)
        {
            var result = new JsonResult();
            try
            {
                var data = new List<Cust_reg_tbl>();

                if (!string.IsNullOrEmpty(model.CustomerCode))
                {
                    data = dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved && x.CustomerCode != null && x.CustomerCode.ToString().Contains(model.CustomerCode.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Email))
                {
                    data = (!string.IsNullOrEmpty(model.CustomerCode))
                        ? data.Where(x => x.Email != null && x.Email.ToLower().Contains(model.Email.ToLower())).ToList()
                        : dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved && x.Email != null && x.Email.ToLower().Contains(model.Email.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Cust_name))
                {
                    data = (!string.IsNullOrEmpty(model.CustomerCode)) || (!string.IsNullOrEmpty(model.Email))
                        ? data.Where(x => x.Cust_name != null && x.Cust_name.ToLower().Contains(model.Cust_name.ToLower())).ToList()
                        : dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved && x.Cust_name != null && x.Cust_name.ToLower().Contains(model.Cust_name.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Cust_CodeVehicles))
                {
                    data = (!string.IsNullOrEmpty(model.CustomerCode)) || (!string.IsNullOrEmpty(model.Email)) || (!string.IsNullOrEmpty(model.Cust_name))
                        ? data.Where(x => x.Cust_CodeVehicles != null && x.Cust_CodeVehicles.ToString().ToLower().Contains(model.Cust_CodeVehicles.ToLower())).ToList()
                        : dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved && x.Cust_CodeVehicles != null && x.Cust_CodeVehicles.ToString().ToLower().Contains(model.Cust_CodeVehicles.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(model.Status))
                {
                    if ((!string.IsNullOrEmpty(model.CustomerCode)) || (!string.IsNullOrEmpty(model.Email)) || (!string.IsNullOrEmpty(model.Cust_name))
                        || !string.IsNullOrEmpty(model.Cust_CodeVehicles))
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
                            data = new List<Cust_reg_tbl>();
                        }
                        else
                        {
                            data = new List<Cust_reg_tbl>();
                        }
                    }
                }

                if (string.IsNullOrEmpty(model.CustomerCode) && string.IsNullOrEmpty(model.Email)
                      && string.IsNullOrEmpty(model.Cust_name) && string.IsNullOrEmpty(model.Status)
                      && string.IsNullOrEmpty(model.Cust_CodeVehicles))
                {
                    data = dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved).ToList();
                }

                var customerApprovers = dataContext.CustomerApprovals.Where(x => !x.IsDeleted).ToList();
                var customers = new List<CustomerListModel>();

                data.ForEach(item =>
                {
                    var documents = new List<string>();
                    if (item.Step4 == true)
                    {
                        foreach (var document in item.CustomerFiles)
                        {
                            documents.Add($"<a href='/Customers/Download/{item.ID}?fileName={document.Name}' target='_blank'>{document.FileUploadType}</a>");
                        }
                    }

                    customers.Add(new CustomerListModel()
                    {
                        Id = item.ID,
                        Email = item.Email,
                        Cust_name = item.Cust_name,
                        CustomerCode = item.CustomerCode?.ToString(),
                        Cust_CodeVehicles = item.Cust_CodeVehicles?.ToString(),
                        Cust_CodeSpares = item.Cust_CodeSpares?.ToString(),
                        Cust_CodeSecurity = item.Cust_CodeSecurity?.ToString(),
                        Step4 = item.Step4 ?? false,
                        Status = item.IsFinalApproved ? "Approved" : "Pending",
                        Owner = item.tbl_Users.HANAME,
                        Documents = string.Join(" | ", documents),
                        NextApprover = item.NextApprover,
                        PreviousApprover = $"{customerApprovers.Where(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.tbl_Users.HANAME} ({customerApprovers.Where(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        LegalDepartment = string.IsNullOrEmpty(item.CINNo_LLPNo) ? (item.Step4 == true ? "Legal Department not required" : "") : $"{customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())?.tbl_Users.HANAME} ({customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
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
                var objCustomer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.CustomerCode == Convert.ToInt32(customerCode));
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
                htmlContent = htmlContent.Replace("[LogoPath]", System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + "/Content/img/main-logo.jpg");

                var customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.ID == id);
                if (customer != null)
                {
                    htmlContent = htmlContent.Replace("[ORG_STS]", customer.Org_Sts == "1" ? "Private Ltd" :
                                            customer.Org_Sts == "2" ? "Partnership/LLP" : customer.Org_Sts == "3" ? "Proprietorship" :
                                            customer.Org_Sts == "4" ? "Public Ltd (Listed)" : "Others");
                    htmlContent = htmlContent.Replace("[CUST_NAME]", customer.Cust_name);
                    htmlContent = htmlContent.Replace("[CEO_NAME]", customer.CEO_name);
                    htmlContent = htmlContent.Replace("[CEO_DESIGNATION]", customer.CEO_Designation);
                    htmlContent = htmlContent.Replace("[CEO_CONTACT_NO]", customer.CEO_Contact_no);
                    htmlContent = htmlContent.Replace("[CONTACT_NO]", customer.Contact_no);
                    htmlContent = htmlContent.Replace("[EMAIL]", customer.Email);
                    htmlContent = htmlContent.Replace("[ADDRESS1]", $"{customer.Dlr_Address}, {customer.Dlr_Add_Pincode} - {customer.Dlr_Add_City}, {customer.Dlr_Add_State}, {customer.Dlr_Add_Country}");
                    htmlContent = htmlContent.Replace("[ADDRESS2]", $"{customer.Supp_Address}, {customer.Supp_Add_Pincode} - {customer.Supp_Add_City}, {customer.Supp_Add_State}, {customer.Supp_Add_Country}");
                    htmlContent = htmlContent.Replace("[AC_CONTACT_DESIG]", customer.AC_contact_Desig);
                    htmlContent = htmlContent.Replace("[AC_CONTACT_NAME]", customer.AC_contact_name);
                    htmlContent = htmlContent.Replace("[AC_CONTACT_PHNO]", customer.AC_contact_Phno);
                    htmlContent = htmlContent.Replace("[AC_CONTACT_MOB]", customer.AC_contact_Mob);
                    htmlContent = htmlContent.Replace("[AC_CONTACT_EMAIL]", customer.AC_contact_Email);
                    htmlContent = htmlContent.Replace("[CINNO_LLPNO]", customer.CINNo_LLPNo);
                    htmlContent = htmlContent.Replace("[PAN_NO]", customer.PAN_No);
                    htmlContent = htmlContent.Replace("[TYPE_CUST_GST]", customer.Type_Cust_gst == "1" ? "Registered" :
                        customer.Type_Cust_gst == "2" ? "Unregistered" : "Composite");
                    htmlContent = htmlContent.Replace("[GST_REG_NO]", customer.GST_Reg_no);
                    htmlContent = htmlContent.Replace("[SEUCIRTY_DEPOSIT]", customer.Seucirty_Deposit.ToString());
                    htmlContent = htmlContent.Replace("[DDNO_UTRNO]", customer.DDNo_UTRNo);
                    htmlContent = htmlContent.Replace("[BENIFICIARY_NAME]", customer.Benificiary_name);
                    htmlContent = htmlContent.Replace("[BANK_NAME]", customer.Bank_name);
                    htmlContent = htmlContent.Replace("[BRANCH_NAME_ADD]", customer.Branch_name_Add);
                    htmlContent = htmlContent.Replace("[ACCOUNT_NO]", customer.Account_no);
                    htmlContent = htmlContent.Replace("[MICR_CODE]", customer.MICR_code.ToString());
                    htmlContent = htmlContent.Replace("[IFSC_RTGS_CODE]", customer.IFSC_RTGS_code);
                    htmlContent = htmlContent.Replace("[SWIFT_CODE]", customer.Swift_Code);
                    htmlContent = htmlContent.Replace("[ITR_RETURNSTS]", customer.ITR_ReturnSts);
                    htmlContent = htmlContent.Replace("[ITR_RETURNSTSTURNOVER]", customer.ITR_ReturnStsTurnover ?? false ? "Yes" : "No");
                    htmlContent = htmlContent.Replace("[ITR_RETURNTDSDEDUCT]", customer.ITR_ReturnTDSDeduct ?? false ? "Yes" : "No");
                    htmlContent = htmlContent.Replace("[DATE]", $"{customer.Date.ToString().Substring(6, 2)}/{customer.Date.ToString().Substring(4, 2)}/{customer.Date.ToString().Substring(0, 4)}");
                }

                return Json(htmlToPdf.GeneratePdf(htmlContent, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult DownloadExcel(string code, string email, string customerName, string status)
        {
            var data = new List<Cust_reg_tbl>();

            if (!string.IsNullOrEmpty(code))
            {
                data = dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved && x.Cust_CodeVehicles != null && x.Cust_CodeVehicles.ToString().Contains(code.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(email))
            {
                data = (!string.IsNullOrEmpty(code))
                    ? data.Where(x => x.Email != null && x.Email.ToLower().Contains(email.ToLower())).ToList()
                    : dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved && x.Email != null && x.Email.ToLower().Contains(email.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(customerName))
            {
                data = (!string.IsNullOrEmpty(code)) || (!string.IsNullOrEmpty(email))
                    ? data.Where(x => x.Cust_name != null && x.Cust_name.ToLower().Contains(customerName.ToLower())).ToList()
                    : dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved && x.Cust_name != null && x.Cust_name.ToLower().Contains(customerName.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(status))
            {
                if ((!string.IsNullOrEmpty(code)) || (!string.IsNullOrEmpty(email)) || (!string.IsNullOrEmpty(customerName)))
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
                        data = new List<Cust_reg_tbl>();
                    }
                }
                else
                {
                    if ("Approved".Contains(status))
                    {
                        data = new List<Cust_reg_tbl>();
                    }
                    else if ("Pending".Contains(status))
                    {
                        data = dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved).ToList();
                    }
                    else
                    {
                        data = new List<Cust_reg_tbl>();
                    }
                }
            }

            if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(email)
                && string.IsNullOrEmpty(customerName) && string.IsNullOrEmpty(status))
            {
                data = dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved).ToList();
            }

            DataTable dt = new DataTable("XlsGrid");
            dt.Columns.AddRange(new DataColumn[62] { new DataColumn("Main Code"),
                                            new DataColumn("Spare Code"),
                                            new DataColumn("Security Code"),
                                            new DataColumn("Org Status"),
                                            new DataColumn("Customer Name"),
                                            new DataColumn("CEO Name"),
                                            new DataColumn("Designation"),
                                            new DataColumn("CEO Contact No"),
                                            new DataColumn("Email"),
                                            new DataColumn("Contact No"),
                                            new DataColumn("Dlr Address"),
                                            new DataColumn("Dlr Country"),
                                            new DataColumn("Dlr State"),
                                            new DataColumn("Dlr City"),
                                            new DataColumn("Dlr Pincode"),
                                            new DataColumn("Dlr StateCode"),
                                            new DataColumn("Is Same Address"),
                                            new DataColumn("Supp Address"),
                                            new DataColumn("Supp Country"),
                                            new DataColumn("Supp State"),
                                            new DataColumn("Supp City"),
                                            new DataColumn("Supp Pincode"),
                                            new DataColumn("Supp StateCode"),
                                            new DataColumn("AC Contact Designation"),
                                            new DataColumn("AC Contact Name"),
                                            new DataColumn("AC Contact PhNo"),
                                            new DataColumn("AC Contact MobNo"),
                                            new DataColumn("AC Contact Email"),
                                            new DataColumn("CIN No"),
                                            new DataColumn("PAN No"),
                                            new DataColumn("Customer GST Type"),
                                            new DataColumn("GST Reg No"),
                                            new DataColumn("Security Code"),
                                            new DataColumn("DD/UTR No"),
                                            new DataColumn("Benificiary Name"),
                                            new DataColumn("Bank Name"),
                                            new DataColumn("Branch Address"),
                                            new DataColumn("Acc No"),
                                            new DataColumn("MICR Code"),
                                            new DataColumn("IFSC_RTGS Code"),
                                            new DataColumn("Swift Code"),
                                            new DataColumn("ITR Return Status"),
                                            new DataColumn("ITR Return Status Turnover"),
                                            new DataColumn("ITR Return TDS Deduct"),
                                            new DataColumn("Dealer Type"),
                                            new DataColumn("Date"),
                                            new DataColumn("Next Approver Role"),
                                            new DataColumn("Next Approver"),
                                            new DataColumn("Initiator Approval"),
                                            new DataColumn("Legal Department Approval"),
                                            new DataColumn("Finance Department Approval"),
                                            new DataColumn("IT Department Approval"),
                                            new DataColumn("Company"),
                                            new DataColumn("Customer Type"),
                                            new DataColumn("Payment Type"),
                                            new DataColumn("Terms Code"),
                                            new DataColumn("Currency"),
                                            new DataColumn("Agent/Sales"),
                                            new DataColumn("Tax Code"),
                                            new DataColumn("Document Sequence Prefix"),
                                            new DataColumn("Is Final Approved"),
                                            new DataColumn("Created Date")
            });

            foreach (var item in data)
            {
                dt.Rows.Add(item.Cust_CodeVehicles, item.Cust_CodeSpares, item.Cust_CodeSecurity, item.Org_Sts, item.Cust_name, item.CEO_name, item.CEO_Designation, item.CEO_Contact_no,
                    item.Email, item.Contact_no, item.Dlr_Address, item.Dlr_Add_Country, item.Dlr_Add_State, item.Dlr_Add_City, item.Dlr_Add_Pincode, item.Dlr_Add_StateCode, item.IsSameAsDlr_Address == true ? "Yes" : "No", item.Supp_Address,
                    item.Supp_Add_Country, item.Supp_Add_State, item.Supp_Add_City, item.Supp_Add_Pincode, item.Supp_Add_StateCode, item.AC_contact_Desig, item.AC_contact_name, item.AC_contact_Phno, item.AC_contact_Mob, item.AC_contact_Email,
                    item.CINNo_LLPNo, item.PAN_No, item.Type_Cust_gst, item.GST_Reg_no, item.Seucirty_Deposit, item.DDNo_UTRNo, item.Benificiary_name, item.Bank_name, item.Branch_name_Add, item.Account_no, item.MICR_code, item.IFSC_RTGS_code, item.Swift_Code,
                    item.ITR_ReturnSts, item.ITR_ReturnStsTurnover == true ? "Yes" : "No", item.ITR_ReturnTDSDeduct == true ? "Yes" : "No", item.DealerType, item.Date?.ToString("dd MMM yyyy"),
                    item.NextApproverRole, item.NextApprover,
                    item.InitiatorApproval, item.LegalDepartmentApproval, item.FinanceDepartmentApproval, item.ITDepartmentApproval, item.Company, item.CustomerType,
                    item.PaymentType, item.TermsCode, item.CurrencyCode, item.AgentSales, item.TaxCode, item.DocumentSequencePrefix,
                    item.IsFinalApproved == true ? "Yes" : "No", item.CreatedByDate.ToString("dd MMM yyyy HH:mm"));
            }

            var grid = new GridView();
            grid.DataSource = dt;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Customer_" + DateTime.Now.ToString("dd MMM yyyy") + ".xls");
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

        public ActionResult DownloadExcelApproved(string code, string email, string customerName, string status)
        {
            var data = new List<Cust_reg_tbl>();

            if (!string.IsNullOrEmpty(code))
            {
                data = dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved && x.Cust_CodeVehicles != null && x.Cust_CodeVehicles.ToString().Contains(code.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(email))
            {
                data = (!string.IsNullOrEmpty(code))
                    ? data.Where(x => x.Email != null && x.Email.ToLower().Contains(email.ToLower())).ToList()
                    : dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved && x.Email != null && x.Email.ToLower().Contains(email.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(customerName))
            {
                data = (!string.IsNullOrEmpty(code)) || (!string.IsNullOrEmpty(email))
                    ? data.Where(x => x.Cust_name != null && x.Cust_name.ToLower().Contains(customerName.ToLower())).ToList()
                    : dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved && x.Cust_name != null && x.Cust_name.ToLower().Contains(customerName.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(status))
            {
                if ((!string.IsNullOrEmpty(code)) || (!string.IsNullOrEmpty(email)) || (!string.IsNullOrEmpty(customerName)))
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
                        data = new List<Cust_reg_tbl>();
                    }
                }
                else
                {
                    if ("Approved".Contains(status))
                    {
                        data = dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved).ToList();
                    }
                    else if ("Pending".Contains(status))
                    {
                        data = new List<Cust_reg_tbl>();
                    }
                    else
                    {
                        data = new List<Cust_reg_tbl>();
                    }
                }
            }

            if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(email)
                && string.IsNullOrEmpty(customerName) && string.IsNullOrEmpty(status))
            {
                data = dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved).ToList();
            }

            DataTable dt = new DataTable("XlsGrid");
            dt.Columns.AddRange(new DataColumn[62] { new DataColumn("Main Code"),
                                            new DataColumn("Spare Code"),
                                            new DataColumn("Security Code"),
                                            new DataColumn("Org Status"),
                                            new DataColumn("Customer Name"),
                                            new DataColumn("CEO Name"),
                                            new DataColumn("Designation"),
                                            new DataColumn("CEO Contact No"),
                                            new DataColumn("Email"),
                                            new DataColumn("Contact No"),
                                            new DataColumn("Dlr Address"),
                                            new DataColumn("Dlr Country"),
                                            new DataColumn("Dlr State"),
                                            new DataColumn("Dlr City"),
                                            new DataColumn("Dlr Pincode"),
                                            new DataColumn("Dlr StateCode"),
                                            new DataColumn("Is Same Address"),
                                            new DataColumn("Supp Address"),
                                            new DataColumn("Supp Country"),
                                            new DataColumn("Supp State"),
                                            new DataColumn("Supp City"),
                                            new DataColumn("Supp Pincode"),
                                            new DataColumn("Supp StateCode"),
                                            new DataColumn("AC Contact Designation"),
                                            new DataColumn("AC Contact Name"),
                                            new DataColumn("AC Contact PhNo"),
                                            new DataColumn("AC Contact MobNo"),
                                            new DataColumn("AC Contact Email"),
                                            new DataColumn("CIN No"),
                                            new DataColumn("PAN No"),
                                            new DataColumn("Customer GST Type"),
                                            new DataColumn("GST Reg No"),
                                            new DataColumn("Security Code"),
                                            new DataColumn("DD/UTR No"),
                                            new DataColumn("Benificiary Name"),
                                            new DataColumn("Bank Name"),
                                            new DataColumn("Branch Address"),
                                            new DataColumn("Acc No"),
                                            new DataColumn("MICR Code"),
                                            new DataColumn("IFSC_RTGS Code"),
                                            new DataColumn("Swift Code"),
                                            new DataColumn("ITR Return Status"),
                                            new DataColumn("ITR Return Status Turnover"),
                                            new DataColumn("ITR Return TDS Deduct"),
                                            new DataColumn("Dealer Type"),
                                            new DataColumn("Date"),
                                            new DataColumn("Next Approver Role"),
                                            new DataColumn("Next Approver"),
                                            new DataColumn("Initiator Approval"),
                                            new DataColumn("Legal Department Approval"),
                                            new DataColumn("Finance Department Approval"),
                                            new DataColumn("IT Department Approval"),
                                            new DataColumn("Company"),
                                            new DataColumn("Customer Type"),
                                            new DataColumn("Payment Type"),
                                            new DataColumn("Terms Code"),
                                            new DataColumn("Currency"),
                                            new DataColumn("Agent/Sales"),
                                            new DataColumn("Tax Code"),
                                            new DataColumn("Document Sequence Prefix"),
                                            new DataColumn("Is Final Approved"),
                                            new DataColumn("Created Date")
            });

            foreach (var item in data)
            {
                dt.Rows.Add(item.Cust_CodeVehicles, item.Cust_CodeSpares, item.Cust_CodeSecurity, item.Org_Sts, item.Cust_name, item.CEO_name, item.CEO_Designation, item.CEO_Contact_no,
                    item.Email, item.Contact_no, item.Dlr_Address, item.Dlr_Add_Country, item.Dlr_Add_State, item.Dlr_Add_City, item.Dlr_Add_Pincode, item.Dlr_Add_StateCode, item.IsSameAsDlr_Address == true ? "Yes" : "No", item.Supp_Address,
                    item.Supp_Add_Country, item.Supp_Add_State, item.Supp_Add_City, item.Supp_Add_Pincode, item.Supp_Add_StateCode, item.AC_contact_Desig, item.AC_contact_name, item.AC_contact_Phno, item.AC_contact_Mob, item.AC_contact_Email,
                    item.CINNo_LLPNo, item.PAN_No, item.Type_Cust_gst, item.GST_Reg_no, item.Seucirty_Deposit, item.DDNo_UTRNo, item.Benificiary_name, item.Bank_name, item.Branch_name_Add, item.Account_no, item.MICR_code, item.IFSC_RTGS_code, item.Swift_Code,
                    item.ITR_ReturnSts, item.ITR_ReturnStsTurnover == true ? "Yes" : "No", item.ITR_ReturnTDSDeduct == true ? "Yes" : "No", item.DealerType, item.Date?.ToString("dd MMM yyyy"),
                    item.NextApproverRole, item.NextApprover,
                    item.InitiatorApproval, item.LegalDepartmentApproval, item.FinanceDepartmentApproval, item.ITDepartmentApproval, item.Company, item.CustomerType,
                    item.PaymentType, item.TermsCode, item.CurrencyCode, item.AgentSales, item.TaxCode, item.DocumentSequencePrefix,
                    item.IsFinalApproved == true ? "Yes" : "No", item.CreatedByDate.ToString("dd MMM yyyy HH:mm"));
            }

            var grid = new GridView();
            grid.DataSource = dt;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Customer_" + DateTime.Now.ToString("dd MMM yyyy") + ".xls");
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

        public JsonResult Code(string term)
        {
            var result = dataContext.Cust_reg_tbl.Where(c => c.Cust_CodeVehicles != null && c.Cust_CodeVehicles.ToString().ToLower().Contains(term)).Select(a => new { label = a.Cust_CodeVehicles }).Distinct().ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CustomerName(string term)
        {
            var result = dataContext.Cust_reg_tbl.Where(c => c.Cust_name != null && c.Cust_name.ToString().ToLower().Contains(term)).Select(a => new { label = a.Cust_name }).Distinct().ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}