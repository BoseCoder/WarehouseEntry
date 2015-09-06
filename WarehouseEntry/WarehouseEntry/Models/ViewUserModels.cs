using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using WarehouseEntry.Business.Languages;
using WarehouseEntry.Business.Models;
using WarehouseEntry.Controllers;

namespace WarehouseEntry.Models
{
    public class ViewRoleModel : ViewBaseModel<BusinessRoleModel>
    {
        public bool Selected { get; set; }

        public ViewRoleModel()
        {}

        public ViewRoleModel(BaseController controller)
            : base(controller)
        { }
    }

    [Serializable]
    [DataContract]
    public class ViewUserModel : ViewBaseModel
    {
        [DataMember(Name = "OldPassword")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UserModelResource), ErrorMessageResourceName = "PasswordRequired")]
        [StringLength(12, MinimumLength = 6, ErrorMessageResourceType = typeof(UserModelResource), ErrorMessageResourceName = "PasswordStringLength")]
        [Display(ResourceType = typeof(UserModelResource), Name = "PasswordOld")]
        public string OldPassword { get; set; }

        [DataMember(Name = "NewPassword")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UserModelResource), ErrorMessageResourceName = "PasswordRequired")]
        [StringLength(12, MinimumLength = 6, ErrorMessageResourceType = typeof(UserModelResource), ErrorMessageResourceName = "PasswordStringLength")]
        [Display(ResourceType = typeof(UserModelResource), Name = "PasswordNew")]
        public string NewPassword { get; set; }

        public ViewUserModel()
        {}

        public ViewUserModel(BaseController controller)
            : base(controller)
        { }
    }

    public class ViewUserIndexModel : ViewBaseModel
    {
        public List<ViewRoleModel> Roles { get; set; }
        public List<BusinessUserModel> Users { get; set; }

        public ViewUserIndexModel()
        {}

        public ViewUserIndexModel(BaseController controller)
            : base(controller)
        { }
    }

    public class ViewRightIndexModel : ViewBaseModel
    {
        public List<ViewRoleModel> Roles { get; set; }
        public List<BusinessRightModel> Rights { get; set; }

        public ViewRightIndexModel()
        {}

        public ViewRightIndexModel(BaseController controller)
            : base(controller)
        { }
    }
}