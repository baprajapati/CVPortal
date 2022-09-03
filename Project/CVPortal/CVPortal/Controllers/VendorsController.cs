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
    public class VendorsController : Controller
    {
        CVPortalEntities dataContext = new CVPortalEntities();

        public ActionResult VendorStep1(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/VendorLogin/" + id);

            var model = new VendorStep1();
            var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == id);
            if (vendor != null)
            {
                if (Utility.UserCode.Equals(vendor.Email) && vendor.Step4 == true && !vendor.IsOpened)
                {
                    return RedirectToAction("FinalForm", new { id });
                }

                model.Id = vendor.ID;
                model.Org_Sts = vendor.Org_Sts;
                model.vend_name = vendor.vend_name;
                model.CEO_name = vendor.CEO_name;
                model.Designation = vendor.Designation;
                model.Contact_no = vendor.Contact_no;
                model.Email = vendor.Email;
                model.Address1 = vendor.Address1;
                model.Address1Country = vendor.Address1Country;
                model.Address1State = vendor.Address1State;
                model.Address1City = vendor.Address1City;
                model.Address1Pincode = vendor.Address1Pincode;
                model.IsSameAsAddress1 = vendor.IsSameAsAddress1 ?? false;
                model.Address2 = vendor.Address2;
                model.Address2Country = vendor.Address2Country;
                model.Address2State = vendor.Address2State;
                model.Address2City = vendor.Address2City;
                model.Address2Pincode = vendor.Address2Pincode;
                model.IsMain = vendor.IsNewVendor && Utility.UserCode.Equals(vendor.Email);
                model.IsExistingUpdate = !vendor.IsNewVendor && vendor.ExistingReason == VendorExistingOptionEnum.VendorLocation.ToString() && Utility.UserCode.Equals(vendor.Email);
            }
            else
            {
                return RedirectToAction("../Account/VendorLogin/" + id);
            }

            ViewBag.Id = id;
            return View(model);
        }

        public ActionResult VendorStep2(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/VendorLogin/" + id);

            var model = new VendorStep2();
            var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == id);
            if (vendor != null)
            {
                if (Utility.UserCode.Equals(vendor.Email) && vendor.Step4 == true && !vendor.IsOpened)
                {
                    return RedirectToAction("FinalForm", new { id });
                }

                model.Id = vendor.ID;
                model.AC_contact_Desig = vendor.AC_contact_Desig;
                model.Spy_contact_Desig = vendor.Spy_contact_Desig;
                model.AC_contact_name = vendor.AC_contact_name;
                model.Spy_contact_name = vendor.Spy_contact_name;
                model.AC_contact_Phno = vendor.AC_contact_Phno;
                model.Spy_contact_Phno = vendor.Spy_contact_Phno;
                model.AC_contact_Email = vendor.AC_contact_Email;
                model.Spy_contact_Email = vendor.Spy_contact_Email;
                model.CIN_No = vendor.CIN_No;
                model.PAN_No = vendor.PAN_No;
                model.Type_vend_gst = vendor.Type_vend_gst;
                model.GST_Reg_no = vendor.GST_Reg_no;
                model.Item_Desc = vendor.Item_Desc;
                model.HSN_SAC_code = vendor.HSN_SAC_code;
                model.MSME_no = vendor.MSME_no;
                model.Annu_TurnOver = vendor.Annu_TurnOver;
                model.Nature_of_service = vendor.Nature_of_service;
                model.ITR_Field_dtl = vendor.ITR_Field_dtl;
                model.FinancialYear1 = vendor.FinancialYear1;
                model.FinancialYear2 = vendor.FinancialYear2;
                model.IsITRFiled1 = vendor.IsITRFiled1;
                model.IsITRFiled2 = vendor.IsITRFiled2;
                model.AcknowledgeNo1 = vendor.AcknowledgeNo1;
                model.AcknowledgeNo2 = vendor.AcknowledgeNo2;
                model.IsMain = vendor.IsNewVendor && Utility.UserCode.Equals(vendor.Email);

                var cinFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.CIN.ToString());
                if (cinFile != null)
                {
                    model.CINFileName = cinFile.Name;
                }

                var panFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.Pan.ToString());
                if (panFile != null)
                {
                    model.PANFileName = panFile.Name;
                }

                var gstFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.GST.ToString());
                if (gstFile != null)
                {
                    model.GSTFileName = gstFile.Name;
                }

                var msmeFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.MSME.ToString());
                if (msmeFile != null)
                {
                    model.MSMEFileName = msmeFile.Name;
                }
            }
            else
            {
                return RedirectToAction("../Account/VendorLogin/" + id);
            }

            ViewBag.Id = id;
            return View(model);
        }

        public ActionResult VendorStep3(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/VendorLogin/" + id);

            var model = new VendorStep3();
            var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == id);
            if (vendor != null)
            {
                if (Utility.UserCode.Equals(vendor.Email) && vendor.Step4 == true && !vendor.IsOpened)
                {
                    return RedirectToAction("FinalForm", new { id });
                }

                model.Id = vendor.ID;
                model.Benificiary_name = vendor.Benificiary_name;
                model.Bank_name = vendor.Bank_name;
                model.Branch_name_Add = vendor.Branch_name_Add;
                model.Account_no = vendor.Account_no;
                model.MICR_code = vendor.MICR_code;
                model.IFSC_RTGS_code = vendor.IFSC_RTGS_code;
                model.Date = vendor.Date;
                model.IsMain = vendor.IsNewVendor && Utility.UserCode.Equals(vendor.Email);
                model.IsExistingUpdate = !vendor.IsNewVendor && vendor.ExistingReason == VendorExistingOptionEnum.VendorBankDetails.ToString() && Utility.UserCode.Equals(vendor.Email);

                var bankFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.Bank.ToString());
                if (bankFile != null)
                {
                    model.BankFileName = bankFile.Name;
                }
            }
            else
            {
                return RedirectToAction("../Account/VendorLogin/" + id);
            }

            ViewBag.Id = id;
            return View(model);
        }

        public ActionResult VendorStep4(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/VendorLogin/" + id);

            var model = new VendorStep4();
            var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == id);
            if (vendor != null)
            {
                if (Utility.UserCode.Equals(vendor.Email) && vendor.Step4 == true && !vendor.IsOpened)
                {
                    return RedirectToAction("FinalForm", new { id });
                }

                model.Id = vendor.ID;
                model.IsMain = vendor.IsNewVendor && Utility.UserCode.Equals(vendor.Email);
                model.IsApprover = vendor.Step4 ?? false && !vendor.IsFinalApproved;
                model.IsExistingUpdate = !vendor.IsNewVendor && !string.IsNullOrEmpty(vendor.ExistingReason) && Utility.UserCode.Equals(vendor.Email);

                if (model.IsApprover)
                {
                    var vendorApprover = vendor.VendorApprovals.Where(x => !x.IsDeleted && x.VendorId == id).OrderByDescending(x => x.CreatedByDate).FirstOrDefault();
                    if (vendorApprover != null)
                    {
                        if (vendorApprover.ApproverRole == ApprovarRoleEnum.NextApprover.ToString())
                        {
                            var isApproverRole = vendor.NextApprover == ApprovarRoleEnum.LegalDepartment.ToString() || vendor.NextApprover == ApprovarRoleEnum.FinanceDepartment.ToString()
                                || vendor.NextApprover == ApprovarRoleEnum.ITDepartment.ToString();

                            if (!isApproverRole)
                            {
                                var user = dataContext.tbl_Users.FirstOrDefault(x => x.HAUSER == vendor.NextApprover);
                                model.IsApprover = user?.Id == Utility.UserId;
                            }
                            else
                            {
                                var roleName = string.IsNullOrEmpty(vendor.CIN_No) ? ApprovarRoleEnum.FinanceDepartment.ToString() : ApprovarRoleEnum.LegalDepartment.ToString();
                                var role = dataContext.webpages_Roles.First(x => x.RoleName == roleName);
                                model.IsApprover = role.tbl_Users.Any(x => x.Id == Utility.UserId) && Session["Role"].ToString() == roleName;
                            }
                        }
                        else
                        {
                            if (vendorApprover.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())
                            {
                                model.IsApprover = dataContext.webpages_Roles.Where(x => x.RoleName == ApprovarRoleEnum.FinanceDepartment.ToString())
                                    .SelectMany(x => x.tbl_Users).Any(x => x.Id == Utility.UserId) && Session["Role"].ToString() == ApprovarRoleEnum.FinanceDepartment.ToString();
                            }
                            else if (vendorApprover.ApproverRole == ApprovarRoleEnum.FinanceDepartment.ToString())
                            {
                                model.IsApprover = dataContext.webpages_Roles.Where(x => x.RoleName == ApprovarRoleEnum.ITDepartment.ToString())
                                    .SelectMany(x => x.tbl_Users).Any(x => x.Id == Utility.UserId) && Session["Role"].ToString() == ApprovarRoleEnum.ITDepartment.ToString();
                            }
                            else
                            {
                                model.IsApprover = false;
                            }
                        }
                    }
                    else
                    {
                        var user = dataContext.tbl_Users.FirstOrDefault(x => x.HAUSER == vendor.NextApprover);
                        model.IsApprover = user?.Id == Utility.UserId && Session["Role"].ToString() == "Initiator";
                    }
                }

                var auditedFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.Audited.ToString());
                if (auditedFile != null)
                {
                    model.AuditedFileName = auditedFile.Name;
                }

                var moaFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.MOA.ToString());
                if (moaFile != null)
                {
                    model.MOAFileName = moaFile.Name;
                }
            }
            else
            {
                return RedirectToAction("../Account/VendorLogin/" + id);
            }

            ViewBag.Id = id;
            ViewBag.TermsCodeList = GetTermsCode(null);
            ViewBag.BankCodeList = GetBankCodes(null);
            ViewBag.PaymentTypeList = GetPaymentTypes(null);
            ViewBag.TaxCodeList = GetTaxCodes(null);
            ViewBag.VendorTypeList = GetVendorTypes(null);

            return View(model);
        }

        public SelectList GetTermsCode(string[] selectedValue)
        {
            var termsCodes = dataContext.PaymentTermsMasters.ToList();

            List<SelectListItem> list = new List<SelectListItem>();

            termsCodes.ForEach(item =>
            {
                list.Add(new SelectListItem()
                {
                    Text = $"{item.PTerms_Code} - {item.PTerms_CodeDesc}",
                    Value = item.PTerms_ID.ToString(),
                    Selected = selectedValue != null && selectedValue.Contains(item.PTerms_ID.ToString())
                });
            });

            return new SelectList(list, "Value", "Text");
        }

        public SelectList GetBankCodes(string[] selectedValue)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem()
            {
                Text = "ICICI",
                Value = "ICICI",
                Selected = selectedValue != null && selectedValue.Contains("ICICI")
            });

            list.Add(new SelectListItem()
            {
                Text = "BTM",
                Value = "BTM",
                Selected = selectedValue != null && selectedValue.Contains("ICICI")
            });

            return new SelectList(list, "Value", "Text");
        }

        public SelectList GetPaymentTypes(string[] selectedValue)
        {
            var termsCodes = dataContext.PayTypeMasters.ToList();

            List<SelectListItem> list = new List<SelectListItem>();

            termsCodes.ForEach(item =>
            {
                list.Add(new SelectListItem()
                {
                    Text = $"{item.PayType_Code} - {item.PayType_Desc}",
                    Value = item.PayType_ID.ToString(),
                    Selected = selectedValue != null && selectedValue.Contains(item.PayType_ID.ToString())
                });
            });

            return new SelectList(list, "Value", "Text");
        }

        public SelectList GetTaxCodes(string[] selectedValue)
        {
            var termsCodes = dataContext.LX_TaxCode.ToList();

            List<SelectListItem> list = new List<SelectListItem>();

            termsCodes.ForEach(item =>
            {
                list.Add(new SelectListItem()
                {
                    Text = $"{item.ItemTaxCDE} - {item.TaxDSC}",
                    Value = item.Id.ToString(),
                    Selected = selectedValue != null && selectedValue.Contains(item.Id.ToString())
                });
            });

            return new SelectList(list, "Value", "Text");
        }

        public SelectList GetVendorTypes(string[] selectedValue)
        {
            var termsCodes = dataContext.VendorTypeMasters.Where(x => x.Currency == "INR").ToList();

            List<SelectListItem> list = new List<SelectListItem>();

            termsCodes.ForEach(item =>
            {
                list.Add(new SelectListItem()
                {
                    Text = $"{item.VendorType} - {item.Description}",
                    Value = item.VT_ID.ToString(),
                    Selected = selectedValue != null && selectedValue.Contains(item.VT_ID.ToString())
                });
            });

            return new SelectList(list, "Value", "Text");
        }

        public ActionResult FinalForm(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/VendorLogin/" + id);

            var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == id);
            if (vendor != null)
            {
                ViewBag.TextMessage = vendor.IsFinalApproved ? "Your form successfully approved!" : "Form Submitted Successfully<br/><br/>Approval is in progress!";
            }

            ViewBag.Id = id;
            return View();
        }

        public ActionResult VendorIndex(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/VendorLogin/" + id);

            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public ActionResult VendorStep1(VendorStep1 model)
        {
            if (model.IsMain || model.IsExistingUpdate)
            {
                if (ModelState.IsValid)
                {
                    model.Contact_no = model.Contact_no.Substring(0, 1) == "0" ? model.Contact_no.Substring(1, model.Contact_no.Length - 1) : model.Contact_no;

                    var isError = false;
                    if (model.Contact_no.Length != 10)
                    {
                        ModelState.AddModelError(nameof(model.Contact_no), "Please add proper contact no.");
                        isError = true;
                    }

                    if (model.Address1.Length > 50)
                    {
                        ModelState.AddModelError(nameof(model.Address1), "Please add address less than 50 character.");
                        isError = true;
                    }

                    if (model.Address2.Length > 50)
                    {
                        ModelState.AddModelError(nameof(model.Address2), "Please add address less than 50 character.");
                        isError = true;
                    }

                    if (isError)
                        return View(model);

                    var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == model.Id);
                    if (vendor != null)
                    {
                        if (!model.IsExistingUpdate)
                        {
                            vendor.Org_Sts = model.Org_Sts;
                            vendor.OrgCode = dataContext.Orginzation_StatusMaster.FirstOrDefault(x => x.Orginzation_Status.ToString() == model.Org_Sts)?.OrgCode?.ToString();
                            vendor.vend_name = model.vend_name;
                            vendor.CEO_name = model.CEO_name;
                            vendor.Designation = model.Designation;
                            vendor.Contact_no = model.Contact_no;
                        }
                        else
                        {
                            vendor.Step2 = true;
                            vendor.Step3 = true;
                        }

                        vendor.Address1 = model.Address1;
                        vendor.Address1Country = model.Address1Country;
                        vendor.Address1State = model.Address1State;
                        vendor.Address1StateCode = dataContext.StateCodeMasters.FirstOrDefault(x => x.StateName == model.Address1State)?.StateCode.ToString();
                        vendor.Address1City = model.Address1City;
                        vendor.Address1Pincode = model.Address1Pincode;
                        vendor.IsSameAsAddress1 = model.IsSameAsAddress1;
                        vendor.Address2 = model.Address2;
                        vendor.Address2Country = model.Address2Country;
                        vendor.Address2State = model.Address2State;
                        vendor.Address2StateCode = dataContext.StateCodeMasters.FirstOrDefault(x => x.StateName == model.Address2State)?.StateCode.ToString();
                        vendor.Address2City = model.Address2City;
                        vendor.Address2Pincode = model.Address2Pincode;
                        vendor.Step1 = true;

                        dataContext.SaveChanges();
                    }

                    return RedirectToAction("VendorStep2", new { id = model.Id });
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("VendorStep2", new { id = model.Id });
            }
        }

        [HttpPost]
        public ActionResult VendorStep2(VendorStep2 model)
        {
            if (model.IsMain)
            {
                var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == model.Id);

                if (model.CINFile != null && !string.IsNullOrEmpty(model.CINFile.FileName))
                {
                    var fileName = $"{Path.GetFileNameWithoutExtension(model.CINFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.CINFile.FileName)}";
                    var path = Server.MapPath($"~/Content/FileUpload/Vendor/{model.Id}");

                    Directory.CreateDirectory(path);

                    path = Path.Combine(path, fileName);
                    model.CINFile.SaveAs(path);
                    model.CINFileName = fileName;

                    string contentType = model.CINFile.ContentType;
                    using (Stream fileStream = model.CINFile.InputStream)
                    {
                        using (BinaryReader binaryReader = new BinaryReader(fileStream))
                        {
                            byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);

                            var vendorFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.CIN.ToString());
                            if (vendorFile != null)
                            {
                                vendorFile.ContentType = contentType;
                                vendorFile.Data = bytes;
                                vendorFile.FileUploadType = FileUploadEnum.CIN.ToString();
                                vendorFile.Name = fileName;
                                vendorFile.VendorId = model.Id;
                            }
                            else
                            {
                                vendor.VendorFiles.Add(new VendorFile()
                                {
                                    ContentType = contentType,
                                    Data = bytes,
                                    FileUploadType = FileUploadEnum.CIN.ToString(),
                                    Name = fileName,
                                    VendorId = model.Id
                                });
                            }
                        }
                    }
                }

                dataContext.SaveChanges();

                if (model.PANFile != null && !string.IsNullOrEmpty(model.PANFile.FileName))
                {
                    var fileName = $"{Path.GetFileNameWithoutExtension(model.PANFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.PANFile.FileName)}";
                    var path = Server.MapPath($"~/Content/FileUpload/Vendor/{model.Id}");

                    Directory.CreateDirectory(path);

                    path = Path.Combine(path, fileName);
                    model.PANFile.SaveAs(path);
                    model.PANFileName = fileName;

                    string contentType = model.PANFile.ContentType;
                    using (Stream fileStream = model.PANFile.InputStream)
                    {
                        using (BinaryReader binaryReader = new BinaryReader(fileStream))
                        {
                            byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);

                            var vendorFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.Pan.ToString());
                            if (vendorFile != null)
                            {
                                vendorFile.ContentType = contentType;
                                vendorFile.Data = bytes;
                                vendorFile.FileUploadType = FileUploadEnum.Pan.ToString();
                                vendorFile.Name = fileName;
                                vendorFile.VendorId = model.Id;
                            }
                            else
                            {
                                vendor.VendorFiles.Add(new VendorFile()
                                {
                                    ContentType = contentType,
                                    Data = bytes,
                                    FileUploadType = FileUploadEnum.Pan.ToString(),
                                    Name = fileName,
                                    VendorId = model.Id
                                });
                            }
                        }
                    }
                }

                dataContext.SaveChanges();

                if (model.GSTFile != null && !string.IsNullOrEmpty(model.GSTFile.FileName))
                {
                    var fileName = $"{Path.GetFileNameWithoutExtension(model.GSTFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.GSTFile.FileName)}";
                    var path = Server.MapPath($"~/Content/FileUpload/Vendor/{model.Id}");

                    Directory.CreateDirectory(path);

                    path = Path.Combine(path, fileName);
                    model.GSTFile.SaveAs(path);
                    model.GSTFileName = fileName;

                    string contentType = model.GSTFile.ContentType;
                    using (Stream fileStream = model.GSTFile.InputStream)
                    {
                        using (BinaryReader binaryReader = new BinaryReader(fileStream))
                        {
                            byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);

                            var vendorFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.GST.ToString());
                            if (vendorFile != null)
                            {
                                vendorFile.ContentType = contentType;
                                vendorFile.Data = bytes;
                                vendorFile.FileUploadType = FileUploadEnum.GST.ToString();
                                vendorFile.Name = fileName;
                                vendorFile.VendorId = model.Id;
                            }
                            else
                            {
                                vendor.VendorFiles.Add(new VendorFile()
                                {
                                    ContentType = contentType,
                                    Data = bytes,
                                    FileUploadType = FileUploadEnum.GST.ToString(),
                                    Name = fileName,
                                    VendorId = model.Id
                                });
                            }
                        }
                    }
                }

                dataContext.SaveChanges();

                if (model.MSMEFile != null && !string.IsNullOrEmpty(model.MSMEFile.FileName))
                {
                    var fileName = $"{Path.GetFileNameWithoutExtension(model.MSMEFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.MSMEFile.FileName)}";
                    var path = Server.MapPath($"~/Content/FileUpload/Vendor/{model.Id}");

                    Directory.CreateDirectory(path);

                    path = Path.Combine(path, fileName);
                    model.MSMEFile.SaveAs(path);
                    model.MSMEFileName = fileName;

                    string contentType = model.MSMEFile.ContentType;
                    using (Stream fileStream = model.MSMEFile.InputStream)
                    {
                        using (BinaryReader binaryReader = new BinaryReader(fileStream))
                        {
                            byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);

                            var vendorFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.MSME.ToString());
                            if (vendorFile != null)
                            {
                                vendorFile.ContentType = contentType;
                                vendorFile.Data = bytes;
                                vendorFile.FileUploadType = FileUploadEnum.MSME.ToString();
                                vendorFile.Name = fileName;
                                vendorFile.VendorId = model.Id;
                            }
                            else
                            {
                                vendor.VendorFiles.Add(new VendorFile()
                                {
                                    ContentType = contentType,
                                    Data = bytes,
                                    FileUploadType = FileUploadEnum.MSME.ToString(),
                                    Name = fileName,
                                    VendorId = model.Id
                                });
                            }
                        }
                    }
                }

                dataContext.SaveChanges();

                if (ModelState.IsValid)
                {
                    if (vendor != null)
                    {
                        var isError = false;
                        if (!string.IsNullOrEmpty(model.CIN_No) && string.IsNullOrEmpty(model.CINFileName))
                        {
                            ModelState.AddModelError(nameof(model.CINFileName), "Please upload CIN file");
                            isError = true;
                        }

                        if ((model.Type_vend_gst == "R" || model.Type_vend_gst == "C") && string.IsNullOrEmpty(model.GST_Reg_no))
                        {
                            ModelState.AddModelError(nameof(model.GST_Reg_no), "Please enter GST reg no");
                            isError = true;
                        }

                        if ((model.Type_vend_gst == "R" || model.Type_vend_gst == "C") && string.IsNullOrEmpty(model.GSTFileName))
                        {
                            ModelState.AddModelError(nameof(model.GSTFileName), "Please upload GST file");
                            isError = true;
                        }

                        model.Spy_contact_Phno = model.Spy_contact_Phno.Substring(0, 1) == "0" ? model.Spy_contact_Phno.Substring(1, model.Spy_contact_Phno.Length - 1) : model.Spy_contact_Phno;

                        if (model.Spy_contact_Phno.Length != 10)
                        {
                            ModelState.AddModelError(nameof(model.Spy_contact_Phno), "Please add proper contact no.");
                            isError = true;
                        }

                        model.AC_contact_Phno = model.AC_contact_Phno.Substring(0, 1) == "0" ? model.AC_contact_Phno.Substring(1, model.AC_contact_Phno.Length - 1) : model.AC_contact_Phno;

                        if (model.AC_contact_Phno.Length != 10)
                        {
                            ModelState.AddModelError(nameof(model.AC_contact_Phno), "Please add proper contact no.");
                            isError = true;
                        }

                        if (!string.IsNullOrEmpty(model.CIN_No) && model.CIN_No.Length != 21)
                        {
                            ModelState.AddModelError(nameof(model.CIN_No), "Please add proper CIN no.");
                            isError = true;
                        }

                        if (model.PAN_No.Length != 10)
                        {
                            ModelState.AddModelError(nameof(model.PAN_No), "Please add proper Pan no.");
                            isError = true;
                        }

                        if (!string.IsNullOrEmpty(model.GST_Reg_no) && model.GST_Reg_no.Length != 15)
                        {
                            ModelState.AddModelError(nameof(model.GST_Reg_no), "Please add proper GSTIN no.");
                            isError = true;
                        }

                        if (!string.IsNullOrEmpty(model.MSME_no) && model.MSME_no.Length != 12)
                        {
                            ModelState.AddModelError(nameof(model.MSME_no), "Please add proper MSME no.");
                            isError = true;
                        }

                        if (!string.IsNullOrEmpty(model.MSME_no) && string.IsNullOrEmpty(model.MSMEFileName))
                        {
                            ModelState.AddModelError(nameof(model.MSMEFileName), "Please upload MSME file");
                            isError = true;
                        }

                        if (model.FinancialYear1.ToString().Length != 4)
                        {
                            ModelState.AddModelError(nameof(model.FinancialYear1), "Please add proper year (yyyy).");
                            isError = true;
                        }

                        if (model.FinancialYear2.ToString().Length != 4)
                        {
                            ModelState.AddModelError(nameof(model.FinancialYear2), "Please add proper year (yyyy).");
                            isError = true;
                        }

                        if (isError)
                            return View(model);

                        vendor.AC_contact_Desig = model.AC_contact_Desig;
                        vendor.Spy_contact_Desig = model.Spy_contact_Desig;
                        vendor.AC_contact_name = model.AC_contact_name;
                        vendor.Spy_contact_name = model.Spy_contact_name;
                        vendor.AC_contact_Phno = model.AC_contact_Phno;
                        vendor.Spy_contact_Phno = model.Spy_contact_Phno;
                        vendor.AC_contact_Email = model.AC_contact_Email;
                        vendor.Spy_contact_Email = model.Spy_contact_Email;
                        vendor.CIN_No = model.CIN_No;
                        vendor.PAN_No = model.PAN_No;
                        vendor.Type_vend_gst = model.Type_vend_gst;
                        vendor.GST_Reg_no = model.GST_Reg_no;
                        vendor.Item_Desc = model.Item_Desc;
                        vendor.HSN_SAC_code = model.HSN_SAC_code;
                        vendor.MSME_no = model.MSME_no;
                        vendor.Annu_TurnOver = model.Annu_TurnOver;
                        vendor.Nature_of_service = model.Nature_of_service;
                        vendor.NCode = dataContext.VendorNatureServices.FirstOrDefault(x => x.NService.ToString() == model.Nature_of_service)?.NCode.ToString();
                        vendor.ITR_Field_dtl = model.ITR_Field_dtl;
                        vendor.FinancialYear1 = model.FinancialYear1;
                        vendor.FinancialYear2 = model.FinancialYear2;
                        vendor.IsITRFiled1 = model.IsITRFiled1;
                        vendor.IsITRFiled2 = model.IsITRFiled2;
                        vendor.AcknowledgeNo1 = model.AcknowledgeNo1;
                        vendor.AcknowledgeNo2 = model.AcknowledgeNo2;
                        vendor.Step2 = true;
                        dataContext.SaveChanges();
                    }

                    return RedirectToAction("VendorStep3", new { id = model.Id });
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("VendorStep3", new { id = model.Id });
            }
        }

        [HttpPost]
        public ActionResult VendorStep3(VendorStep3 model)
        {
            if (model.IsMain || model.IsExistingUpdate)
            {
                var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == model.Id);

                if (model.BankFile != null && !string.IsNullOrEmpty(model.BankFile.FileName))
                {
                    var fileName = $"{Path.GetFileNameWithoutExtension(model.BankFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.BankFile.FileName)}";
                    var path = Server.MapPath($"~/Content/FileUpload/Vendor/{model.Id}");

                    Directory.CreateDirectory(path);

                    path = Path.Combine(path, fileName);
                    model.BankFile.SaveAs(path);
                    model.BankFileName = fileName;

                    string contentType = model.BankFile.ContentType;
                    using (Stream fileStream = model.BankFile.InputStream)
                    {
                        using (BinaryReader binaryReader = new BinaryReader(fileStream))
                        {
                            byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);

                            var vendorFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.Bank.ToString());
                            if (vendorFile != null)
                            {
                                vendorFile.ContentType = contentType;
                                vendorFile.Data = bytes;
                                vendorFile.FileUploadType = FileUploadEnum.Bank.ToString();
                                vendorFile.Name = fileName;
                                vendorFile.VendorId = model.Id;
                            }
                            else
                            {
                                vendor.VendorFiles.Add(new VendorFile()
                                {
                                    ContentType = contentType,
                                    Data = bytes,
                                    FileUploadType = FileUploadEnum.Bank.ToString(),
                                    Name = fileName,
                                    VendorId = model.Id
                                });
                            }
                        }
                    }
                }

                dataContext.SaveChanges();

                if (ModelState.IsValid)
                {
                    if (vendor != null)
                    {
                        if (!model.IsExistingUpdate)
                        {
                            vendor.Benificiary_name = model.Benificiary_name;
                        }

                        vendor.Bank_name = model.Bank_name;
                        vendor.Branch_name_Add = model.Branch_name_Add;
                        vendor.Account_no = model.Account_no;
                        vendor.MICR_code = model.MICR_code;
                        vendor.IFSC_RTGS_code = model.IFSC_RTGS_code;
                        vendor.Date = model.Date;
                        vendor.Step3 = true;
                        dataContext.SaveChanges();
                    }

                    return RedirectToAction("VendorStep4", new { id = model.Id });
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("VendorStep4", new { id = model.Id });
            }
        }

        [HttpPost]
        public ActionResult VendorStep4(VendorStep4 model)
        {
            if (!model.IsExistingUpdate)
            {
                var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == model.Id);

                if (model.AuditedFile != null && !string.IsNullOrEmpty(model.AuditedFile.FileName))
                {
                    var fileName = $"{Path.GetFileNameWithoutExtension(model.AuditedFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.AuditedFile.FileName)}";
                    var path = Server.MapPath($"~/Content/FileUpload/Vendor/{model.Id}");

                    Directory.CreateDirectory(path);

                    path = Path.Combine(path, fileName);
                    model.AuditedFile.SaveAs(path);

                    string contentType = model.AuditedFile.ContentType;
                    using (Stream fileStream = model.AuditedFile.InputStream)
                    {
                        using (BinaryReader binaryReader = new BinaryReader(fileStream))
                        {
                            byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);

                            var vendorFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.Audited.ToString());
                            if (vendorFile != null)
                            {
                                vendorFile.ContentType = contentType;
                                vendorFile.Data = bytes;
                                vendorFile.FileUploadType = FileUploadEnum.Audited.ToString();
                                vendorFile.Name = fileName;
                                vendorFile.VendorId = model.Id;
                            }
                            else
                            {
                                vendor.VendorFiles.Add(new VendorFile()
                                {
                                    ContentType = contentType,
                                    Data = bytes,
                                    FileUploadType = FileUploadEnum.Audited.ToString(),
                                    Name = fileName,
                                    VendorId = model.Id
                                });
                            }
                        }
                    }
                }

                dataContext.SaveChanges();

                if (model.MOAFile != null && !string.IsNullOrEmpty(model.MOAFile.FileName))
                {
                    var fileName = $"{Path.GetFileNameWithoutExtension(model.MOAFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.MOAFile.FileName)}";
                    var path = Server.MapPath($"~/Content/FileUpload/Vendor/{model.Id}");

                    Directory.CreateDirectory(path);

                    path = Path.Combine(path, fileName);
                    model.MOAFile.SaveAs(path);

                    string contentType = model.MOAFile.ContentType;
                    using (Stream fileStream = model.MOAFile.InputStream)
                    {
                        using (BinaryReader binaryReader = new BinaryReader(fileStream))
                        {
                            byte[] bytes = binaryReader.ReadBytes((int)fileStream.Length);

                            var vendorFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.MOA.ToString());
                            if (vendorFile != null)
                            {
                                vendorFile.ContentType = contentType;
                                vendorFile.Data = bytes;
                                vendorFile.FileUploadType = FileUploadEnum.MOA.ToString();
                                vendorFile.Name = fileName;
                                vendorFile.VendorId = model.Id;
                            }
                            else
                            {
                                vendor.VendorFiles.Add(new VendorFile()
                                {
                                    ContentType = contentType,
                                    Data = bytes,
                                    FileUploadType = FileUploadEnum.MOA.ToString(),
                                    Name = fileName,
                                    VendorId = model.Id
                                });
                            }
                        }
                    }
                }

                dataContext.SaveChanges();

                if (ModelState.IsValid)
                {
                    if (vendor != null)
                    {
                        vendor.Step4 = true;
                        vendor.IsOpened = false;
                        vendor.InitiatorApproval = "P";
                        vendor.LegalDepartmentApproval = "P";
                        vendor.FinanceDepartmentApproval = "P";
                        vendor.ITDepartmentApproval = "P";
                        vendor.NextApprover = vendor.tbl_Users.HAUSER;
                        vendor.NextApproverRole = "Initiator";

                        dataContext.SaveChanges();

                        string mailTo = vendor.tbl_Users.EmailAddress;
                        string CC = string.Empty;
                        string BCC = string.Empty;
                        string subject = "Vendor approval details";

                        var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\VendorApproval.html"));
                        string body = htmlContent.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/VendorLogin/{model.Id}");
                        body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                        body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                        string displayName = string.Empty;
                        string attachments = string.Empty;
                        Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, Utility.UserId, EmailTypeEnum.Vendor, model.Id);
                    }

                    return RedirectToAction("FinalForm", new { id = model.Id });
                }
            }
            else
            {
                var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == model.Id);
                if (vendor != null)
                {
                    vendor.Step4 = true;
                    vendor.IsOpened = false;
                    vendor.NextApprover = vendor.tbl_Users.HAUSER;
                    vendor.NextApproverRole = "Initiator";
                    dataContext.SaveChanges();
                }

                string mailTo = vendor.tbl_Users.EmailAddress;
                string CC = string.Empty;
                string BCC = string.Empty;
                string subject = "Vendor approval details";

                var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\VendorApproval.html"));
                string body = htmlContent.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/VendorLogin/{model.Id}");
                body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                string displayName = string.Empty;
                string attachments = string.Empty;
                Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, Utility.UserId, EmailTypeEnum.Vendor, model.Id);

                return RedirectToAction("FinalForm", new { id = model.Id });
            }

            ViewBag.TermsCodeList = GetTermsCode(null);
            ViewBag.BankCodeList = GetBankCodes(null);
            ViewBag.PaymentTypeList = GetPaymentTypes(null);
            ViewBag.TaxCodeList = GetTaxCodes(null);
            ViewBag.VendorTypeList = GetVendorTypes(null);

            return View(model);
        }

        [HttpGet]
        public FileResult Download(int id, string fileType)
        {
            var vendorFile = dataContext.VendorFiles.OrderByDescending(x => x.Id).FirstOrDefault(x => x.VendorId == id && x.FileUploadType == fileType);
            return File(Server.MapPath($"~/Content/FileUpload/Vendor/{id}/{vendorFile?.Name}"), "application/pdf");
        }

        public JsonResult ApproveVendorDetails(VendorApproval model)
        {
            try
            {
                var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == model.VendorId);
                if (vendor != null)
                {
                    var vendorApprover = vendor.VendorApprovals.Where(x => !x.IsDeleted && x.VendorId == model.VendorId).OrderByDescending(x => x.CreatedByDate).FirstOrDefault();

                    model.Status = VendorApprovalEnum.Approved.ToString();
                    model.CreatedById = Utility.UserId;
                    model.CreatedByCode = dataContext.tbl_Users.FirstOrDefault(x => x.Id == Utility.UserId)?.HAUSER;
                    model.CreatedByDate = DateTime.Now;

                    if (vendorApprover != null)
                    {
                        if (vendorApprover.ApproverRole == ApprovarRoleEnum.NextApprover.ToString())
                        {
                            model.ApproverRole = (vendor.VendorApprovals.Count(x => !x.IsDeleted && x.VendorId == model.VendorId) == 1 && !string.IsNullOrEmpty(vendor.NextApprover)) || !string.IsNullOrEmpty(vendorApprover.tbl_Users.HANEXT) ? ApprovarRoleEnum.NextApprover.ToString() : string.IsNullOrEmpty(vendor.CIN_No) ? ApprovarRoleEnum.FinanceDepartment.ToString() : ApprovarRoleEnum.LegalDepartment.ToString();
                        }
                        else
                        {
                            model.ApproverRole = vendorApprover.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString() ? ApprovarRoleEnum.FinanceDepartment.ToString()
                                : ApprovarRoleEnum.ITDepartment.ToString();
                        }
                    }
                    else
                    {
                        model.ApproverRole = ApprovarRoleEnum.NextApprover.ToString();
                    }

                    if (Session["Role"].ToString() == "Initiator")
                    {
                        vendor.InitiatorApproval = "A";

                        var nextApprover = dataContext.tbl_Users.FirstOrDefault(x => x.Id == Utility.UserId)?.HANEXT;
                        vendor.NextApprover = !string.IsNullOrEmpty(nextApprover) ? nextApprover : string.IsNullOrEmpty(vendor.CIN_No) ? ApprovarRoleEnum.FinanceDepartment.ToString() : ApprovarRoleEnum.LegalDepartment.ToString();
                    }
                    else if (model.ApproverRole != ApprovarRoleEnum.NextApprover.ToString())
                    {
                        if (model.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())
                        {
                            vendor.LegalDepartmentApproval = "A";
                            vendor.NextApprover = ApprovarRoleEnum.FinanceDepartment.ToString();
                        }
                        else if (model.ApproverRole == ApprovarRoleEnum.FinanceDepartment.ToString())
                        {
                            vendor.FinanceDepartmentApproval = "A";
                            vendor.NextApprover = ApprovarRoleEnum.ITDepartment.ToString();
                        }
                        else if (model.ApproverRole == ApprovarRoleEnum.ITDepartment.ToString())
                        {
                            vendor.ITDepartmentApproval = "A";
                            vendor.NextApprover = null;
                        }
                    }
                    else
                    {
                        var nextApprover = dataContext.tbl_Users.FirstOrDefault(x => x.Id == Utility.UserId)?.HANEXT;
                        vendor.NextApprover = !string.IsNullOrEmpty(nextApprover) ? nextApprover : string.IsNullOrEmpty(vendor.CIN_No) ? ApprovarRoleEnum.FinanceDepartment.ToString() : ApprovarRoleEnum.LegalDepartment.ToString();
                    }

                    vendor.VendorApprovals.Add(model);

                    if (model.ApproverRole == ApprovarRoleEnum.ITDepartment.ToString())
                    {
                        if (vendor.IsNewVendor)
                        {
                            if (vendor.Nature_of_service == "1")
                            {
                                vendor.VendorCode = (dataContext.Vend_reg_tbl.Where(x => x.Nature_of_service == "1" && x.VendorCode != null).Select(x => x.VendorCode).OrderByDescending(x => x).FirstOrDefault() ?? 53001001) + 1000;
                            }
                            else if (vendor.Nature_of_service == "2" || vendor.Nature_of_service == "3")
                            {
                                vendor.VendorCode = (dataContext.Vend_reg_tbl.Where(x => (x.Nature_of_service == "2" || x.Nature_of_service == "3") && x.VendorCode != null).Select(x => x.VendorCode).OrderByDescending(x => x).FirstOrDefault() ?? 12001001) + 1000;
                            }
                            else if (vendor.Nature_of_service == "4")
                            {
                                vendor.VendorCode = (dataContext.Vend_reg_tbl.Where(x => x.Nature_of_service == "4" && x.VendorCode != null).Select(x => x.VendorCode).OrderByDescending(x => x).FirstOrDefault() ?? 21001001) + 1000;
                            }
                            else if (vendor.Nature_of_service == "5")
                            {
                                vendor.VendorCode = (dataContext.Vend_reg_tbl.Where(x => x.Nature_of_service == "5" && x.VendorCode != null).Select(x => x.VendorCode).OrderByDescending(x => x).FirstOrDefault() ?? 91001001) + 1000;
                            }
                        }

                        vendor.IsFinalApproved = true;
                    }

                    if (model.TermsCodeId != null)
                    {
                        vendor.TermsCode = dataContext.PaymentTermsMasters.FirstOrDefault(x => x.PTerms_ID == model.TermsCodeId)?.PTerms_Code;
                        vendor.BankCode = model.BankCode;
                        vendor.BankBranch = model.BankBranch;
                        vendor.PaymentType = dataContext.PayTypeMasters.FirstOrDefault(x => x.PayType_ID == model.PaymentTypeId)?.PayType_Code;
                        vendor.TaxCode = dataContext.LX_TaxCode.FirstOrDefault(x => x.Id == model.TaxCodeId)?.ItemTaxCDE;
                    }

                    if (model.VendorTypeId != null)
                    {
                        vendor.Company = model.Company;
                        vendor.VendorType = dataContext.VendorTypeMasters.FirstOrDefault(x => x.VT_ID == model.VendorTypeId)?.VendorType;
                        vendor.Currency = dataContext.VendorTypeMasters.FirstOrDefault(x => x.VT_ID == model.VendorTypeId)?.Currency;
                        vendor.DocumentPfx = model.DocumentPfx;
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
                            string subject = "Vendor approval details";

                            var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\VendorApproval.html"));
                            string body = htmlContent.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/VendorLogin/{model.VendorId}");
                            body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                            body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                            string displayName = string.Empty;
                            string attachments = string.Empty;
                            Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, Utility.UserId, EmailTypeEnum.Vendor, model.VendorId);
                        }
                        else
                        {
                            var roleName = string.IsNullOrEmpty(vendor.CIN_No) ? ApprovarRoleEnum.FinanceDepartment.ToString() : ApprovarRoleEnum.LegalDepartment.ToString();
                            var roles = dataContext.webpages_Roles.FirstOrDefault(x => x.RoleName == roleName);
                            var emails = roles.tbl_Users.Select(x => x.EmailAddress).ToList();

                            string mailTo = string.Join(",", emails);
                            string CC = string.Empty;
                            string BCC = string.Empty;
                            string subject = "Vendor approval details";

                            var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\VendorApproval.html"));
                            string body = htmlContent.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/VendorLogin/{model.VendorId}");
                            body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                            body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                            string displayName = string.Empty;
                            string attachments = string.Empty;
                            Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, Utility.UserId, EmailTypeEnum.Vendor, model.VendorId);
                        }
                    }
                    else
                    {
                        var nextApproverRole = model.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString() ? ApprovarRoleEnum.FinanceDepartment.ToString()
                                : ApprovarRoleEnum.ITDepartment.ToString();

                        if (!string.IsNullOrEmpty(nextApproverRole))
                        {
                            var roles = dataContext.webpages_Roles.FirstOrDefault(x => x.RoleName == nextApproverRole);
                            var emails = roles.tbl_Users.Select(x => x.EmailAddress).ToList();

                            string mailTo = string.Join(",", emails);
                            string CC = string.Empty;
                            string BCC = string.Empty;
                            string subject = "Vendor approval details";

                            var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\VendorApproval.html"));
                            string body = htmlContent.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/VendorLogin/{model.VendorId}");
                            body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                            body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                            string displayName = string.Empty;
                            string attachments = string.Empty;
                            Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, Utility.UserId, EmailTypeEnum.Vendor, model.VendorId);
                        }
                    }

                    string mailTo1 = $"{Utility.UserCode},{vendor.tbl_Users.EmailAddress}";
                    string CC1 = string.Empty;
                    string BCC1 = string.Empty;
                    string subject1 = "Vendor approval details";

                    var htmlContent1 = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\VendorApproved.html"));
                    string body1 = htmlContent1.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/VendorLogin/{model.VendorId}");
                    body1 = body1.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                    body1 = body1.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                    string displayName1 = string.Empty;
                    string attachments1 = string.Empty;
                    Utility.SendMail(mailTo1, CC1, BCC1, subject1, body1, displayName1, attachments1, true, Utility.UserId, EmailTypeEnum.Vendor, model.VendorId);

                    return Json(new { status = true });
                }

                return Json(new { status = false, result = "Vendor not exists in system." });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, result = "Some error occured, please try again." });
            }
        }

        public JsonResult RejectVendorDetails(int id, string remarks)
        {
            try
            {
                var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == id);
                if (vendor != null)
                {
                    var vendorApprover = vendor.VendorApprovals.Where(x => !x.IsDeleted && x.VendorId == id).OrderByDescending(x => x.CreatedByDate).FirstOrDefault();

                    foreach (var item in vendor.VendorApprovals)
                    {
                        item.IsDeleted = true;
                    }

                    var data = new VendorApproval()
                    {
                        VendorId = id,
                        Status = VendorApprovalEnum.Rejected.ToString(),
                        Remarks = remarks,
                        IsDeleted = true,
                        CreatedById = Utility.UserId,
                        CreatedByDate = DateTime.Now
                    };

                    if (vendorApprover != null)
                    {
                        if (vendorApprover.ApproverRole == ApprovarRoleEnum.NextApprover.ToString())
                        {
                            data.ApproverRole = (vendor.VendorApprovals.Count(x => !x.IsDeleted && x.VendorId == id) == 1 && !string.IsNullOrEmpty(vendor.NextApprover)) || !string.IsNullOrEmpty(vendorApprover.tbl_Users.HANEXT) ? ApprovarRoleEnum.NextApprover.ToString() : string.IsNullOrEmpty(vendor.CIN_No) ? ApprovarRoleEnum.FinanceDepartment.ToString() : ApprovarRoleEnum.LegalDepartment.ToString();
                        }
                        else
                        {
                            data.ApproverRole = vendorApprover.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString() ? ApprovarRoleEnum.FinanceDepartment.ToString()
                                : ApprovarRoleEnum.ITDepartment.ToString();
                        }
                    }
                    else
                    {
                        data.ApproverRole = ApprovarRoleEnum.NextApprover.ToString();
                    }

                    if (Session["Role"].ToString() == "Initiator")
                    {
                        vendor.InitiatorApproval = "R";
                    }
                    else
                    {
                        if (data.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())
                        {
                            vendor.LegalDepartmentApproval = "R";
                        }
                        else if (data.ApproverRole == ApprovarRoleEnum.FinanceDepartment.ToString())
                        {
                            vendor.FinanceDepartmentApproval = "R";
                        }
                        else if (data.ApproverRole == ApprovarRoleEnum.ITDepartment.ToString())
                        {
                            vendor.ITDepartmentApproval = "R";
                        }
                    }

                    vendor.VendorApprovals.Add(data);
                    vendor.NextApprover = null;

                    dataContext.SaveChanges();

                    string mailTo1 = $"{Utility.UserCode},{vendor.tbl_Users.EmailAddress}";

                    if (Session["Role"].ToString() == "Initiator")
                    {
                        mailTo1 += $",{vendor.Email}";
                    }

                    string CC1 = string.Empty;
                    string BCC1 = string.Empty;
                    string subject1 = "Vendor approval details";

                    var htmlContent1 = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\VendorRejected.html"));
                    string body1 = htmlContent1.Replace("[URL]", $"{ConfigurationManager.AppSettings["SiteUrl"].ToString()}/Account/VendorLogin/{id}");
                    body1 = body1.Replace("[REMARKS]", remarks);
                    body1 = body1.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                    body1 = body1.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                    string displayName1 = string.Empty;
                    string attachments1 = string.Empty;
                    Utility.SendMail(mailTo1, CC1, BCC1, subject1, body1, displayName1, attachments1, true, Utility.UserId, EmailTypeEnum.Vendor, id);

                    return Json(new { status = true });
                }

                return Json(new { status = false, result = "Vendor not exists in system." });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, result = "Some error occured, please try again." });
            }
        }

        public ActionResult GetVendor()
        {
            var result = new JsonResult();
            try
            {
                var user = dataContext.tbl_Users.FirstOrDefault(x => x.Id == Utility.UserId);
                var userCode = user?.HAUSER;
                var deptCode = user?.Dept_Code;

                var vendorIds = new List<int>();
                if (Session["Role"].ToString() == "LegalDepartment" || Session["Role"].ToString() == "FinanceDepartment" || Session["Role"].ToString() == "ITDepartment" || Session["Role"].ToString() == "Admin")
                {
                    vendorIds = dataContext.Vend_reg_tbl.Select(x => x.ID).ToList();
                }
                else
                {
                    var userIds = new List<int> { Utility.UserId };
                    userIds.AddRange(dataContext.tbl_Users.Where(x => x.Dept_Code == deptCode).Select(x => x.Id).ToList());

                    vendorIds = dataContext.Vend_reg_tbl.Where(x => userIds.Contains(x.CreatedById) || x.NextApprover == userCode || x.Email == Utility.UserCode).Select(x => x.ID).ToList();
                    vendorIds.AddRange(dataContext.VendorApprovals.Where(x => x.CreatedById == Utility.UserId).Select(x => x.VendorId).ToList());
                }

                var data = dataContext.Vend_reg_tbl.Where(x => vendorIds.Contains(x.ID)).ToList();
                if (data.Any(x => x.Email == Utility.UserCode))
                {
                    data = data.Where(x => x.Email == Utility.UserCode).ToList();
                }

                var vendorApprovers = dataContext.VendorApprovals.Where(x => !x.IsDeleted).ToList();
                var vendors = new List<VendorListModel>();

                data.ForEach(item =>
                {
                    var documents = new List<string>();
                    if (item.Step4 == true)
                    {
                        foreach (var document in item.VendorFiles)
                        {
                            documents.Add($"<a href='/Vendors/Download/{item.ID}?fileType={document.FileUploadType}' target='_blank'>{document.FileUploadType}</a>");
                        }
                    }

                    vendors.Add(new VendorListModel()
                    {
                        Id = item.ID,
                        Email = item.Email,
                        vend_name = item.vend_name,
                        NewExistingVendor = item.IsNewVendor ? "New" : "Existing",
                        VendorCode = item.VendorCode?.ToString(),
                        Status = Utility.UserId == 0 ? (item.IsFinalApproved ? "Approved" : (!item.IsOpened && item.Step4 == true && item.NextApprover == null ? "Rejected" : "Pending")) : vendorApprovers.Any(x => x.VendorId == item.ID && x.CreatedById == Utility.UserId) ? "Approved" : (item.IsFinalApproved ? "Approved" : (!item.IsOpened && item.Step4 == true && item.NextApprover == null ? "Rejected" : "Pending")),
                        Owner = item.tbl_Users.HANAME,
                        Documents = string.Join(" | ", documents),
                        NextApprover = item.NextApprover,
                        PreviousApprover = $"{vendorApprovers.Where(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.tbl_Users.HANAME} ({vendorApprovers.Where(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.NextApprover.ToString()).OrderByDescending(x => x.CreatedByDate).FirstOrDefault()?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        LegalDepartment = string.IsNullOrEmpty(item.CIN_No) || !item.IsNewVendor ? (item.Step4 == true ? "Legal Department not required" : "") : $"{vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())?.tbl_Users.HANAME} ({vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.LegalDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        FinanceDepartment = $"{vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.FinanceDepartment.ToString())?.tbl_Users.HANAME} ({vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.FinanceDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})",
                        ITDepartment = $"{vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.ITDepartment.ToString())?.tbl_Users.HANAME} ({vendorApprovers.FirstOrDefault(x => x.VendorId == item.ID && x.ApproverRole == ApprovarRoleEnum.ITDepartment.ToString())?.CreatedByDate.ToString("dd-MM-yyyy hh:mm tt")})"
                    });
                });

                result = this.Json(new
                {
                    draw = Convert.ToInt32(Request.Form.GetValues("draw")[0]),
                    recordsTotal = (vendors.Count > 0) ? vendors.Count : 0,
                    recordsFiltered = (vendors.Count > 0) ? vendors.Count : 0,
                    data = vendors.OrderByDescending(x => x.Status).ToList()
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}