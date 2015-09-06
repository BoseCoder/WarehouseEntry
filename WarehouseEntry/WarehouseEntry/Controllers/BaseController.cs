using System;
using System.Web.Mvc;
using Resources;

namespace WarehouseEntry.Controllers
{
    public abstract class BaseController : Controller
    {
        protected const string VirtualAccount = "Admin";

        public string LoginAccount
        {
            get { return Session["LoginAccount"] as string; }
            set { Session["LoginAccount"] = value; }
        }

        public string LoginUserName
        {
            get { return Session["LoginUserName"] as string; }
            set { Session["LoginUserName"] = value; }
        }

        public bool IsVirtualAccount
        {
            get { return string.Equals(this.LoginAccount, VirtualAccount, StringComparison.OrdinalIgnoreCase); }
        }

        public bool IsLogin
        {
            get { return !string.IsNullOrWhiteSpace(this.LoginAccount); }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            this.ViewBag.Title = SiteCommonResource.ResourceManager.GetString(string.Format("ViewTitle{0}Controller{1}",
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                filterContext.ActionDescriptor.ActionName));
        }
    }
}