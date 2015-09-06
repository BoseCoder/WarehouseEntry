using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace WarehouseEntry.Models
{
    [Serializable]
    [XmlRoot(ElementName = "SiteMenuRoot")]
    public class SiteMenuModel
    {
        private static readonly Dictionary<string, SiteMenuModel> SiteMenuDictionary = new Dictionary<string, SiteMenuModel>();
        private static readonly Dictionary<string, SiteMenuModel> BreadCrumbDictionary = new Dictionary<string, SiteMenuModel>();
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }
        [XmlAttribute(AttributeName = "isAction")]
        public bool IsAction { get; set; }
        [XmlAttribute(AttributeName = "cssClass")]
        public string CssClass { get; set; }
        [XmlAttribute(AttributeName = "visible")]
        public bool Visible { get; set; }
        [XmlArray(ElementName = "SubMenus")]
        [XmlArrayItem(ElementName = "SiteMenuModel")]
        public List<SiteMenuModel> SubMenus { get; set; }
        [XmlIgnore]
        public List<SiteMenuModel> VisiabledSubMenus
        {
            get { return this.SubMenus.Where(m => m.Visible).ToList(); }
        }
        [XmlIgnore]
        public SiteMenuModel ParentMenu { get; set; }

        private void UpdateActionDictionary(SiteMenuModel menu, Dictionary<string, SiteMenuModel> dictionary)
        {
            if (menu == null)
            {
                return;
            }
            if (menu.IsAction && !string.IsNullOrWhiteSpace(menu.Url) && !dictionary.ContainsKey(menu.Url))
            {
                dictionary.Add(menu.Url, menu);
            }
            if (menu.SubMenus != null)
            {
                foreach (SiteMenuModel subMenu in menu.SubMenus)
                {
                    UpdateActionDictionary(subMenu, dictionary);
                    subMenu.ParentMenu = menu;
                }
            }
        }

        public void UpdateSiteMenuDictionary()
        {
            SiteMenuDictionary.Clear();
            UpdateActionDictionary(this, SiteMenuDictionary);
        }

        public static SiteMenuModel GetSiteMenuCurrentNode(string url)
        {
            if (SiteMenuDictionary.ContainsKey(url))
            {
                return SiteMenuDictionary[url];
            }
            return null;
        }

        public void UpdateBreadCrumbDictionary()
        {
            BreadCrumbDictionary.Clear();
            foreach (SiteMenuModel subMenu in this.SubMenus)
            {
                UpdateActionDictionary(subMenu, BreadCrumbDictionary);
                subMenu.ParentMenu = this;
            }
        }

        public static SiteMenuModel GetBreadCrumbCurrentNode(string url)
        {
            if (BreadCrumbDictionary.ContainsKey(url))
            {
                return BreadCrumbDictionary[url];
            }
            return null;
        }
    }
}