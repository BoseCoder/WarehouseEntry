using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Resources;
using WarehouseEntry.Business.Define;
using WarehouseEntry.Business.Exceptions;
using WarehouseEntry.Business.Functions.Security;
using WarehouseEntry.Business.Models;
using WarehouseEntry.Controllers.Attributes;
using WarehouseEntry.Models;

namespace WarehouseEntry.Controllers
{
    public class UserController : BaseController
    {
        private static ViewRoleModel GetSelectedRole(List<ViewRoleModel> roles, string roleName)
        {
            ViewRoleModel role = null;
            if (roles.Any())
            {
                role = string.IsNullOrWhiteSpace(roleName)
                    ? roles.FirstOrDefault() : roles.FirstOrDefault(r => r.DataModel.RoleName == roleName.Trim());
            }
            if (role != null)
            {
                role.Selected = true;
            }
            return role;
        }

        [HttpGet]
        [ActionRightFilter(AllowVirtualAccount = true)]
        public ActionResult UserIndex(string roleName)
        {
            List<ViewRoleModel> roles = UserManager.GetRoles().Select(r => new ViewRoleModel { DataModel = r }).ToList();
            ViewRoleModel selectedRole = GetSelectedRole(roles, roleName);
            if (!string.IsNullOrWhiteSpace(roleName) && selectedRole == null)
            {
                return RedirectToAction("UserIndex");
            }
            List<BusinessUserModel> users = null;
            if (selectedRole != null)
            {
                users = UserManager.GetUsers(selectedRole.DataModel.RoleName);
            }
            ViewUserIndexModel model = new ViewUserIndexModel(this)
            {
                Roles = roles,
                Users = users ?? new List<BusinessUserModel>()
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult UserList(List<BusinessUserModel> model)
        {
            return PartialView(model);
        }

        [HttpPost]
        [ActionRightFilter(AllowVirtualAccount = true)]
        public JsonResult CreateRole(string roleName)
        {
            JsonModel jsonModel;
            try
            {
                BusinessRoleModel role = UserManager.CreateRole(roleName);
                jsonModel = JsonModel.Success(SiteCommonResource.MessageCreateRoleSuccessfully);
                jsonModel.ObjectModel = role;
            }
            catch (Exception ex)
            {
                jsonModel = BaseException.HandleException(ex);
            }
            return Json(jsonModel);
        }

        [HttpPost]
        [ActionRightFilter(AllowVirtualAccount = true)]
        public JsonResult DeleteRole(string roleName)
        {
            JsonModel jsonModel;
            try
            {
                jsonModel = JsonModel.Success(UserManager.DeleteRole(roleName)
                    ? SiteCommonResource.MessageDeleteRoleSuccessfully : SiteCommonResource.MessageRoleDeleted);
            }
            catch (Exception ex)
            {
                jsonModel = BaseException.HandleException(ex);
            }
            return Json(jsonModel);
        }

        [HttpPost]
        [ActionAjaxFilter]
        [ActionRightFilter(AllowVirtualAccount = true)]
        public JsonResult CreateUser(BusinessUserModel model, string password)
        {
            JsonModel jsonModel;
            try
            {
                UserManager.CreateUser(model, password);
                jsonModel = JsonModel.Success(SiteCommonResource.MessageCreateUserSuccessfully);
                jsonModel.ObjectModel = model;
            }
            catch (Exception ex)
            {
                jsonModel = BaseException.HandleException(ex);
            }
            return Json(jsonModel);
        }

        [HttpPost]
        [ActionRightFilter(AllowVirtualAccount = true)]
        public JsonResult EnableUser(string userName, bool enabled)
        {
            JsonModel jsonModel;
            try
            {
                UserManager.ChangeEnabled(userName, enabled);
                jsonModel = JsonModel.Success(enabled
                    ? SiteCommonResource.MessageEnableUserSuccessfully
                    : SiteCommonResource.MessageDisableUserSuccessfully);
            }
            catch (Exception ex)
            {
                jsonModel = BaseException.HandleException(ex);
            }
            return Json(jsonModel);
        }

        [HttpGet]
        [ActionRightFilter(AllowVirtualAccount = true)]
        public ActionResult RightIndex(string roleName)
        {
            List<ViewRoleModel> roles = UserManager.GetRoles().Select(r => new ViewRoleModel { DataModel = r }).ToList();
            ViewRoleModel selectedRole = GetSelectedRole(roles, roleName);
            if (!string.IsNullOrWhiteSpace(roleName) && selectedRole == null)
            {
                return RedirectToAction("RightIndex");
            }
            List<BusinessRightModel> rights = null;
            if (selectedRole != null)
            {
                rights = new List<BusinessRightModel>
                {
                    new BusinessRightModel
                    {
                        MenuName = SystemFunction.UserControllerUserIndex.ToString(),
                        SubRightModels = new List<BusinessRightModel>
                        {
                            new BusinessRightModel { MenuName = SystemFunction.UserControllerCreateRole.ToString() },
                            new BusinessRightModel { MenuName = SystemFunction.UserControllerDeleteRole.ToString() },
                            new BusinessRightModel { MenuName = SystemFunction.UserControllerCreateUser.ToString() },
                            new BusinessRightModel { MenuName = SystemFunction.UserControllerEnableUser.ToString() },
                            //new BusinessRightModel { MenuName = SystemFunction.UserControllerChangePassword.ToString() }
                        }
                    },
                    new BusinessRightModel
                    {
                        MenuName = SystemFunction.UserControllerRightIndex.ToString(),
                        SubRightModels = new List<BusinessRightModel>
                        {
                            new BusinessRightModel { MenuName = SystemFunction.UserControllerEnableRight.ToString() }
                        }
                    },
                    new BusinessRightModel
                    {
                        MenuName = SystemFunction.EntryControllerEntryIndex.ToString(),
                        SubRightModels = new List<BusinessRightModel>
                        {
                            new BusinessRightModel { MenuName = SystemFunction.EntryControllerCreateEntry.ToString() },
                            new BusinessRightModel { MenuName = SystemFunction.EntryControllerEditEntry.ToString() },
                            new BusinessRightModel { MenuName = SystemFunction.EntryControllerCompleteEntry.ToString() },
                            new BusinessRightModel { MenuName = SystemFunction.EntryControllerBatchCreateEntry.ToString() },
                            new BusinessRightModel { MenuName = SystemFunction.EntryControllerBatchEditEntry.ToString() },
                            new BusinessRightModel { MenuName = SystemFunction.EntryControllerBatchCompleteEntry.ToString() },
                        }
                    }
                };
                rights = UserManager.GetAllRights(selectedRole.DataModel, rights);
            }
            ViewRightIndexModel model = new ViewRightIndexModel(this)
            {
                Roles = roles,
                Rights = rights ?? new List<BusinessRightModel>()
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult RightList(List<BusinessRightModel> model)
        {
            return PartialView(model);
        }

        [HttpPost]
        [ActionAjaxFilter]
        [ActionRightFilter(AllowVirtualAccount = true)]
        public JsonResult EnableRight(string roleName, List<BusinessRightModel> model)
        {
            JsonModel jsonModel;
            try
            {
                RightManager.SetSystemRight(roleName, model);
                jsonModel = JsonModel.Success(SiteCommonResource.MessageSaveRightSuccessfully);
            }
            catch (Exception ex)
            {
                jsonModel = BaseException.HandleException(ex);
            }
            return Json(jsonModel);
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            BusinessUserModel model = new BusinessUserModel
            {
                UserName = base.LoginAccount
            };
            Session.Clear();
            return View(model);
        }

        [HttpPost]
        public JsonResult SignIn(string userName, string password, string backUrl)
        {
            JsonModel jsonModel;
            try
            {
                BusinessUserModel model = UserManager.SignIn(userName, password,
                    string.Equals(userName, VirtualAccount, StringComparison.OrdinalIgnoreCase));
                base.LoginAccount = model.UserName;
                base.LoginUserName = model.DisplayName;
                jsonModel = JsonModel.Success(SiteCommonResource.MessageLoginSucessfully);
                if (string.IsNullOrWhiteSpace(backUrl))
                {
                    backUrl = Request["backUrl"];
                }
                if (string.IsNullOrWhiteSpace(backUrl))
                {
                    backUrl = "../Home/Index";
                }
                jsonModel.ObjectModel = new { backUrl };
            }
            catch (Exception ex)
            {
                jsonModel = BaseException.HandleException(ex);
            }
            return Json(jsonModel);
        }

        [HttpGet]
        [ActionRightFilter(CheckRight = false)]
        public ActionResult ChangePassword()
        {
            ViewUserModel model = new ViewUserModel(this);
            return View(model);
        }

        [HttpPost]
        [ActionAjaxFilter]
        [ActionRightFilter(CheckRight = false)]
        public ActionResult ChangePassword(ViewUserModel model)
        {
            JsonModel jsonModel;
            try
            {
                UserManager.ChangePassword(base.LoginAccount, model.OldPassword, model.NewPassword);
                jsonModel = JsonModel.Success(SiteCommonResource.MessageChangePasswordSuccessfully);
            }
            catch (Exception ex)
            {
                jsonModel = BaseException.HandleException(ex);
            }
            return Json(jsonModel);
        }
    }
}