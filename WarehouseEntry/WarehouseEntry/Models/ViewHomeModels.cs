using System.Collections.Generic;
using WarehouseEntry.Business.Models;
using WarehouseEntry.Controllers;

namespace WarehouseEntry.Models
{
    public class ViewHomeIndexModel : ViewBaseModel<List<TaskModel>>
    {
        public bool CanCreateEntry { get; set; }
        public bool CanBatchCreateEntry { get; set; }

        public ViewHomeIndexModel()
        {
            
        }

        public ViewHomeIndexModel(BaseController controller)
            : base(controller)
        { }
    }
}