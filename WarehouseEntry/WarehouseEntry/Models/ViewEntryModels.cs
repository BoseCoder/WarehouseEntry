using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;
using WarehouseEntry.Business.Languages;
using WarehouseEntry.Controllers;

namespace WarehouseEntry.Models
{
    public class ViewEntryModel<T> : ViewBaseModel<T> where T : new()
    {
        public ViewEntryModel()
        { }

        public ViewEntryModel(BaseController controller)
            : base(controller)
        { }

        /// <summary>
        /// 批注完成时间人员
        /// </summary>
        [Display(ResourceType = typeof(BusinessCommonResource), Name = "HandlerName")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(BusinessCommonResource), ErrorMessageResourceName = "HandlerNameRequired")]
        public string HandlerName { get; set; }

        [IgnoreDataMember]
        public SelectList Handlers { get; set; }
    }
}