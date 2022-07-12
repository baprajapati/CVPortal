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
using System.Web.UI;
using System.Web.UI.WebControls;
using WebMatrix.WebData;

namespace CVPortal.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        CVPortalEntities dataContext = new CVPortalEntities();

        public CustomerController()
        {
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
                    && string.IsNullOrEmpty(model.Cust_name) && string.IsNullOrEmpty(model.Status))
                {
                    data = dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved).ToList();
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
                        CustomerCode = item.CustomerCode?.ToString(),
                        Step4 = item.Step4 ?? false,
                        Status = item.IsFinalApproved ? "Approved" : "Pending",
                        Owner = item.tbl_Users.HANAME,
                        NextApprover = item.NextApprover,
                        PreviousApprover = $"{customerApprovers.Where(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.tbl_Users.HANAME} ({customerApprovers.Where(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        LegalDepartment = string.IsNullOrEmpty(item.CINNo_LLPNo) ? "Legal Department not required" : $"{customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())?.tbl_Users.HANAME} ({customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
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
                            data = new List<Cust_reg_tbl>();
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
                    data = dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved).ToList();
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
                        CustomerCode = item.CustomerCode?.ToString(),
                        Step4 = item.Step4 ?? false,
                        Status = item.IsFinalApproved ? "Approved" : "Pending",
                        Owner = item.tbl_Users.HANAME,
                        NextApprover = item.NextApprover,
                        PreviousApprover = $"{customerApprovers.Where(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.tbl_Users.HANAME} ({customerApprovers.Where(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        LegalDepartment = string.IsNullOrEmpty(item.CINNo_LLPNo) ? "Legal Department not required" : $"{customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())?.tbl_Users.HANAME} ({customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
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
                    htmlContent = htmlContent.Replace("[DATE]", customer.Date.ToString());
                }

                return Json(htmlToPdf.GeneratePdf(htmlContent, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult DownloadExcel(string customerCode, string email, string customerName, string status)
        {
            var data = new List<Cust_reg_tbl>();

            if (!string.IsNullOrEmpty(customerCode))
            {
                data = dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved && x.CustomerCode != null && x.CustomerCode.ToString().Contains(customerCode.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(email))
            {
                data = (!string.IsNullOrEmpty(customerCode))
                    ? data.Where(x => x.Email != null && x.Email.ToLower().Contains(email.ToLower())).ToList()
                    : dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved && x.Email != null && x.Email.ToLower().Contains(email.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(customerName))
            {
                data = (!string.IsNullOrEmpty(customerCode)) || (!string.IsNullOrEmpty(email))
                    ? data.Where(x => x.Cust_name != null && x.Cust_name.ToLower().Contains(customerName.ToLower())).ToList()
                    : dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved && x.Cust_name != null && x.Cust_name.ToLower().Contains(customerName.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(status))
            {
                if ((!string.IsNullOrEmpty(customerCode)) || (!string.IsNullOrEmpty(email)) || (!string.IsNullOrEmpty(customerName)))
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

            if (string.IsNullOrEmpty(customerCode) && string.IsNullOrEmpty(email)
                && string.IsNullOrEmpty(customerName) && string.IsNullOrEmpty(status))
            {
                data = dataContext.Cust_reg_tbl.Where(x => !x.IsFinalApproved).ToList();
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

        public ActionResult DownloadExcelApproved(string customerCode, string email, string customerName, string status)
        {
            var data = new List<Cust_reg_tbl>();

            if (!string.IsNullOrEmpty(customerCode))
            {
                data = dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved && x.CustomerCode != null && x.CustomerCode.ToString().Contains(customerCode.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(email))
            {
                data = (!string.IsNullOrEmpty(customerCode))
                    ? data.Where(x => x.Email != null && x.Email.ToLower().Contains(email.ToLower())).ToList()
                    : dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved && x.Email != null && x.Email.ToLower().Contains(email.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(customerName))
            {
                data = (!string.IsNullOrEmpty(customerCode)) || (!string.IsNullOrEmpty(email))
                    ? data.Where(x => x.Cust_name != null && x.Cust_name.ToLower().Contains(customerName.ToLower())).ToList()
                    : dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved && x.Cust_name != null && x.Cust_name.ToLower().Contains(customerName.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(status))
            {
                if ((!string.IsNullOrEmpty(customerCode)) || (!string.IsNullOrEmpty(email)) || (!string.IsNullOrEmpty(customerName)))
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

            if (string.IsNullOrEmpty(customerCode) && string.IsNullOrEmpty(email)
                && string.IsNullOrEmpty(customerName) && string.IsNullOrEmpty(status))
            {
                data = dataContext.Cust_reg_tbl.Where(x => x.IsFinalApproved).ToList();
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