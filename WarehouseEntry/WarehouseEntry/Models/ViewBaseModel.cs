using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Resources;
using WarehouseEntry.Controllers;

namespace WarehouseEntry.Models
{
    public interface IViewBaseModel
    {
        List<SiteMenuModel> VisiabledRootMenus { get; }
        bool ContainSiteMenuCurrentNode(SiteMenuModel rootMenu);
        SiteMenuModel BreadCrumbCurrentNode { get; }
        List<SiteMenuModel> BreadCrumb { get; }
        bool IsLogin { get; }
        string LoginUserInfo { get; }
    }

    public class ViewBaseModel : IViewBaseModel
    {
        private readonly bool _isLogin;
        private readonly bool _isVirtualAccount;
        private readonly string _loginUserInfo;
        private readonly SiteMenuModel _currentRootMenu;
        private readonly List<SiteMenuModel> _rootMenus;
        public List<SiteMenuModel> RootMenus
        {
            get { return this._rootMenus; }
        }
        public List<SiteMenuModel> VisiabledRootMenus
        {
            get { return this._rootMenus.Where(m => m.Visible).ToList(); }
        }
        private readonly List<SiteMenuModel> _breadCrumb;
        public List<SiteMenuModel> BreadCrumb
        {
            get { return this._breadCrumb; }
        }
        private readonly SiteMenuModel _breadCrumbCurrentNode;
        public SiteMenuModel BreadCrumbCurrentNode
        {
            get { return this._breadCrumbCurrentNode; }
        }
        public bool IsLogin
        {
            get { return this._isLogin; }
        }
        public bool IsVirtualAccount
        {
            get { return this._isVirtualAccount; }
        }
        public string LoginUserInfo
        {
            get { return this._loginUserInfo; }
        }

        public ViewBaseModel()
        { }

        public ViewBaseModel(BaseController controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }
            this._isLogin = controller.IsLogin;
            this._isVirtualAccount = controller.IsVirtualAccount;
            this._loginUserInfo = string.Format(SiteCommonResource.TextFormatUserInfo,
                controller.LoginUserName, controller.LoginAccount);
            HttpContext context = HttpContext.Current;
            SiteMenuModel menuRoot = CacheManager.GetSiteMenus(context);
            this._rootMenus = menuRoot.SubMenus;
            string relativePath = string.Format("~/{0}/{1}",
                controller.ValueProvider.GetValue("controller").RawValue, controller.ValueProvider.GetValue("action").RawValue);
            SiteMenuModel siteMenuCurrentNode = SiteMenuModel.GetSiteMenuCurrentNode(relativePath);
            if (siteMenuCurrentNode != null)
            {
                this._currentRootMenu = siteMenuCurrentNode;
                while (this._currentRootMenu.ParentMenu != null && this._currentRootMenu.ParentMenu != menuRoot)
                {
                    this._currentRootMenu = this._currentRootMenu.ParentMenu;
                }
            }

            this._breadCrumbCurrentNode = SiteMenuModel.GetBreadCrumbCurrentNode(relativePath);
            if (this._breadCrumbCurrentNode != null)
            {
                this._breadCrumb = new List<SiteMenuModel>();
                SiteMenuModel parentNode = this._breadCrumbCurrentNode.ParentMenu;
                while (parentNode != null)
                {
                    this._breadCrumb.Insert(0, parentNode);
                    parentNode = parentNode.ParentMenu;
                }
            }
        }

        public bool ContainSiteMenuCurrentNode(SiteMenuModel rootMenu)
        {
            return this._currentRootMenu != null && this._currentRootMenu == rootMenu;
        }


        public static string GetFunctionText(string functionName)
        {
            return FunctionResource.ResourceManager.GetString(functionName);
        }
    }

    [Serializable]
    [DataContract]
    public class ViewBaseModel<T> : ViewBaseModel where T : new()
    {
        public ViewBaseModel()
        { }

        public ViewBaseModel(BaseController controller)
            : base(controller)
        { }

        [DataMember(Name = "DataModel")]
        public T DataModel { get; set; }
    }
}