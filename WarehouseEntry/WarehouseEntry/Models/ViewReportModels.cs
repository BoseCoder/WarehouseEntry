using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WarehouseEntry.Business.Languages;
using WarehouseEntry.Controllers;

namespace WarehouseEntry.Models
{
    public class ViewProductReportModel : ViewBaseModel
    {
        public ViewProductReportModel()
        { }

        public ViewProductReportModel(BaseController controller)
            : base(controller)
        { }

        [Display(ResourceType = typeof(EntryModelResource), Name = "ProjectName")]
        public string ProjectName { get; set; }

        public SelectList ProjectNames { get; set; }
    }
}