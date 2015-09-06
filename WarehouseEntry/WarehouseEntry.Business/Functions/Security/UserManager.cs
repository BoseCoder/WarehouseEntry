using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using WarehouseEntry.Business.Exceptions;
using WarehouseEntry.Business.Models;
using WarehouseEntry.Database.DataAction;
using WarehouseEntry.Database.DataModel;

namespace WarehouseEntry.Business.Functions.Security
{
    /// <summary>
    /// 用户管理功能
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// 检查角色名
        /// </summary>
        /// <param name="roleName">角色名</param>
        public static string CheckRoleName(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ErrorException("RoleNameEmpty");
            }
            return roleName.Trim();
        }

        /// <summary>
        /// 获取数据库对象
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        public static SecurityRole GetSecurityRole(WarehouseEntryDbContext dbContext, string roleName)
        {
            SecurityRole role = SecurityData.SelectRole(dbContext, roleName);
            if (role == null)
            {
                throw new ErrorException("RoleNotExist", roleName);
            }
            return role;
        }

        /// <summary>
        /// 检查用户名
        /// </summary>
        /// <param name="userName">用户名</param>
        protected static string CheckUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ErrorException("UserNameEmpty");
            }
            return userName.Trim();
        }

        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="password">密码</param>
        private static void CheckPassword(string password)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ErrorException("PasswordEmpty");
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns></returns>
        private static string EncryptPassword(string password)
        {
            byte[] result = Encoding.UTF8.GetBytes(password);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return Convert.ToBase64String(output);
        }

        /// <summary>
        /// 获取数据库对象
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static SecurityUser GetSecurityUser(WarehouseEntryDbContext dbContext, string userName)
        {
            SecurityUser user = SecurityData.SelectUser(dbContext, userName);
            if (user == null)
            {
                throw new ErrorException("UserNotExist", userName);
            }
            return user;
        }

        /// <summary>
        /// 获取数据库对象，并进行密码校验
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        private static SecurityUser GetSecurityUser(WarehouseEntryDbContext dbContext, string userName, string password)
        {
            SecurityUser user = GetSecurityUser(dbContext, userName);
            if (!user.Enabled)
            {
                throw new ErrorException("UserDisabled", userName);
            }
            password = EncryptPassword(password);
            if (user.Password != password)
            {
                throw new ErrorException("PasswordError");
            }
            return user;
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public static List<BusinessRoleModel> GetRoles()
        {
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                return SecurityData.SelectRoles(dbContext).Select(r => new BusinessRoleModel
                {
                    RoleName = r.RoleName,
                    Enabled = r.Enabled
                }).ToList();
            }
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="roleName">角色名</param>
        public static BusinessRoleModel CreateRole(string roleName)
        {
            roleName = CheckRoleName(roleName);
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                SecurityRole role = SecurityData.SelectRole(dbContext, roleName);
                if (role != null)
                {
                    throw new ErrorException("RoleNameNotUnique", roleName);
                }
                role = new SecurityRole
                {
                    RoleName = roleName,
                    Enabled = true
                };
                dbContext.SecurityRole.Add(role);
                dbContext.SaveChanges();
                return new BusinessRoleModel
                {
                    RoleName = roleName,
                    Enabled = role.Enabled
                };
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleName">角色名</param>
        public static bool DeleteRole(string roleName)
        {
            roleName = CheckRoleName(roleName);
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                if (dbContext.SecurityUser.Any(u => u.SecurityRole.RoleName == roleName))
                {
                    throw new ErrorException("RoleHasUser");
                }
                SecurityRole role = SecurityData.SelectRole(dbContext, roleName);
                if (role != null)
                {
                    dbContext.SecurityRole.Remove(role);
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <param name="userNameKey">用户名关键字</param>
        public static List<BusinessUserModel> GetUsers(string roleName, string userNameKey = null)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentNullException("roleName");
            }
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                return SecurityData.SelectUsers(dbContext, roleName, userNameKey)
                    .Select(u => new BusinessUserModel
                    {
                        UserName = u.UserName,
                        DisplayName = u.DisplayName,
                        CreationTime = u.CreationTime,
                        Enabled = u.Enabled
                    }).ToList();
            }
        }

        /// <summary>
        /// 获取有权限的用户列表
        /// </summary>
        /// <param name="menuName">菜单名</param>
        /// <returns></returns>
        public static List<BusinessUserModel> GetRightUsers(string menuName)
        {
            if (string.IsNullOrWhiteSpace(menuName))
            {
                throw new ArgumentNullException("menuName");
            }
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                return SecurityData.SelectRightUsers(dbContext, menuName).Select(u => new BusinessUserModel
                {
                    UserName = u.UserName,
                    DisplayName = u.DisplayName
                }).ToList();
            }
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userModel">用户模型数据</param>
        /// <param name="password">密码</param>
        public static void CreateUser(BusinessUserModel userModel, string password)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException("userModel");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("password");
            }
            userModel.RoleName = CheckRoleName(userModel.RoleName);
            userModel.UserName = CheckUserName(userModel.UserName);
            userModel.DisplayName = string.IsNullOrWhiteSpace(userModel.DisplayName)
                ? userModel.UserName : userModel.DisplayName.Trim();
            userModel.CreationTime = DateTime.Now;
            CheckPassword(password);
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                SecurityRole role = GetSecurityRole(dbContext, userModel.RoleName);
                SecurityUser user = SecurityData.SelectUser(dbContext, userModel.UserName);
                if (user != null)
                {
                    throw new ErrorException("UserNameNotUnique", user.UserName);
                }
                user = new SecurityUser
                {
                    UserName = userModel.UserName,
                    DisplayName = userModel.DisplayName,
                    Password = EncryptPassword(password),
                    Enabled = userModel.Enabled,
                    CreationTime = userModel.CreationTime,
                    SecurityRole = role
                };
                dbContext.SecurityUser.Add(user);
                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 检查用户名、密码进行登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="isVirtual">是否虚账户</param>
        /// <returns></returns>
        public static BusinessUserModel SignIn(string userName, string password, bool isVirtual)
        {
            userName = CheckUserName(userName);
            CheckPassword(password);
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                if (isVirtual)
                {
                    if (!dbContext.SecurityUser.Any(u => u.Enabled && u.SecurityRole.Enabled))
                    {
                        return new BusinessUserModel
                        {
                            UserName = userName,
                            DisplayName = userName
                        };
                    }
                    throw new ErrorException("AdminDisabled");
                }
                SecurityUser user = GetSecurityUser(dbContext, userName, password);
                return new BusinessUserModel
                {
                    UserName = user.UserName,
                    DisplayName = user.DisplayName
                };
            }
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        public static void ChangePassword(string userName, string oldPassword, string newPassword)
        {
            userName = CheckUserName(userName);
            CheckPassword(oldPassword);
            CheckPassword(newPassword);
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                SecurityUser user = GetSecurityUser(dbContext, userName, oldPassword);
                user.Password = EncryptPassword(newPassword);
                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="enabled">启用状态</param>
        public static void ChangeEnabled(string userName, bool enabled)
        {
            userName = CheckUserName(userName);
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                SecurityUser user = GetSecurityUser(dbContext, userName);
                user.Enabled = enabled;
                dbContext.SaveChanges();
            }
        }

        public static List<BusinessRightModel> GetAllRights(BusinessRoleModel role, List<BusinessRightModel> rightModels)
        {
            if (role == null || string.IsNullOrWhiteSpace(role.RoleName))
            {
                throw new ArgumentNullException("role");
            }
            using (TransactionScope t = DbContextHelper.BeginTransaction())
            {
                using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
                {
                    SecurityRole sRole = GetSecurityRole(dbContext, role.RoleName);
                    foreach (BusinessRightModel rightModel in rightModels)
                    {
                        List<string> subMenus = rightModel.SubRightModels.Select(r => r.MenuName).ToList();
                        SecurityData.AppendSystemMenus(dbContext, rightModel.MenuName, subMenus);
                    }
                    List<SystemRight> rights = rightModels.Select(rm => new SystemRight
                    {
                        SystemMenu = new SystemMenu { MenuName = rm.MenuName }
                    }).ToList();
                    SecurityData.SelectAllRights(dbContext, sRole, rights);
                    List<BusinessRightModel> result = rights.Select(r => new BusinessRightModel
                    {
                        MenuName = r.SystemMenu.MenuName,
                        Enabled = r.Enabled,
                    }).ToList();
                    rightModels.ForEach(rm =>
                    {
                        List<SystemRight> subRights = rm.SubRightModels.Select(srm => new SystemRight
                        {
                            SystemMenu = new SystemMenu { MenuName = srm.MenuName }
                        }).ToList();
                        SecurityData.SelectAllRights(dbContext, sRole, subRights);
                        result.First(rf => rf.MenuName == rm.MenuName)
                            .SubRightModels = subRights.Select(sr => new BusinessRightModel
                        {
                            MenuName = sr.SystemMenu.MenuName,
                            Enabled = sr.Enabled,
                        }).ToList();
                    });
                    DbContextHelper.CompleteTransaction(t);
                    return result;
                }
            }
        }
    }
}
