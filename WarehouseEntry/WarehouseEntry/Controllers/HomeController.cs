using System.Web.Mvc;
using WarehouseEntry.Business.Functions.Security;
using WarehouseEntry.Business.Functions.Task;
using WarehouseEntry.Controllers.Attributes;
using WarehouseEntry.Models;

namespace WarehouseEntry.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        [ActionRightFilter(CheckRight = false)]
        public ActionResult Index()
        {
            ViewHomeIndexModel model = new ViewHomeIndexModel(this)
            {
                CanCreateEntry = RightManager.HasRight(base.LoginAccount, "EntryControllerCreateEntry"),
                CanBatchCreateEntry = RightManager.HasRight(base.LoginAccount, "EntryControllerBatchCreateEntry"),
                DataModel = TaskManager.GetTaskList(base.LoginAccount, null)
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult NoAccess()
        {
            ViewBaseModel model = new ViewBaseModel(this);
            return View(model);
        }
    }
}