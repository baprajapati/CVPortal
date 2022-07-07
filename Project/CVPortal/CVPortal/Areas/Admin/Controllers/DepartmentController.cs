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
                    var data = dataContext.Lx_GSV.FirstOrDefault(x => x.GSV_ID == id);
                    if (data != null)
                    {
                        department = new DepartmentViewModel()
                        {
                            Seg_ID = data.Seg_ID,
                            Dept_Code = data.Dept_Code,
                            Dept_Desc = data.Dept_Desc
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
                    var dataName = dataContext.Lx_GSV.FirstOrDefault(x => x.Dept_Desc == departmentViewModel.Dept_Desc);
                    if (dataName != null)
                    {
                        ModelState.AddModelError(nameof(departmentViewModel.Dept_Desc), "Description already exist.");
                        return View(departmentViewModel);
                    }

                    var dataCode = dataContext.Lx_GSV.FirstOrDefault(x => x.Dept_Code == departmentViewModel.Dept_Code);
                    if (dataCode != null)
                    {
                        ModelState.AddModelError(nameof(departmentViewModel.Dept_Code), "Code already exist.");
                        return View(departmentViewModel);
                    }

                    var department = new Lx_GSV()
                    {
                        Seg_ID = departmentViewModel.Seg_ID,
                        Dept_Code = departmentViewModel.Dept_Code,
                        Dept_Desc = departmentViewModel.Dept_Desc,
                        IsActive = true,
                        CreatedById = WebSecurity.CurrentUserId,
                        CreatedByDate = DateTime.Now
                    };
                    dataContext.Lx_GSV.Add(department);
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
                var data = dataContext.Lx_GSV.ToList();

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

        [HttpGet]
        public JsonResult ActiveDeactiveDepartment(int id, bool status)
        {
            try
            {
                var objUser = dataContext.Lx_GSV.FirstOrDefault(x => x.GSV_ID == id);
                if (objUser != null)
                {
                    objUser.IsActive = status;
                    dataContext.SaveChanges();
                }

                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, result = ex.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
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
                    var dataCode = dataContext.Lx_GSV.FirstOrDefault(x => x.GSV_ID != departmentViewModel.Id && x.Dept_Code == departmentViewModel.Dept_Code);
                    if (dataCode != null)
                    {
                        ModelState.AddModelError(nameof(departmentViewModel.Dept_Code), "Code already exist.");
                        return View(departmentViewModel);
                    }

                    var dataName = dataContext.Lx_GSV.FirstOrDefault(x => x.GSV_ID != departmentViewModel.Id && x.Dept_Desc == departmentViewModel.Dept_Desc);
                    if (dataName != null)
                    {
                        ModelState.AddModelError(nameof(departmentViewModel.Dept_Desc), "Description already exist.");
                        return View(departmentViewModel);
                    }

                    var data = dataContext.Lx_GSV.FirstOrDefault(x => x.GSV_ID == departmentViewModel.Id);
                    if (data != null)
                    {
                        data.Seg_ID = departmentViewModel.Seg_ID;
                        data.Dept_Code = departmentViewModel.Dept_Code;
                        data.Dept_Desc = departmentViewModel.Dept_Desc;
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