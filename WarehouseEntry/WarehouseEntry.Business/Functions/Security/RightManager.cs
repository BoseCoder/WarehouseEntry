using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using WarehouseEntry.Business.Models;
using WarehouseEntry.Database.DataAction;
using WarehouseEntry.Database.DataModel;

namespace WarehouseEntry.Business.Functions.Security
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public class RightManager
    {
        /// <summary>
        /// 获取功能权限
        /// </summary>
        /// <param name="userName">账户</param>
        /// <param name="menuName">功能</param>
        /// <returns></returns>
        public static bool HasRight(string userName, string menuName)
        {
            if (String.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("userName");
            }
            if (String.IsNullOrWhiteSpace(menuName))
            {
                throw new ArgumentNullException("menuName");
            }
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                return dbContext.SystemRight.Any(r => r.Enabled
                    && r.SystemMenu.MenuName == menuName
                    && r.SystemMenu.Enabled
                    && r.SecurityRole.SecurityUsers.Any(u => u.UserName == userName && u.Enabled));
            }
        }

        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <param name="viewModel">权限字典</param>
        public static void SetSystemRight(string roleName, List<BusinessRightModel> viewModel)
        {
            roleName = UserManager.CheckRoleName(roleName);
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }
            using (TransactionScope t = DbContextHelper.BeginTransaction())
            {
                using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
                {
                    SecurityRole role = UserManager.GetSecurityRole(dbContext, roleName);
                    Dictionary<string, bool> rightDic = viewModel.Union(viewModel.SelectMany(m => m.SubRightModels))
                        .ToDictionary(m => m.MenuName, m => m.Enabled);
                    List<SystemRight> rights = SecurityData.SelectRights(dbContext, role, rightDic.Select(m => m.Key).ToArray());
                    foreach (SystemRight right in rights)
                    {
                        right.Enabled = rightDic[right.SystemMenu.MenuName];
                        if (right.RightID < 1)
                        {
                            dbContext.SystemRight.Add(right);
                        }
                    }
                    dbContext.SaveChanges();
                    DbContextHelper.CompleteTransaction(t);
                }
            }
        }
    }
}
