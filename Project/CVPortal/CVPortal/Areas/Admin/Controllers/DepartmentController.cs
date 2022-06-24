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

namespace CVPortal.Areas.Admin.Controllers
{
    public class DepartmentController : Controller
    {
        CVPortalEntities dataContext = new CVPortalEntities();

        public DepartmentController()
        {
        }

        [HttpGet]
        public ActionResult AddDepartment()
        {
            try
            {
                if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                    return RedirectToAction("../../Account/Login");

                return View(new DepartmentViewModel());
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult EditDepartment(int? id)
        {
            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../../Account/Login");

            var department = new DepartmentViewModel();
            
            try
            {
                if (id != null)
                {
                    var data = dataContext.DepartmentMasters.FirstOrDefault(x => x.Id == id);
                    if (data != null)
                    {
                        department = new DepartmentViewModel()
                        {
                            Name = data.Name,
                            Code = data.Code,
                            Description = data.Description
                        };
                    }
                    else
                    {
                        return RedirectToAction("AddDepartment", "Department");
                    }
                }
                else
                    return RedirectToAction("AddDepartment", "Department");
            }
            catch (Exception)
            {
            }

            return View(department);
        }

        public ActionResult DepartmentIndex()
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
        public ActionResult AddDepartment(DepartmentViewModel departmentViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dataName = dataContext.DepartmentMasters.FirstOrDefault(x => x.Name == departmentViewModel.Name);
                    if (dataName != null)
                    {
                        ModelState.AddModelError(nameof(departmentViewModel.Name), "Name already exist.");
                        return View(departmentViewModel);
                    }

                    var dataCode = dataContext.DepartmentMasters.FirstOrDefault(x => x.Code == departmentViewModel.Code);
                    if (dataCode != null)
                    {
                        ModelState.AddModelError(nameof(departmentViewModel.Code), "Code already exist.");
                        return View(departmentViewModel);
                    }

                    var department = new DepartmentMaster()
                    {
                        Name = departmentViewModel.Name,
                        Code = departmentViewModel.Code,
                        Description = departmentViewModel.Description,
                        CreatedById = WebSecurity.CurrentUserId,
                        CreatedByDate = DateTime.Now
                    };
                    dataContext.DepartmentMasters.Add(department);
                    dataContext.SaveChanges();

                    return RedirectToAction("DepartmentIndex");
                }
            }
            catch (Exception)
            {
            }

            return View(departmentViewModel);
        }

        public ActionResult GetDepartment()
        {
            var result = new JsonResult();
            try
            {
                var data = dataContext.DepartmentMasters.ToList();

                result = this.Json(new
                {
                    draw = Convert.ToInt32(Request.Form.GetValues("draw")[0]),
                    recordsTotal = (data.Count > 0) ? data.Count : 0,
                    recordsFiltered = (data.Count > 0) ? data.Count : 0,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
            }

            return result;
        }

        public JsonResult DeleteDepartment(int id)
        {
            try
            {
                var objDepartment = dataContext.DepartmentMasters.FirstOrDefault(x => x.Id == id);
                if (objDepartment != null)
                {
                    var objUser = dataContext.tbl_Users.FirstOrDefault(x => x.Dept_Code == objDepartment.Code);
                    if(objUser != null)
                    {
                        return Json(new { status = false, result = "Department used in user, so you can't delete this department" });
                    }

                    dataContext.DepartmentMasters.Remove(objDepartment);
                    dataContext.SaveChanges();
                }

                return Json(new { status = true });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, result = ex.GetBaseException().Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDepartment(DepartmentViewModel departmentViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dataCode = dataContext.DepartmentMasters.FirstOrDefault(x => x.Id != departmentViewModel.Id && x.Code == departmentViewModel.Code);
                    if (dataCode != null)
                    {
                        ModelState.AddModelError(nameof(departmentViewModel.Code), "Code already exist.");
                        return View(departmentViewModel);
                    }

                    var dataName = dataContext.DepartmentMasters.FirstOrDefault(x => x.Id != departmentViewModel.Id && x.Name == departmentViewModel.Name);
                    if (dataName != null)
                    {
                        ModelState.AddModelError(nameof(departmentViewModel.Name), "Name already exist.");
                        return View(departmentViewModel);
                    }

                    var data = dataContext.DepartmentMasters.FirstOrDefault(x => x.Id == departmentViewModel.Id);
                    if (data != null)
                    {
                        data.Code = departmentViewModel.Code;
                        data.Name = departmentViewModel.Name;
                        data.Description = departmentViewModel.Description;
                        dataContext.SaveChanges();
                    }

                    return RedirectToAction("DepartmentIndex");
                }
            }
            catch (Exception)
            {
            }

            return View(departmentViewModel);
        }
    }
}