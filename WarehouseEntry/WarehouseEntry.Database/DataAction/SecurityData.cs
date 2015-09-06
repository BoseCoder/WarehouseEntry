using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WarehouseEntry.Database.DataModel;

namespace WarehouseEntry.Database.DataAction
{
    /// <summary>
    /// 账户数据
    /// </summary>
    public static class SecurityData
    {
        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <returns></returns>
        public static List<SecurityRole> SelectRoles(WarehouseEntryDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            return dbContext.SecurityRole.ToList();
        }

        /// <summary>
        /// 查询角色实体
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        public static SecurityRole SelectRole(WarehouseEntryDbContext dbContext, string roleName)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentNullException("roleName");
            }
            return dbContext.SecurityRole.FirstOrDefault(r => r.RoleName == roleName);
        }

        ///// <summary>
        ///// 查询用户列表
        ///// </summary>
        ///// <param name="dbContext">DbContext</param>
        ///// <param name="roleName">角色名</param>
        ///// <param name="userNameKey">用户名关键字</param>
        ///// <param name="pageIndex">分页页码</param>
        ///// <returns></returns>
        //public static List<SecurityUser> SelectUsers(WarehouseEntryDbContext dbContext, string roleName, string userNameKey, int pageIndex)
        //{
        //    if (dbContext == null)
        //    {
        //        throw new ArgumentNullException("dbContext");
        //    }
        //    IQueryable<SecurityUser> query = dbContext.SecurityUser;
        //    if (!string.IsNullOrWhiteSpace(roleName))
        //    {
        //        query = query.Where(u => u.SecurityRole.RoleName == roleName);
        //    }
        //    if (!string.IsNullOrWhiteSpace(userNameKey))
        //    {
        //        query = query.Where(u => u.UserName.Contains(userNameKey));
        //    }
        //    PagingData paging = new PagingData { PageIndex = pageIndex };
        //    return paging.SelectFootPagingData(query, u => u.CreationTime).ToList();
        //}

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <param name="roleName">角色名</param>
        /// <param name="userNameKey">用户名关键字</param>
        /// <returns></returns>
        public static List<SecurityUser> SelectUsers(WarehouseEntryDbContext dbContext, string roleName, string userNameKey)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentNullException("roleName");
            }
            IQueryable<SecurityUser> query = dbContext.SecurityUser.Where(u => u.SecurityRole.RoleName == roleName);
            if (!string.IsNullOrWhiteSpace(userNameKey))
            {
                query = query.Where(u => u.UserName.Contains(userNameKey));
            }
            return query.OrderBy(u => u.UserName).ToList();
        }

        /// <summary>
        /// 查询有权限的用户列表
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <param name="menuName">菜单名</param>
        /// <returns></returns>
        public static List<SecurityUser> SelectRightUsers(WarehouseEntryDbContext dbContext, string menuName)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            if (string.IsNullOrWhiteSpace(menuName))
            {
                throw new ArgumentNullException("menuName");
            }
            return dbContext.SecurityUser.Where(u => u.Enabled && u.SecurityRole.Enabled
                && dbContext.SystemRight.Any(r => r.SystemMenu.MenuName == menuName
                    && r.SecurityRole.RoleName == u.SecurityRole.RoleName && r.Enabled)).ToList();
        }

        /// <summary>
        /// 查询用户实体
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static SecurityUser SelectUser(WarehouseEntryDbContext dbContext, string userName)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("userName");
            }
            return dbContext.SecurityUser.FirstOrDefault(u => u.UserName == userName);
        }

        /// <summary>
        /// 查询菜单列表
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <param name="menus">菜单名列表</param>
        /// <returns></returns>
        public static List<SystemMenu> SelectSystemMenus(WarehouseEntryDbContext dbContext, string[] menus)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            if (menus == null)
            {
                throw new ArgumentNullException("menus");
            }
            if (menus.Any(string.IsNullOrWhiteSpace))
            {
                throw new InvalidEnumArgumentException("menus");
            }
            return dbContext.SystemMenu.Where(m => menus.Contains(m.MenuName)).ToList();
        }

        /// <summary>
        /// 添加菜单列表
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <param name="parentMenu">父级菜单名</param>
        /// <param name="menus">菜单名列表</param>
        public static void AppendSystemMenus(WarehouseEntryDbContext dbContext, string parentMenu, List<string> menus)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            if (menus == null)
            {
                throw new ArgumentNullException("menus");
            }
            if (menus.Any(string.IsNullOrWhiteSpace))
            {
                throw new InvalidEnumArgumentException("menus");
            }
            SystemMenu parentDataMenu = null;
            if (!string.IsNullOrWhiteSpace(parentMenu))
            {
                parentDataMenu = dbContext.SystemMenu.FirstOrDefault(m => m.MenuName == parentMenu);
            }
            if (parentDataMenu == null)
            {
                parentDataMenu = new SystemMenu { MenuName = parentMenu, Enabled = true };
            }
            List<SystemMenu> dataMenus = dbContext.SystemMenu.Where(m => menus.Contains(m.MenuName)).ToList();
            foreach (SystemMenu dataMenu in dataMenus)
            {
                menus.Remove(dataMenu.MenuName);
            }
            dbContext.SystemMenu.AddRange(menus.Select(m => new SystemMenu
            {
                ParentMenu = parentDataMenu,
                MenuName = m,
                Enabled = true
            }));
            dbContext.SaveChanges();
        }

        /// <summary>
        /// 查询所有权限字典
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <param name="role">角色</param>
        /// <param name="rights">权限列表</param>
        public static void SelectAllRights(WarehouseEntryDbContext dbContext, SecurityRole role, List<SystemRight> rights)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            if (string.IsNullOrWhiteSpace(role.RoleName))
            {
                throw new InvalidEnumArgumentException("role");
            }
            rights.ForEach(r => r.Enabled = dbContext.SystemRight.Any(dr =>
                dr.SecurityRole.RoleName == role.RoleName
                && dr.SystemMenu.MenuName == r.SystemMenu.MenuName
                && dr.Enabled));
        }

        /// <summary>
        /// 查询用户权限字典
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <param name="role">角色实体</param>
        /// <param name="menus">菜单名称列表</param>
        /// <returns></returns>
        public static List<SystemRight> SelectRights(WarehouseEntryDbContext dbContext, SecurityRole role, string[] menus)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            if (string.IsNullOrWhiteSpace(role.RoleName))
            {
                throw new InvalidEnumArgumentException("role");
            }
            if (menus == null)
            {
                throw new ArgumentNullException("menus");
            }
            List<SystemMenu> dataMenus = dbContext.SystemMenu.Where(m => menus.Contains(m.MenuName)).ToList();
            return dataMenus.Select(dataMenu => dbContext.SystemRight.FirstOrDefault(r => r.SecurityRole.RoleName == role.RoleName
                && r.SystemMenu.MenuName == dataMenu.MenuName)
                ?? new SystemRight
                {
                    SecurityRole = role, SystemMenu = dataMenu
                }).ToList();
        }
    }
}
