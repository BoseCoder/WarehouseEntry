using System.Collections.Generic;
using System.Web.Mvc;
using WarehouseEntry.Business.Functions.Task;
using WarehouseEntry.Business.Models;

namespace WarehouseEntry.Controllers
{
    public class TaskController : BaseController
    {
        [HttpGet]
        public PartialViewResult TaskList(string handler, string taskNameKey)
        {
            List<TaskModel> model = TaskManager.GetTaskList(handler, taskNameKey);
            return PartialView(model);
        }

        [HttpGet]
        public PartialViewResult MyTaskList()
        {
            return TaskList(base.LoginAccount, string.Empty);
        }
    }
}