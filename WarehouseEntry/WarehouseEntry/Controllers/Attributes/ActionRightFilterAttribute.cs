using System.Web.Mvc;
using System.Web.Routing;
using Resources;
using WarehouseEntry.Business.Functions.Security;
using WarehouseEntry.Business.Models;

namespace WarehouseEntry.Controllers.Attributes
{
    public class ActionRightFilterAttribute : ActionFilterAttribute
    {
        public bool MustSignIn { get; set; }

        public bool CheckRight { get; set; }

        public bool AllowVirtualAccount { get; set; }

        public ActionRightFilterAttribute()
        {
            this.MustSignIn = true;
            this.CheckRight = true;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            BaseController controller = filterContext.Controller as BaseController;
            if (controller != null)
            {
                if (controller.IsLogin)
                {
                    if (this.CheckRight)
                    {
                        if ((controller.IsVirtualAccount && !this.AllowVirtualAccount)
                        || (!controller.IsVirtualAccount && !RightManager.HasRight(controller.LoginAccount,
                            string.Format("{0}Controller{1}",
                                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                                filterContext.ActionDescriptor.ActionName))))
                        {
                            if (filterContext.HttpContext.Request.IsAjaxRequest())
                            {
                                filterContext.Result = new JsonResult { Data = JsonModel.Fail(SiteCommonResource.TextNoAccess) };
                            }
                            else
                            {
                                filterContext.Result = new RedirectResult("~/Home/NoAccess");
                            }
                        }
                    }
                }
                else if (this.MustSignIn)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult
                        {
                            Data = JsonModel.Fail(SiteCommonResource.AlertWarningUnSignIn, new
                            {
                                backUrl = controller.Url.Action("SignIn", "User", new
                                {
                                    backUrl = controller.Url.Action(filterContext.ActionDescriptor.ActionName,
                                        filterContext.ActionDescriptor.ControllerDescriptor.ControllerName)
                                })
                            })
                        };
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            controller = "User",
                            action = "SignIn",
                            backUrl = controller.Url.Action(filterContext.ActionDescriptor.ActionName,
                                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName)
                        }));
                    }
                }
            }
        }
    }
}