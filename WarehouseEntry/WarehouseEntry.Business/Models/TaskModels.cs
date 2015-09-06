using System;
using System.ComponentModel.DataAnnotations;
using WarehouseEntry.Business.Languages;

namespace WarehouseEntry.Business.Models
{
    public class TaskModel
    {
        [Display(ResourceType = typeof(TaskModelResource), Name = "TaskName")]
        public string TaskName { get; set; }
        public string TaskUrl { get; set; }
        [Display(ResourceType = typeof(TaskModelResource), Name = "CurrentHandler")]
        public BusinessUserModel CurrentHandler { get; set; }
        [Display(ResourceType = typeof(TaskModelResource), Name = "SetupTime")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime SetupTime { get; set; }
    }
}
