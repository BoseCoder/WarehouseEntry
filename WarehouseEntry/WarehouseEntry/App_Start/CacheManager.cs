using System;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;
using WarehouseEntry.Models;

namespace WarehouseEntry
{
    public class CacheManager
    {
        private const string SiteMenuXml = "~/CacheConfig/SiteMenuConfig/SiteMenuConfig.xml";
        private const string BreadCrumbXml = "~/CacheConfig/BreadCrumbConfig/BreadCrumbConfig.xml";

        static CacheManager()
        {
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                InitializeSiteMenuModel(context);

                InitializeBreadCrumb(context);
            }
        }

        #region Site Menu

        private static void InitializeSiteMenuModel(HttpContext context)
        {
            string siteMenuXml = context.Server.MapPath(SiteMenuXml);
            SiteMenuModel model = UpdateCacheFromXml<SiteMenuModel>(siteMenuXml);
            model.UpdateSiteMenuDictionary();
        }

        public static SiteMenuModel GetSiteMenus(HttpContext context)
        {
            string siteMenuXml = context.Server.MapPath(SiteMenuXml);
            SiteMenuModel model = HttpRuntime.Cache[siteMenuXml] as SiteMenuModel;
            if (model == null)
            {
                model = UpdateCacheFromXml<SiteMenuModel>(siteMenuXml);
                model.UpdateSiteMenuDictionary();
            }
            return model;
        }

        #endregion

        #region Bread Crumb

        private static void InitializeBreadCrumb(HttpContext context)
        {
            string breadCrumbXml = context.Server.MapPath(BreadCrumbXml);
            SiteMenuModel model = UpdateCacheFromXml<SiteMenuModel>(breadCrumbXml);
            model.UpdateBreadCrumbDictionary();
        }

        public static SiteMenuModel GetBreadCrumb(HttpContext context)
        {
            string breadCrumbXml = context.Server.MapPath(BreadCrumbXml);
            SiteMenuModel model = HttpRuntime.Cache[breadCrumbXml] as SiteMenuModel;
            if (model == null)
            {
                model = UpdateCacheFromXml<SiteMenuModel>(breadCrumbXml);
                model.UpdateBreadCrumbDictionary();
            }
            return model;
        }

        #endregion

        private static T UpdateCacheFromXml<T>(string filePath)
        {
            using (XmlReader reader = new XmlTextReader(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                T obj = (T)serializer.Deserialize(reader);
                HttpRuntime.Cache.Insert(filePath, obj, new CacheDependency(filePath), DateTime.MaxValue,
                    Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                return obj;
            }
        }
    }
}