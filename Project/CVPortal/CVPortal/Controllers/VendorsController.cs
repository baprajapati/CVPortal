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
                return RedirectToAction("../Account/VendorCustomerLogin/" + id);

            var model = new VendorStep1();
            var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == id);
            if (vendor != null)
            {
                if (Utility.UserCode.Equals(vendor.Email) && vendor.Step4 == true)
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
                model.Address1CountryId = vendor.Address1CountryId;
                model.Address1StateId = vendor.Address1StateId;
                model.Address1CityId = vendor.Address1CityId;
                model.Address2 = vendor.Address2;
                model.Address2CountryId = vendor.Address2CountryId;
                model.Address2StateId = vendor.Address2StateId;
                model.Address2CityId = vendor.Address2CityId;
                model.IsMain = Utility.UserCode.Equals(vendor.Email);
            }
            else
            {
                return RedirectToAction("../Account/VendorCustomerLogin/" + id);
            }

            SetDroddownData(model);
            ViewBag.Id = id;
            return View(model);
        }

        public ActionResult VendorStep2(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/VendorCustomerLogin/" + id);

            var model = new VendorStep2();
            var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == id);
            if (vendor != null)
            {
                if (Utility.UserCode.Equals(vendor.Email) && vendor.Step4 == true)
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
                model.IsMain = Utility.UserCode.Equals(vendor.Email);

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
                return RedirectToAction("../Account/VendorCustomerLogin/" + id);
            }

            ViewBag.Id = id;
            return View(model);
        }

        public ActionResult VendorStep3(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/VendorCustomerLogin/" + id);

            var model = new VendorStep3();
            var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == id);
            if (vendor != null)
            {
                if (Utility.UserCode.Equals(vendor.Email) && vendor.Step4 == true)
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
                model.IsMain = Utility.UserCode.Equals(vendor.Email);

                var bankFile = vendor.VendorFiles.FirstOrDefault(x => x.FileUploadType == FileUploadEnum.Bank.ToString());
                if (bankFile != null)
                {
                    model.BankFileName = bankFile.Name;
                }
            }
            else
            {
                return RedirectToAction("../Account/VendorCustomerLogin/" + id);
            }

            ViewBag.Id = id;
            return View(model);
        }

        public ActionResult VendorStep4(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/VendorCustomerLogin/" + id);

            var model = new VendorStep4();
            var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == id);
            if (vendor != null)
            {
                if (Utility.UserCode.Equals(vendor.Email) && vendor.Step4 == true)
                {
                    return RedirectToAction("FinalForm", new { id });
                }

                model.Id = vendor.ID;
                model.Type_of_Vend = vendor.Type_of_Vend;
                model.IsMain = Utility.UserCode.Equals(vendor.Email);

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
                return RedirectToAction("../Account/VendorCustomerLogin/" + id);
            }

            ViewBag.Id = id;
            return View(model);
        }

        public ActionResult FinalForm(int? id)
        {
            if (id == null)
                return RedirectToAction("../Account/Login/");

            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../Account/VendorCustomerLogin/" + id);

            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public ActionResult VendorStep1(VendorStep1 model)
        {
            //var subject = "Vendor Form Submitted";
            //var body = "https://localhost:44318/Vendor/Index/1";

            //Utility.SendMail("baprajapati2444@gmail.com", null, null, subject, body, "Vendor Form Submitted", null, true);
            if (model.IsMain)
            {
                if (ModelState.IsValid)
                {
                    var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == model.Id);
                    if (vendor != null)
                    {
                        vendor.Org_Sts = model.Org_Sts;
                        vendor.vend_name = model.vend_name;
                        vendor.CEO_name = model.CEO_name;
                        vendor.Designation = model.Designation;
                        vendor.Contact_no = model.Contact_no;
                        vendor.Address1 = model.Address1;
                        vendor.Address1CountryId = model.Address1CountryId;
                        vendor.Address1StateId = model.Address1StateId;
                        vendor.Address1CityId = model.Address1CityId;
                        vendor.Address2 = model.Address2;
                        vendor.Address2CountryId = model.Address2CountryId;
                        vendor.Address2StateId = model.Address2StateId;
                        vendor.Address2CityId = model.Address2CityId;
                        vendor.Step1 = true;
                        dataContext.SaveChanges();
                    }

                    return RedirectToAction("VendorStep2", new { id = model.Id });
                }

                SetDroddownData(model);
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
                if (ModelState.IsValid)
                {
                    var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == model.Id);
                    if (vendor != null)
                    {
                        if (model.CINFile != null && model.CINFile.ContentLength > 0)
                        {
                            var fileName = $"{Path.GetFileNameWithoutExtension(model.CINFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.CINFile.FileName)}";
                            var path = Server.MapPath($"~/Content/FileUpload/Vendor/{model.Id}");

                            Directory.CreateDirectory(path);

                            path = Path.Combine(path, fileName);
                            model.CINFile.SaveAs(path);

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

                        if (model.PANFile != null && model.PANFile.ContentLength > 0)
                        {
                            var fileName = $"{Path.GetFileNameWithoutExtension(model.PANFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.PANFile.FileName)}";
                            var path = Server.MapPath($"~/Content/FileUpload/Vendor/{model.Id}");

                            Directory.CreateDirectory(path);

                            path = Path.Combine(path, fileName); model.PANFile.SaveAs(path);
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

                        if (model.GSTFile != null && model.GSTFile.ContentLength > 0)
                        {
                            var fileName = $"{Path.GetFileNameWithoutExtension(model.GSTFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.GSTFile.FileName)}";
                            var path = Server.MapPath($"~/Content/FileUpload/Vendor/{model.Id}");

                            Directory.CreateDirectory(path);

                            path = Path.Combine(path, fileName);
                            model.GSTFile.SaveAs(path);

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

                        if (model.MSMEFile != null && model.MSMEFile.ContentLength > 0)
                        {
                            var fileName = $"{Path.GetFileNameWithoutExtension(model.MSMEFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.MSMEFile.FileName)}";
                            var path = Server.MapPath($"~/Content/FileUpload/Vendor/{model.Id}");

                            Directory.CreateDirectory(path);

                            path = Path.Combine(path, fileName);
                            model.MSMEFile.SaveAs(path);

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
            if (model.IsMain)
            {
                if (ModelState.IsValid)
                {
                    var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == model.Id);
                    if (vendor != null)
                    {
                        if (model.BankFile != null && model.BankFile.ContentLength > 0)
                        {
                            var fileName = $"{Path.GetFileNameWithoutExtension(model.BankFile.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{Path.GetExtension(model.BankFile.FileName)}";
                            var path = Server.MapPath($"~/Content/FileUpload/Vendor/{model.Id}");

                            Directory.CreateDirectory(path);

                            path = Path.Combine(path, fileName);
                            model.BankFile.SaveAs(path);

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

                        vendor.Benificiary_name = model.Benificiary_name;
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
            if (ModelState.IsValid)
            {
                var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.ID == model.Id);
                if (vendor != null)
                {
                    if (model.AuditedFile != null && model.AuditedFile.ContentLength > 0)
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

                    if (model.MOAFile != null && model.MOAFile.ContentLength > 0)
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

                    vendor.Type_of_Vend = model.Type_of_Vend;
                    vendor.Step4 = true;
                    dataContext.SaveChanges();
                }

                return RedirectToAction("FinalForm", new { id = model.Id });
            }

            return View(model);
        }

        [HttpGet]
        public FileResult Download(int id, string fileName)
        {
            return File(Server.MapPath($"~/Content/FileUpload/Vendor/{id}/{fileName}"), "application/pdf");
        }

        [HttpGet]
        public JsonResult StateDetails(int CountryId)
        {

            try
            {
                return Json(dataContext.States.Where(x => x.country_id == CountryId).Select(x => new StateViewModel() { id = x.id, name = x.name }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult CityDetails(int stateId)
        {
            try
            {
                return Json(dataContext.Cities.Where(x => x.state_id == stateId).Select(x => new CityViewModel() { id = x.id, name = x.name }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetDroddownData(VendorStep1 model)
        {
            var country1 = new List<SelectListItem>() {
                new SelectListItem()
                {
                    Text = "Select Country",
                    Value = ""
                }
            };

            dataContext.Countries.ToList().ForEach(item =>
            {
                country1.Add(new SelectListItem()
                {
                    Text = item.name,
                    Value = item.id.ToString(),
                    Selected = item.id == model.Address1CountryId
                });
            });

            var state1 = new List<SelectListItem>() {
                new SelectListItem()
                {
                    Text = "Select State",
                    Value = ""
                }
            };

            dataContext.States.Where(x => x.country_id == model.Address1CountryId).ToList().ForEach(item =>
            {
                state1.Add(new SelectListItem()
                {
                    Text = item.name,
                    Value = item.id.ToString(),
                    Selected = item.id == model.Address1StateId
                });
            });

            var city1 = new List<SelectListItem>() {
                new SelectListItem()
                {
                    Text = "Select State",
                    Value = ""
                }
            };

            dataContext.Cities.Where(x => x.state_id == model.Address1StateId).ToList().ForEach(item =>
            {
                city1.Add(new SelectListItem()
                {
                    Text = item.name,
                    Value = item.id.ToString(),
                    Selected = item.id == model.Address1CityId
                });
            });

            var country2 = new List<SelectListItem>() {
                new SelectListItem()
                {
                    Text = "Select Country",
                    Value = ""
                }
            };

            dataContext.Countries.ToList().ForEach(item =>
            {
                country2.Add(new SelectListItem()
                {
                    Text = item.name,
                    Value = item.id.ToString(),
                    Selected = item.id == model.Address2CountryId
                });
            });

            var state2 = new List<SelectListItem>() {
                new SelectListItem()
                {
                    Text = "Select State",
                    Value = ""
                }
            };

            dataContext.States.Where(x => x.country_id == model.Address2CountryId).ToList().ForEach(item =>
            {
                state2.Add(new SelectListItem()
                {
                    Text = item.name,
                    Value = item.id.ToString(),
                    Selected = item.id == model.Address2StateId
                });
            });

            var city2 = new List<SelectListItem>() {
                new SelectListItem()
                {
                    Text = "Select State",
                    Value = ""
                }
            };

            dataContext.Cities.Where(x => x.state_id == model.Address2StateId).ToList().ForEach(item =>
            {
                city2.Add(new SelectListItem()
                {
                    Text = item.name,
                    Value = item.id.ToString(),
                    Selected = item.id == model.Address2CityId
                });
            });

            ViewBag.Country1 = country1;
            ViewBag.State1 = state1;
            ViewBag.City1 = city1;

            ViewBag.Country2 = country2;
            ViewBag.State2 = state2;
            ViewBag.City2 = city2;
        }
    }
}