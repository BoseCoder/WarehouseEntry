using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WarehouseEntry.Business.Languages;
using WarehouseEntry.Database.DataModel;

namespace WarehouseEntry.Business.Models
{
    public class BusinessRoleModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UserModelResource), ErrorMessageResourceName = "RoleNameRequired")]
        [StringLength(25, ErrorMessageResourceType = typeof(UserModelResource), ErrorMessageResourceName = "RoleNameStringLength")]
        public string RoleName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UserModelResource), ErrorMessageResourceName = "EnabledRequired")]
        public bool Enabled { get; set; }
    }

    public class BusinessUserModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UserModelResource), ErrorMessageResourceName = "RoleNameSelectRequired")]
        public string RoleName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UserModelResource), ErrorMessageResourceName = "UserNameRequired")]
        [StringLength(25, ErrorMessageResourceType = typeof(UserModelResource), ErrorMessageResourceName = "UserNameStringLength")]
        [Display(ResourceType = typeof(UserModelResource), Name = "UserName")]
        public string UserName { get; set; }
        [Display(ResourceType = typeof(UserModelResource), Name = "DisplayName")]
        public string DisplayName { get; set; }
        [Display(ResourceType = typeof(UserModelResource), Name = "CreationTime")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationTime { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UserModelResource), ErrorMessageResourceName = "EnabledRequired")]
        [Display(ResourceType = typeof(UserModelResource), Name = "Enabled")]
        public bool Enabled { get; set; }

        public BusinessUserModel()
        {
            
        }

        public BusinessUserModel(SecurityUser user)
        {
            this.RoleName = user.SecurityRole.RoleName;
            this.UserName = user.UserName;
            this.DisplayName = user.DisplayName;
            this.CreationTime = user.CreationTime;
            this.Enabled = user.Enabled;
        }
    }

    public class BusinessRightModel
    {
        public string MenuName { get; set; }
        public bool Enabled { get; set; }
        public List<BusinessRightModel> SubRightModels { get; set; }
    }
}