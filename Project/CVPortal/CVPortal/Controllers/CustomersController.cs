using CVPortal.App_Code;
using CVPortal.Models;
using CVPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace CVPortal.Controllers
{
    public class CustomersController : Controller
    {
        CVPortalEntities dataContext = new CVPortalEntities();

        public ActionResult CustomerStep1(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/CustomerLogin/" + id);

            var model = new CustomerStep1();
            var customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.ID == id);
            if (customer != null)
            {
                if (Utility.UserCode.Equals(customer.Email) && customer.Step4 == true)
                {
                    return RedirectToAction("FinalForm", new { id });
                }

                model.Id = customer.ID;
                model.Org_Sts = customer.Org_Sts;
                model.Cust_name = customer.Cust_name;
                model.CEO_name = customer.CEO_name;
                model.CEO_Designation = customer.CEO_Designation;
                model.Contact_no = customer.Contact_no;
                model.Email = customer.Email;
                model.Dlr_Address = customer.Dlr_Address;
                model.Dlr_Add_Country = customer.Dlr_Add_Country;
                model.Dlr_Add_State = customer.Dlr_Add_State;
                model.Dlr_Add_StateCode = customer.Dlr_Add_StateCode;
                model.Dlr_Add_City = customer.Dlr_Add_City;
                model.Dlr_Add_Pincode = customer.Dlr_Add_Pincode;
                model.IsSameAsDlr_Address = customer.IsSameAsDlr_Address ?? false;
                model.Supp_Address = customer.Supp_Address;
                model.Supp_Add_Country = customer.Supp_Add_Country;
                model.Supp_Add_State = customer.Supp_Add_State;
                model.Supp_Add_StateCode = customer.Supp_Add_StateCode;
                model.Supp_Add_City = customer.Supp_Add_City;
                model.Supp_Add_Pincode = customer.Supp_Add_Pincode;
                model.Contact_no = customer.Contact_no;
                model.IsMain = Utility.UserCode.Equals(customer.Email);
            }
            else
            {
                return RedirectToAction("../Account/CustomerLogin/" + id);
            }

            ViewBag.Id = id;
            return View(model);
        }

        public ActionResult CustomerStep2(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/CustomerLogin/" + id);

            var model = new CustomerStep2();
            var customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.ID == id);
            if (customer != null)
            {
                if (Utility.UserCode.Equals(customer.Email) && customer.Step4 == true)
                {
                    return RedirectToAction("FinalForm", new { id });
                }

                model.Id = customer.ID;
                model.AC_contact_Desig = customer.AC_contact_Desig;
                model.AC_contact_name = customer.AC_contact_name;
                model.AC_contact_Phno = customer.AC_contact_Phno;
                model.AC_contact_Email = customer.AC_contact_Email;
                model.CINNo_LLPNo = customer.CINNo_LLPNo;
                model.PAN_No = customer.PAN_No;
                model.Type_Cust_gst = customer.Type_Cust_gst;
                model.GST_Reg_no = customer.GST_Reg_no;
                model.IsMain = Utility.UserCode.Equals(customer.Email);

                var cinFile = customer.CustomerFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.CIN.ToString());
                if (cinFile != null)
                {
                    model.CINFileName = cinFile.Name;
                }

                var panFile = customer.CustomerFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.Pan.ToString());
                if (panFile != null)
                {
                    model.PANFileName = panFile.Name;
                }

                var gstFile = customer.CustomerFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.GST.ToString());
                if (gstFile != null)
                {
                    model.GSTFileName = gstFile.Name;
                }
            }
            else
            {
                return RedirectToAction("../Account/CustomerLogin/" + id);
            }

            ViewBag.Id = id;
            return View(model);
        }

        public ActionResult CustomerStep3(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/CustomerLogin/" + id);

            var model = new CustomerStep3();
            var customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.ID == id);
            if (customer != null)
            {
                if (Utility.UserCode.Equals(customer.Email) && customer.Step4 == true)
                {
                    return RedirectToAction("FinalForm", new { id });
                }

                model.Id = customer.ID;
                model.Seucirty_Deposit = customer.Seucirty_Deposit;
                model.DDNo_UTRNo = customer.DDNo_UTRNo;
                model.Benificiary_name = customer.Benificiary_name;
                model.Bank_name = customer.Bank_name;
                model.Branch_name_Add = customer.Branch_name_Add;
                model.Account_no = customer.Account_no;
                model.MICR_code = customer.MICR_code;
                model.IFSC_RTGS_code = customer.IFSC_RTGS_code;
                model.Date = customer.Date;
                model.IsMain = Utility.UserCode.Equals(customer.Email);

                var bankFile = customer.CustomerFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.Bank.ToString());
                if (bankFile != null)
                {
                    model.BankFileName = bankFile.Name;
                }
            }
            else
            {
                return RedirectToAction("../Account/CustomerLogin/" + id);
            }

            ViewBag.Id = id;
            return View(model);
        }

        public ActionResult CustomerStep4(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/CustomerLogin/" + id);

            var model = new CustomerStep4();
            var customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.ID == id);
            if (customer != null)
            {
                if (Utility.UserCode.Equals(customer.Email) && customer.Step4 == true)
                {
                    return RedirectToAction("FinalForm", new { id });
                }

                model.Id = customer.ID;
                model.Swift_Code = customer.Swift_Code;
                model.ITR_ReturnSts = customer.ITR_ReturnSts;
                model.ITR_ReturnStsTurnover = customer.ITR_ReturnStsTurnover;
                model.ITR_ReturnTDSDeduct = customer.ITR_ReturnTDSDeduct;
                model.IsMain = Utility.UserCode.Equals(customer.Email);
                model.IsApprover = customer.Step4 ?? false && !customer.IsFinalApproved;
                model.IsCreatedUser = customer.CreatedById == Utility.UserId;

                if (model.IsApprover)
                {
                    var customerApprover = customer.CustomerApprovals.Where(x => !x.IsDeleted && x.CustomerId == id).OrderByDescending(x => x.CreatedByDate).FirstOrDefault();
                    if (customerApprover != null)
                    {
                        if (customerApprover.ApproverRole == ApprovarRoleEnum.NextApprover.ToString())
                        {
                            var isApproverRole = customer.NextApprover == ApprovarRoleEnum.InitiatorDepartment.ToString() || customer.NextApprover == ApprovarRoleEnum.HODDepartment.ToString()
                                || customer.NextApprover == ApprovarRoleEnum.LegalDepartment.ToString() || customer.NextApprover == ApprovarRoleEnum.FinanceDepartment.ToString()
                                || customer.NextApprover == ApprovarRoleEnum.ITDepartment.ToString();

                            if (!isApproverRole)
                            {
                                var user = dataContext.tbl_Users.FirstOrDefault(x => x.HAUSER == customer.NextApprover);
                                model.IsApprover = user?.Id == Utility.UserId;
                            }
                            else
                            {
                                var role = dataContext.webpages_Roles.First(x => x.RoleName == ApprovarRoleEnum.InitiatorDepartment.ToString());
                                model.IsApprover = role.tbl_Users.Any(x => x.Id == Utility.UserId);
                            }
                        }
                        else
                        {
                            var roles = dataContext.webpages_Roles.ToList();
                            if (customerApprover.ApproverRole == ApprovarRoleEnum.InitiatorDepartment.ToString())
                            {
                                model.IsApprover = roles.Where(x => x.RoleName == ApprovarRoleEnum.HODDepartment.ToString())
                                    .SelectMany(x => x.tbl_Users).Any(x => x.Id == Utility.UserId);
                            }
                            else if (customerApprover.ApproverRole == ApprovarRoleEnum.HODDepartment.ToString())
                            {
                                if (string.IsNullOrEmpty(customer.CINNo_LLPNo))
                                {
                                    model.IsApprover = roles.Where(x => x.RoleName == ApprovarRoleEnum.FinanceDepartment.ToString())
                                      .SelectMany(x => x.tbl_Users).Any(x => x.Id == Utility.UserId);
                                }
                                else
                                {
                                    model.IsApprover = roles.Where(x => x.RoleName == ApprovarRoleEnum.LegalDepartment.ToString())
                                       .SelectMany(x => x.tbl_Users).Any(x => x.Id == Utility.UserId);
                                }
                            }
                            else if (customerApprover.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())
                            {
                                model.IsApprover = roles.Where(x => x.RoleName == ApprovarRoleEnum.FinanceDepartment.ToString())
                                    .SelectMany(x => x.tbl_Users).Any(x => x.Id == Utility.UserId);
                            }
                            else if (customerApprover.ApproverRole == ApprovarRoleEnum.FinanceDepartment.ToString())
                            {
                                model.IsApprover = roles.Where(x => x.RoleName == ApprovarRoleEnum.ITDepartment.ToString())
                                    .SelectMany(x => x.tbl_Users).Any(x => x.Id == Utility.UserId);
                            }
                            else
                            {
                                model.IsApprover = false;
                            }
                        }
                    }
                    else
                    {
                        var user = dataContext.tbl_Users.FirstOrDefault(x => x.HAUSER == customer.NextApprover);
                        model.IsApprover = user?.Id == Utility.UserId;
                    }
                }
            }
            else
            {
                return RedirectToAction("../Account/CustomerLogin/" + id);
            }

            ViewBag.Id = id;

            return View(model);
        }

        public ActionResult FinalForm(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/CustomerLogin/" + id);


            var customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.ID == id);
            if (customer != null)
            {
                ViewBag.TextMessage = customer.IsFinalApproved ? "Vendor already approved!" : "Approval is in progress!";
            }


            ViewBag.Id = id;
            return View();
        }

        public ActionResult CustomerIndex(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/CustomerLogin/" + id);

            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public ActionResult CustomerStep1(CustomerStep1 model)
        {
            if (model.IsMain)
            {
                if (ModelState.IsValid)
                {
                    var customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.ID == model.Id);
                    if (customer != null)
                    {
                        customer.Org_Sts = model.Org_Sts;
                        customer.Cust_name = model.Cust_name;
                        customer.CEO_name = model.CEO_name;
                        customer.CEO_Designation = model.CEO_Designation;
                        customer.Contact_no = model.Contact_no;
                        customer.Dlr_Address = model.Dlr_Address;
                        customer.Dlr_Add_Country = model.Dlr_Add_Country;
                        customer.Dlr_Add_State = model.Dlr_Add_State;
                        customer.Dlr_Add_StateCode = model.Dlr_Add_StateCode;
                        customer.Dlr_Add_City = model.Dlr_Add_City;
                        customer.Dlr_Add_Pincode = model.Dlr_Add_Pincode;
                        customer.IsSameAsDlr_Address = model.IsSameAsDlr_Address;
                        customer.Supp_Address = model.Supp_Address;
                        customer.Supp_Add_Country = model.Supp_Add_Country;
                        customer.Supp_Add_State = model.Supp_Add_State;
                        customer.Supp_Add_StateCode = model.Supp_Add_StateCode;
                        customer.Supp_Add_City = model.Supp_Add_City;
                        customer.Supp_Add_Pincode = model.Supp_Add_Pincode;
                        customer.Contact_no = model.Contact_no;
                        customer.Step1 = true;

                        dataContext.SaveChanges();
                    }

                    return RedirectToAction("CustomerStep2", new { id = model.Id });
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("CustomerStep2", new { id = model.Id });
            }
        }

        [HttpPost]
        public ActionResult CustomerStep2(CustomerStep2 model)
        {
            if (model.IsMain)
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(model.CINNo_LLPNo) && string.IsNullOrEmpty(model.CINFileName))
                    {
                        ModelState.AddModelError(nameof(model.CINFileName), "Please upload CIN file");
                        return View(model);
                    }

                    if ((model.Type_Cust_gst == "1" || model.Type_Cust_gst == "3") && string.IsNullOrEmpty(model.GST_Reg_no))
                    {
                        ModelState.AddModelError(nameof(model.GST_Reg_no), "Please enter GST reg no");
                        return View(model);
                    }

                    if ((model.Type_Cust_gst == "1" || model.Type_Cust_gst == "3") && string.IsNullOrEmpty(model.GSTFileName))
                    {
                        ModelState.AddModelError(nameof(model.GSTFileName), "Please upload GST file");
                        return View(model);
                    }

                    var customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.ID == model.Id);
                    if (customer != null)
                    {
                        if (model.CINFile != null && model.CINFile.ContentLength > 0)
                        {
                            var fileName = $"{Path.GetFileNameWithoutExtension(model.CINFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.CINFile.FileName)}";
                            var path = Server.MapPath($"~/Content/FileUpload/Customer/{model.Id}");

                            Directory.CreateDirectory(path);

                            path = Path.Combine(path, fileName);
                            model.CINFile.SaveAs(path);

                            string contentType = model.CINFile.ContentType;
                            using (Stream fileStream = model.CINFile.InputStream)
                            {
                                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                                {
                                    byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);

                                    var customerFile = customer.CustomerFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.CIN.ToString());
                                    if (customerFile != null)
                                    {
                                        customerFile.ContentType = contentType;
                                        customerFile.Data = bytes;
                                        customerFile.FileUploadType = FileUploadEnum.CIN.ToString();
                                        customerFile.Name = fileName;
                                        customerFile.CustomerId = model.Id;
                                    }
                                    else
                                    {
                                        customer.CustomerFiles.Add(new CustomerFile()
                                        {
                                            ContentType = contentType,
                                            Data = bytes,
                                            FileUploadType = FileUploadEnum.CIN.ToString(),
                                            Name = fileName,
                                            CustomerId = model.Id
                                        });
                                    }
                                }
                            }
                        }

                        if (model.PANFile != null && model.PANFile.ContentLength > 0)
                        {
                            var fileName = $"{Path.GetFileNameWithoutExtension(model.PANFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.PANFile.FileName)}";
                            var path = Server.MapPath($"~/Content/FileUpload/Customer/{model.Id}");

                            Directory.CreateDirectory(path);

                            path = Path.Combine(path, fileName); model.PANFile.SaveAs(path);
                            string contentType = model.PANFile.ContentType;
                            using (Stream fileStream = model.PANFile.InputStream)
                            {
                                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                                {
                                    byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);

                                    var customerFile = customer.CustomerFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.Pan.ToString());
                                    if (customerFile != null)
                                    {
                                        customerFile.ContentType = contentType;
                                        customerFile.Data = bytes;
                                        customerFile.FileUploadType = FileUploadEnum.Pan.ToString();
                                        customerFile.Name = fileName;
                                        customerFile.CustomerId = model.Id;
                                    }
                                    else
                                    {
                                        customer.CustomerFiles.Add(new CustomerFile()
                                        {
                                            ContentType = contentType,
                                            Data = bytes,
                                            FileUploadType = FileUploadEnum.Pan.ToString(),
                                            Name = fileName,
                                            CustomerId = model.Id
                                        });
                                    }
                                }
                            }
                        }

                        if (model.GSTFile != null && model.GSTFile.ContentLength > 0)
                        {
                            var fileName = $"{Path.GetFileNameWithoutExtension(model.GSTFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.GSTFile.FileName)}";
                            var path = Server.MapPath($"~/Content/FileUpload/Customer/{model.Id}");

                            Directory.CreateDirectory(path);

                            path = Path.Combine(path, fileName);
                            model.GSTFile.SaveAs(path);

                            string contentType = model.GSTFile.ContentType;
                            using (Stream fileStream = model.GSTFile.InputStream)
                            {
                                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                                {
                                    byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);

                                    var customerFile = customer.CustomerFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.GST.ToString());
                                    if (customerFile != null)
                                    {
                                        customerFile.ContentType = contentType;
                                        customerFile.Data = bytes;
                                        customerFile.FileUploadType = FileUploadEnum.GST.ToString();
                                        customerFile.Name = fileName;
                                        customerFile.CustomerId = model.Id;
                                    }
                                    else
                                    {
                                        customer.CustomerFiles.Add(new CustomerFile()
                                        {
                                            ContentType = contentType,
                                            Data = bytes,
                                            FileUploadType = FileUploadEnum.GST.ToString(),
                                            Name = fileName,
                                            CustomerId = model.Id
                                        });
                                    }
                                }
                            }
                        }

                        customer.AC_contact_Desig = model.AC_contact_Desig;
                        customer.AC_contact_name = model.AC_contact_name;
                        customer.AC_contact_Phno = model.AC_contact_Phno;
                        customer.AC_contact_Email = model.AC_contact_Email;
                        customer.CINNo_LLPNo = model.CINNo_LLPNo;
                        customer.PAN_No = model.PAN_No;
                        customer.Type_Cust_gst = model.Type_Cust_gst;
                        customer.GST_Reg_no = model.GST_Reg_no;
                        customer.Step2 = true;
                        dataContext.SaveChanges();
                    }

                    return RedirectToAction("CustomerStep3", new { id = model.Id });
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("CustomerStep3", new { id = model.Id });
            }
        }

        [HttpPost]
        public ActionResult CustomerStep3(CustomerStep3 model)
        {
            if (model.IsMain)
            {
                if (ModelState.IsValid)
                {
                    var customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.ID == model.Id);
                    if (customer != null)
                    {
                        if (model.BankFile != null && model.BankFile.ContentLength > 0)
                        {
                            var fileName = $"{Path.GetFileNameWithoutExtension(model.BankFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.BankFile.FileName)}";
                            var path = Server.MapPath($"~/Content/FileUpload/Customer/{model.Id}");

                            Directory.CreateDirectory(path);

                            path = Path.Combine(path, fileName);
                            model.BankFile.SaveAs(path);

                            string contentType = model.BankFile.ContentType;
                            using (Stream fileStream = model.BankFile.InputStream)
                            {
                                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                                {
                                    byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);

                                    var customerFile = customer.CustomerFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.Bank.ToString());
                                    if (customerFile != null)
                                    {
                                        customerFile.ContentType = contentType;
                                        customerFile.Data = bytes;
                                        customerFile.FileUploadType = FileUploadEnum.Bank.ToString();
                                        customerFile.Name = fileName;
                                        customerFile.CustomerId = model.Id;
                                    }
                                    else
                                    {
                                        customer.CustomerFiles.Add(new CustomerFile()
                                        {
                                            ContentType = contentType,
                                            Data = bytes,
                                            FileUploadType = FileUploadEnum.Bank.ToString(),
                                            Name = fileName,
                                            CustomerId = model.Id
                                        });
                                    }
                                }
                            }
                        }

                        customer.Seucirty_Deposit = model.Seucirty_Deposit;
                        customer.DDNo_UTRNo = model.DDNo_UTRNo;
                        customer.Benificiary_name = model.Benificiary_name;
                        customer.Bank_name = model.Bank_name;
                        customer.Branch_name_Add = model.Branch_name_Add;
                        customer.Account_no = model.Account_no;
                        customer.MICR_code = model.MICR_code;
                        customer.IFSC_RTGS_code = model.IFSC_RTGS_code;
                        customer.Date = model.Date;
                        customer.Step3 = true;
                        dataContext.SaveChanges();
                    }

                    return RedirectToAction("CustomerStep4", new { id = model.Id });
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("CustomerStep4", new { id = model.Id });
            }
        }

        [HttpPost]
        public ActionResult CustomerStep4(CustomerStep4 model)
        {
            if (ModelState.IsValid)
            {
                var customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.ID == model.Id);
                if (customer != null)
                {
                    customer.Swift_Code = model.Swift_Code;
                    customer.ITR_ReturnSts = model.ITR_ReturnSts;
                    customer.ITR_ReturnStsTurnover = model.ITR_ReturnStsTurnover;
                    customer.ITR_ReturnTDSDeduct = model.ITR_ReturnTDSDeduct;
                    customer.Step4 = true;
                    customer.InitiatorAdminApproval = "P";
                    customer.InitiatorDepartmentApproval = "P";
                    customer.HODDepartmentApproval = "P";
                    customer.LegalDepartmentApproval = "P";
                    customer.FinanceDepartmentApproval = "P";
                    customer.ITDepartmentApproval = "P";
                    customer.NextApprover = customer.tbl_Users.HAUSER;

                    dataContext.SaveChanges();

                    string mailTo = customer.tbl_Users.EmailAddress;
                    string CC = string.Empty;
                    string BCC = string.Empty;
                    string subject = "Your OTP details";

                    var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\CustomerApproval.html"));
                    string body = htmlContent.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/CustomerLogin/{model.Id}");
                    body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                    body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                    string displayName = string.Empty;
                    string attachments = string.Empty;
                    Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, Utility.UserId, EmailTypeEnum.Customer, model.Id);
                }

                return RedirectToAction("FinalForm", new { id = model.Id });
            }

            return View(model);
        }

        [HttpGet]
        public FileResult Download(int id, string fileName)
        {
            return File(Server.MapPath($"~/Content/FileUpload/Customer/{id}/{fileName}"), "application/pdf");
        }

        public JsonResult ApproveCustomerDetails(CustomerApproval model)
        {
            try
            {
                var customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.ID == model.CustomerId);
                if (customer != null)
                {
                    var customerApprover = customer.CustomerApprovals.Where(x => !x.IsDeleted && x.CustomerId == model.CustomerId).OrderByDescending(x => x.CreatedByDate).FirstOrDefault();

                    model.Status = VendorApprovalEnum.Approved.ToString();
                    model.CreatedById = Utility.UserId;
                    model.CreatedByCode = dataContext.tbl_Users.FirstOrDefault(x => x.Id == Utility.UserId)?.HAUSER;
                    model.CreatedByDate = DateTime.Now;

                    if (customerApprover != null)
                    {
                        if (customerApprover.ApproverRole == ApprovarRoleEnum.NextApprover.ToString())
                        {
                            model.ApproverRole = (customer.CustomerApprovals.Count(x => !x.IsDeleted && x.CustomerId == model.CustomerId) == 1 && !string.IsNullOrEmpty(customer.NextApprover)) || !string.IsNullOrEmpty(customerApprover.tbl_Users.HANEXT) ? ApprovarRoleEnum.NextApprover.ToString() : ApprovarRoleEnum.InitiatorDepartment.ToString();
                        }
                        else
                        {
                            model.ApproverRole = customerApprover.ApproverRole == ApprovarRoleEnum.InitiatorDepartment.ToString() ? ApprovarRoleEnum.HODDepartment.ToString()
                                : customerApprover.ApproverRole == ApprovarRoleEnum.HODDepartment.ToString() ? string.IsNullOrEmpty(customer.CINNo_LLPNo) ? ApprovarRoleEnum.FinanceDepartment.ToString() : ApprovarRoleEnum.LegalDepartment.ToString()
                                : customerApprover.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString() ? ApprovarRoleEnum.FinanceDepartment.ToString()
                                : ApprovarRoleEnum.ITDepartment.ToString();
                        }
                    }
                    else
                    {
                        model.ApproverRole = ApprovarRoleEnum.NextApprover.ToString();
                    }

                    if (Session["Role"].ToString() == "InitiatorAdmin")
                    {
                        customer.InitiatorAdminApproval = "A";

                        var nextApprover = dataContext.tbl_Users.FirstOrDefault(x => x.Id == Utility.UserId)?.HANEXT;
                        customer.NextApprover = !string.IsNullOrEmpty(nextApprover) ? nextApprover : ApprovarRoleEnum.InitiatorDepartment.ToString();
                    }
                    else if (model.ApproverRole != ApprovarRoleEnum.NextApprover.ToString())
                    {
                        if (model.ApproverRole == ApprovarRoleEnum.InitiatorDepartment.ToString())
                        {
                            customer.InitiatorDepartmentApproval = "A";
                            customer.NextApprover = ApprovarRoleEnum.HODDepartment.ToString();
                        }
                        else if (model.ApproverRole == ApprovarRoleEnum.HODDepartment.ToString())
                        {
                            customer.HODDepartmentApproval = "A";
                            customer.NextApprover = ApprovarRoleEnum.LegalDepartment.ToString();
                        }
                        else if (model.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())
                        {
                            customer.LegalDepartmentApproval = "A";
                            customer.NextApprover = ApprovarRoleEnum.FinanceDepartment.ToString();
                        }
                        else if (model.ApproverRole == ApprovarRoleEnum.FinanceDepartment.ToString())
                        {
                            customer.FinanceDepartmentApproval = "A";
                            customer.NextApprover = ApprovarRoleEnum.ITDepartment.ToString();
                        }
                        else if (model.ApproverRole == ApprovarRoleEnum.ITDepartment.ToString())
                        {
                            customer.ITDepartmentApproval = "A";
                            customer.NextApprover = null;
                        }
                    }
                    else
                    {
                        var nextApprover = dataContext.tbl_Users.FirstOrDefault(x => x.Id == Utility.UserId)?.HANEXT;
                        customer.NextApprover = !string.IsNullOrEmpty(nextApprover) ? nextApprover : ApprovarRoleEnum.InitiatorDepartment.ToString();
                    }

                    customer.CustomerApprovals.Add(model);

                    if (Session["Role"].ToString() == "InitiatorAdmin" && customer.CreatedById == Utility.UserId)
                    {
                        if (model.DealerType == "Scrap")
                        {
                            customer.Cust_CodeVehicles = dataContext.Cust_reg_tbl.Where(x => x.DealerType == "Scrap").Max(x => x.Cust_CodeSpares) ?? 0 + 1;
                        }
                        else
                        {
                            if (model.DealerType == "Domestic")
                            {
                                customer.Cust_CodeSpares = dataContext.Cust_reg_tbl.Max(x => x.Cust_CodeSpares) ?? 0 + 1;
                            }

                            customer.Cust_CodeVehicles = model.Code;
                        }

                        customer.Cust_CodeSecurity = dataContext.Cust_reg_tbl.Max(x => x.Cust_CodeSecurity) ?? 0 + 1;
                    }

                    if (model.ApproverRole == ApprovarRoleEnum.ITDepartment.ToString())
                    {
                        customer.CustomerCode = customer.CustomerCode != null ? 1000 + customer.ID : customer.CustomerCode;
                        customer.IsFinalApproved = true;
                    }

                    dataContext.SaveChanges();

                    if (model.ApproverRole == ApprovarRoleEnum.NextApprover.ToString())
                    {
                        var user = dataContext.tbl_Users.FirstOrDefault(x => x.Id == Utility.UserId);
                        if (!string.IsNullOrEmpty(user.HANEXT))
                        {
                            user = dataContext.tbl_Users.FirstOrDefault(x => x.HAUSER == user.HANEXT);
                            string mailTo = user.EmailAddress;
                            string CC = string.Empty;
                            string BCC = string.Empty;
                            string subject = "Customer approval details";

                            var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\CustomerApproval.html"));
                            string body = htmlContent.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/CustomerLogin/{model.CustomerId}");
                            body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                            body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                            string displayName = string.Empty;
                            string attachments = string.Empty;
                            Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, Utility.UserId, EmailTypeEnum.Customer, model.CustomerId);
                        }
                        else
                        {
                            var roles = dataContext.webpages_Roles.FirstOrDefault(x => x.RoleName == ApprovarRoleEnum.InitiatorDepartment.ToString());
                            var emails = roles.tbl_Users.Select(x => x.EmailAddress).ToList();

                            string mailTo = string.Join(",", emails);
                            string CC = string.Empty;
                            string BCC = string.Empty;
                            string subject = "Customer approval details";

                            var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\CustomerApproval.html"));
                            string body = htmlContent.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/CustomerLogin/{model.CustomerId}");
                            body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                            body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                            string displayName = string.Empty;
                            string attachments = string.Empty;
                            Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, Utility.UserId, EmailTypeEnum.Customer, model.Id);
                        }
                    }
                    else
                    {
                        var nextApproverRole = model.ApproverRole == ApprovarRoleEnum.InitiatorDepartment.ToString() ? ApprovarRoleEnum.HODDepartment.ToString()
                                : model.ApproverRole == ApprovarRoleEnum.HODDepartment.ToString() ? string.IsNullOrEmpty(customer.CINNo_LLPNo) ? ApprovarRoleEnum.FinanceDepartment.ToString() : ApprovarRoleEnum.LegalDepartment.ToString()
                                : model.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString() ? ApprovarRoleEnum.FinanceDepartment.ToString()
                                : ApprovarRoleEnum.ITDepartment.ToString();

                        if (!string.IsNullOrEmpty(nextApproverRole))
                        {
                            var roles = dataContext.webpages_Roles.FirstOrDefault(x => x.RoleName == nextApproverRole);
                            var emails = roles.tbl_Users.Select(x => x.EmailAddress).ToList();

                            string mailTo = string.Join(",", emails);
                            string CC = string.Empty;
                            string BCC = string.Empty;
                            string subject = "Customer approval details";

                            var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\CustomerApproval.html"));
                            string body = htmlContent.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/CustomerLogin/{model.CustomerId}");
                            body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                            body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                            string displayName = string.Empty;
                            string attachments = string.Empty;
                            Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, Utility.UserId, EmailTypeEnum.Customer, model.Id);
                        }
                    }

                    string mailTo1 = $"{Utility.UserCode},{customer.Email},{customer.tbl_Users.EmailAddress}";
                    string CC1 = string.Empty;
                    string BCC1 = string.Empty;
                    string subject1 = "Customer approval details";

                    var htmlContent1 = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\CustomerApproved.html"));
                    string body1 = htmlContent1.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/CustomerLogin/{model.CustomerId}");
                    body1 = body1.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                    body1 = body1.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                    string displayName1 = string.Empty;
                    string attachments1 = string.Empty;
                    Utility.SendMail(mailTo1, CC1, BCC1, subject1, body1, displayName1, attachments1, true, Utility.UserId, EmailTypeEnum.Customer, model.Id);

                    return Json(new { status = true });
                }

                return Json(new { status = false, result = "Customer not exists in system." });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, result = "Some error occured, please try again." });
            }
        }

        public JsonResult RejectCustomerDetails(int id, string remarks)
        {
            try
            {
                var customer = dataContext.Cust_reg_tbl.FirstOrDefault(x => x.ID == id);
                if (customer != null)
                {
                    var customerApprover = customer.CustomerApprovals.Where(x => !x.IsDeleted && x.CustomerId == id).OrderByDescending(x => x.CreatedByDate).FirstOrDefault();

                    foreach (var item in customer.CustomerApprovals)
                    {
                        item.IsDeleted = true;
                    }

                    var data = new CustomerApproval()
                    {
                        CustomerId = id,
                        Status = VendorApprovalEnum.Rejected.ToString(),
                        Remarks = remarks,
                        IsDeleted = true,
                        CreatedById = Utility.UserId,
                        CreatedByDate = DateTime.Now
                    };

                    if (customerApprover != null)
                    {
                        if (customerApprover.ApproverRole == ApprovarRoleEnum.NextApprover.ToString())
                        {
                            data.ApproverRole = (customer.CustomerApprovals.Count(x => !x.IsDeleted && x.CustomerId == id) == 1 && !string.IsNullOrEmpty(customer.NextApprover)) || !string.IsNullOrEmpty(customerApprover.tbl_Users.HANEXT) ? ApprovarRoleEnum.NextApprover.ToString() : ApprovarRoleEnum.InitiatorDepartment.ToString();
                        }
                        else
                        {
                            data.ApproverRole = customerApprover.ApproverRole == ApprovarRoleEnum.InitiatorDepartment.ToString() ? ApprovarRoleEnum.HODDepartment.ToString()
                                : customerApprover.ApproverRole == ApprovarRoleEnum.HODDepartment.ToString() ? string.IsNullOrEmpty(customer.CINNo_LLPNo) ? ApprovarRoleEnum.FinanceDepartment.ToString() : ApprovarRoleEnum.LegalDepartment.ToString()
                                : customerApprover.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString() ? ApprovarRoleEnum.FinanceDepartment.ToString()
                                : ApprovarRoleEnum.ITDepartment.ToString();
                        }
                    }
                    else
                    {
                        data.ApproverRole = ApprovarRoleEnum.NextApprover.ToString();
                    }

                    if (Session["Role"].ToString() == "InitiatorAdmin")
                    {
                        customer.InitiatorAdminApproval = "R";
                    }
                    else
                    {
                        if (data.ApproverRole == ApprovarRoleEnum.InitiatorDepartment.ToString())
                        {
                            customer.InitiatorDepartmentApproval = "R";
                        }
                        else if (data.ApproverRole == ApprovarRoleEnum.HODDepartment.ToString())
                        {
                            customer.HODDepartmentApproval = "R";
                        }
                        else if (data.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())
                        {
                            customer.LegalDepartmentApproval = "R";
                        }
                        else if (data.ApproverRole == ApprovarRoleEnum.FinanceDepartment.ToString())
                        {
                            customer.FinanceDepartmentApproval = "R";
                        }
                        else if (data.ApproverRole == ApprovarRoleEnum.ITDepartment.ToString())
                        {
                            customer.ITDepartmentApproval = "R";
                        }
                    }

                    customer.CustomerApprovals.Add(data);

                    customer.Step1 = false;
                    customer.Step2 = false;
                    customer.Step3 = false;
                    customer.Step4 = false;
                    customer.NextApprover = null;

                    dataContext.SaveChanges();

                    string mailTo1 = $"{Utility.UserCode},{customer.Email},{customer.tbl_Users.EmailAddress}";
                    string CC1 = string.Empty;
                    string BCC1 = string.Empty;
                    string subject1 = "Customer approval details";

                    var htmlContent1 = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\CustomerRejected.html"));
                    string body1 = htmlContent1.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/CustomerLogin/{id}");
                    body1 = body1.Replace("[REMARKS]", remarks);
                    body1 = body1.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                    body1 = body1.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                    string displayName1 = string.Empty;
                    string attachments1 = string.Empty;
                    Utility.SendMail(mailTo1, CC1, BCC1, subject1, body1, displayName1, attachments1, true, Utility.UserId, EmailTypeEnum.Customer, id);

                    return Json(new { status = true });
                }

                return Json(new { status = false, result = "Customer not exists in system." });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, result = "Some error occured, please try again." });
            }
        }

        public ActionResult GetCustomer()
        {
            var result = new JsonResult();
            try
            {
                var data = dataContext.Cust_reg_tbl.ToList();
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
                        Status = Utility.UserId == 0 ? (item.IsFinalApproved ? "Approved" : "Pending") : customerApprovers.Any(x => x.CustomerId == item.ID && x.CreatedById == Utility.UserId) ? "Approved" : "Pending",
                        Owner = item.tbl_Users.HANAME,
                        NextApprover = item.NextApprover,
                        PreviousApprover = $"{customerApprovers.Where(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.tbl_Users.HANAME} ({customerApprovers.Where(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        InitiatorDepartment = $"{customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.InitiatorDepartment.ToString())?.tbl_Users.HANAME} ({customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.InitiatorDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        HODDepartment = $"{customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.HODDepartment.ToString())?.tbl_Users.HANAME} ({customerApprovers.FirstOrDefault(x => x.CustomerId == item.ID && x.ApproverRole == ApprovarRoleEnum.HODDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
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
    }
}