using CVPortal.App_Code;
using CVPortal.Models;
using CVPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace CVPortal.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        CVPortalEntities dataContext = new CVPortalEntities();

        public UserController()
        {
        }

        public SelectList GetRoles(string[] selectedValue)
        {
            var roles = dataContext.webpages_Roles.Where(x => x.RoleName != "Admin").ToList();

            List<SelectListItem> list = new List<SelectListItem>();

            roles.ForEach(item =>
            {
                list.Add(new SelectListItem()
                {
                    Text = item.RoleName,
                    Value = item.RoleName,
                    Selected = selectedValue != null && selectedValue.Contains(item.RoleName)
                });
            });

            return new SelectList(list, "Value", "Text");
        }

        public SelectList GetDepartments(string[] selectedValue)
        {
            var departments = dataContext.Lx_GSV.Where(x => x.IsActive).ToList();

            List<SelectListItem> list = new List<SelectListItem>();

            departments.ForEach(item =>
            {
                list.Add(new SelectListItem()
                {
                    Text = $"{item.Dept_Code} - {item.Dept_Desc}",
                    Value = item.Dept_Code,
                    Selected = selectedValue != null && selectedValue.Contains(item.Dept_Code)
                });
            });

            return new SelectList(list, "Value", "Text");
        }

        public SelectList GetUsers(string[] selectedValue, string currentUser)
        {
            var users = dataContext.tbl_Users.Where(x => x.EmailAddress != "admin@gmail.com").ToList();

            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "-- Select --",
                    Value = ""
                }
            };

            if (!string.IsNullOrEmpty(currentUser))
            {
                users = users.Where(x => x.HAUSER != currentUser).ToList();
            }

            users.ForEach(item =>
            {
                list.Add(new SelectListItem()
                {
                    Text = $"{item.HAUSER} - {item.HANAME}",
                    Value = item.HAUSER,
                    Selected = selectedValue != null && selectedValue.Contains(item.EmailAddress)
                });
            });

            return new SelectList(list, "Value", "Text");
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            try
            {
                if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                    return RedirectToAction("../../Account/Login");

                ViewBag.RoleList = GetRoles(null);
                ViewBag.DepartmentList = GetDepartments(null);
                ViewBag.UserList = GetUsers(null, null);

                return View(new UserViewModel() { RoleName = "InitiatorAdmin" });
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult EditUser(int? id)
        {
            if (Utility.UserCode == null || string.IsNullOrEmpty(Utility.UserCode.ToString()))
                return RedirectToAction("../../Account/Login");

            var user = new UserViewModel();

            try
            {
                if (id != null)
                {
                    var data = dataContext.tbl_Users.FirstOrDefault(x => x.Id == id);
                    if (data != null)
                    {
                        var roleName = Roles.GetRolesForUser(data.EmailAddress).First().ToString();

                        ViewBag.UserList = GetUsers(new string[] { data.HANEXT }, data.HAUSER);
                        ViewBag.RoleList = GetRoles(new string[] { roleName });
                        ViewBag.DepartmentList = GetDepartments(new string[] { data.Dept_Code });

                        user = new UserViewModel()
                        {
                            Email = data.EmailAddress,
                            Password = data.Password,
                            RoleName = roleName,
                            HAUSER = data.HAUSER,
                            HANAME = data.HANAME,
                            Dept_Code = data.Dept_Code,
                            HANEXT = data.HANEXT
                        };
                    }
                    else
                    {
                        return RedirectToAction("AddUser", "User");
                    }
                }
                else
                    return RedirectToAction("AddUser", "User");

            }
            catch (Exception)
            {
            }

            return View(user);
        }

        public ActionResult UserIndex()
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
        public ActionResult AddUser(UserViewModel user)
        {
            try
            {
                ViewBag.RoleList = GetRoles(new string[] { user.RoleName });
                ViewBag.UserList = GetUsers(new string[] { user.HANEXT }, null);
                ViewBag.DepartmentList = GetDepartments(new string[] { user.Dept_Code });

                if (ModelState.IsValid)
                {
                    var dataEmail = dataContext.tbl_Users.FirstOrDefault(x => x.EmailAddress == user.Email);
                    if (dataEmail != null)
                    {
                        ModelState.AddModelError(nameof(user.Email), "Email already exist.");
                        return View(user);
                    }

                    var dataUserCode = dataContext.tbl_Users.FirstOrDefault(x => x.HAUSER == user.HAUSER);
                    if (dataUserCode != null)
                    {
                        ModelState.AddModelError(nameof(user.HAUSER), "User Code already exist.");
                        return View(user);
                    }

                    var vendor = dataContext.Vend_reg_tbl.FirstOrDefault(x => x.Email == user.Email);
                    if (vendor != null)
                    {
                        ModelState.AddModelError(nameof(user.Email), "Email already exist.");
                        return View(user);
                    }

                    string token = WebSecurity.CreateUserAndAccount(user.Email, user.Password, new
                    {
                        HAUSER = user.HAUSER,
                        HANEXT = user.HANEXT,
                        HANAME = user.HANAME,
                        Password = user.Password,
                        Dept_Code = user.Dept_Code
                    });
                    Roles.AddUserToRole(user.Email, user.RoleName);

                    string mailTo = user.Email;
                    string CC = string.Empty;
                    string BCC = string.Empty;
                    string subject = "Your Login details";

                    var htmlContent = System.IO.File.ReadAllText(Server.MapPath("\\Content\\EmailTemplate\\SendPassword.html"));
                    string body = htmlContent.Replace("[Password]", !string.IsNullOrEmpty(user.Password) ? $"Password :<b>{user.Password}</b>" : string.Empty);
                    body = body.Replace("[SITEURL]", ConfigurationManager.AppSettings["SiteUrl"].ToString());
                    body = body.Replace("[SITENAME]", ConfigurationManager.AppSettings["SiteName"].ToString());

                    string displayName = string.Empty;
                    string attachments = string.Empty;

                    var entityId = dataContext.tbl_Users.FirstOrDefault(x => x.EmailAddress == user.Email)?.Id;
                    Utility.SendMail(mailTo, CC, BCC, subject, body, displayName, attachments, true, WebSecurity.CurrentUserId, EmailTypeEnum.User, entityId);

                    return RedirectToAction("UserIndex");
                }
            }
            catch (Exception)
            {
            }

            return View(user);
        }

        public ActionResult GetUser(UserViewModel model)
        {
            var result = new JsonResult();
            try
            {
                var data = new List<tbl_Users>();

                if (!string.IsNullOrEmpty(model.Email))
                {
                    data = dataContext.tbl_Users.Where(x => x.EmailAddress != null && x.EmailAddress.ToLower().Contains(model.Email.ToLower()) && x.Id != WebSecurity.CurrentUserId).ToList();
                }

                if (!string.IsNullOrEmpty(model.HANAME))
                {
                    data = (!string.IsNullOrEmpty(model.Email))
                        ? data.Where(x => x.HANAME != null && x.HANAME.ToLower().Contains(model.HANAME.ToLower())).ToList()
                        : dataContext.tbl_Users.Where(x => x.HANAME != null && x.HANAME.ToLower().Contains(model.HANAME.ToLower()) && x.Id != WebSecurity.CurrentUserId).ToList();
                }

                if (!string.IsNullOrEmpty(model.HAUSER))
                {
                    data = (!string.IsNullOrEmpty(model.Email)) || (!string.IsNullOrEmpty(model.HANAME))
                        ? data.Where(x => x.HAUSER != null && x.HAUSER.ToLower().Contains(model.HAUSER.ToLower())).ToList()
                        : dataContext.tbl_Users.Where(x => x.HAUSER != null && x.HAUSER.ToLower().Contains(model.HAUSER.ToLower()) && x.Id != WebSecurity.CurrentUserId).ToList();
                }

                if (!string.IsNullOrEmpty(model.Dept_Code))
                {
                    data = (!string.IsNullOrEmpty(model.Email)) || (!string.IsNullOrEmpty(model.HANAME)) || (!string.IsNullOrEmpty(model.HAUSER))
                        ? data.Where(x => x.Dept_Code != null && x.Dept_Code.ToLower().Contains(model.Dept_Code.ToLower())).ToList()
                        : dataContext.tbl_Users.Where(x => x.Dept_Code != null && x.Dept_Code.ToLower().Contains(model.Dept_Code.ToLower()) && x.Id != WebSecurity.CurrentUserId).ToList();
                }

                if (!string.IsNullOrEmpty(model.Status))
                {
                    if ((!string.IsNullOrEmpty(model.Email)) || (!string.IsNullOrEmpty(model.HANAME))
                        || (!string.IsNullOrEmpty(model.HAUSER)) || (!string.IsNullOrEmpty(model.Dept_Code)))
                    {
                        if ("Active".Contains(model.Status))
                        {
                            data = data.Where(x => x.IsActive).ToList();
                        }
                        else if ("Inactive".Contains(model.Status))
                        {
                            data = data.Where(x => !x.IsActive).ToList();
                        }
                        else
                        {
                            data = new List<tbl_Users>();
                        }
                    }
                    else
                    {
                        if ("Active".Contains(model.Status))
                        {
                            data = dataContext.tbl_Users.Where(x => x.Id != WebSecurity.CurrentUserId && x.IsActive).ToList();
                        }
                        else if ("Inactive".Contains(model.Status))
                        {
                            data = dataContext.tbl_Users.Where(x => x.Id != WebSecurity.CurrentUserId && !x.IsActive).ToList();
                        }
                        else
                        {
                            data = new List<tbl_Users>();
                        }
                    }
                }

                if (string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.HANAME)
                    && string.IsNullOrEmpty(model.HAUSER) && string.IsNullOrEmpty(model.RoleName)
                     && string.IsNullOrEmpty(model.Dept_Code) && string.IsNullOrEmpty(model.Status))
                {
                    data = dataContext.tbl_Users.Where(x => x.Id != WebSecurity.CurrentUserId).ToList();
                }

                var users = new List<UserViewModel>();

                data.ForEach(item =>
                {
                    users.Add(new UserViewModel()
                    {
                        Id = item.Id,
                        Email = item.EmailAddress,
                        RoleName = Roles.GetRolesForUser(item.EmailAddress).First().ToString(),
                        HAUSER = item.HAUSER,
                        HANAME = item.HANAME,
                        Dept_Code = item.Dept_Code,
                        HANEXT = item.HANEXT,
                        Status = item.IsActive ? "Active" : "Inactive"
                    });
                });

                if (!string.IsNullOrEmpty(model.RoleName))
                {
                    users = users.Where(x => x.RoleName != null && x.RoleName.ToLower().Contains(model.RoleName.ToLower())).ToList();
                }

                result = this.Json(new
                {
                    draw = Convert.ToInt32(Request.Form.GetValues("draw")[0]),
                    recordsTotal = (users.Count > 0) ? users.Count : 0,
                    recordsFiltered = (users.Count > 0) ? users.Count : 0,
                    data = users
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
            }

            return result;
        }

        public JsonResult DeleteUser(int id)
        {
            try
            {
                var objUser = dataContext.tbl_Users.FirstOrDefault(x => x.Id == id);
                if (objUser != null)
                {
                    if (dataContext.tbl_Users.FirstOrDefault(x => x.HANEXT == objUser.HAUSER) != null)
                    {
                        return Json(new { status = false, result = "User used as next user, so you can't delete this user" });
                    }

                    ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(objUser.EmailAddress); // deletes record from webpages_Membership table
                    Roles.RemoveUserFromRole(objUser.EmailAddress, Roles.GetRolesForUser(objUser.EmailAddress).First().ToString());
                    ((SimpleMembershipProvider)Membership.Provider).DeleteUser(objUser.EmailAddress, true); // deletes record from UserProfile table
                }

                return Json(new { status = true });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, result = ex.GetBaseException().Message });
            }
        }

        [HttpGet]
        public JsonResult ActiveDeactiveUser(int id, bool status)
        {
            try
            {
                var objUser = dataContext.tbl_Users.FirstOrDefault(x => x.Id == id);
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
        public ActionResult EditUser(UserViewModel user)
        {
            try
            {
                ViewBag.UserList = GetUsers(new string[] { user.HANEXT }, user.HAUSER);
                ViewBag.RoleList = GetRoles(new string[] { user.RoleName });
                ViewBag.DepartmentList = GetDepartments(new string[] { user.Dept_Code });

                if (ModelState.IsValid)
                {
                    var dataUserCode = dataContext.tbl_Users.FirstOrDefault(x => x.Id != user.Id && x.HAUSER == user.HAUSER);
                    if (dataUserCode != null)
                    {
                        ModelState.AddModelError(nameof(user.HAUSER), "User Code already exist.");
                        return View(user);
                    }

                    var data = dataContext.tbl_Users.FirstOrDefault(x => x.Id == user.Id);
                    if (data != null)
                    {
                        var roleName = Roles.GetRolesForUser(data.EmailAddress).First().ToString();
                        if (roleName != user.RoleName)
                        {
                            Roles.RemoveUserFromRole(data.EmailAddress, roleName);
                            Roles.AddUserToRole(data.EmailAddress, user.RoleName);
                        }

                        data.EmailAddress = user.Email;
                        data.HANEXT = user.HANEXT;
                        data.HAUSER = user.HAUSER;
                        data.HANAME = user.HANAME;
                        data.Dept_Code = user.Dept_Code;
                        dataContext.SaveChanges();
                    }

                    return RedirectToAction("UserIndex");
                }
            }
            catch (Exception)
            {
            }

            return View(user);
        }

        public JsonResult UserCode(string term)
        {
            var result = dataContext.tbl_Users.Where(c => c.Id != WebSecurity.CurrentUserId && c.HAUSER.ToString().ToLower().Contains(term)).Select(a => new { label = a.HAUSER }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserStatus(string term)
        {
            var result = new List<string> { "Active", "Inactive" }.Where(c => c.ToString().ToLower().Contains(term)).Select(a => new { label = a }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}