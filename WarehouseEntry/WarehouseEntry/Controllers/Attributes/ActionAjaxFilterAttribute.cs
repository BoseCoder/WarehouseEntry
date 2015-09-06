using System.Linq;
using System.Web.Mvc;
using WarehouseEntry.Business.Models;

namespace WarehouseEntry.Controllers.Attributes
{
    public class ActionAjaxFilterAttribute : ActionFilterAttribute
    {
        public bool ValidateModelState { get; set; }

        public ActionAjaxFilterAttribute()
        {
            this.ValidateModelState = true;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }
            if (!this.ValidateModelState)
            {
                return;
            }
            var modelState = filterContext.Controller.ViewData.ModelState;
            if (!modelState.IsValid)
            {
                var errorModel = modelState.Select(ms => new
                {
                    key = ms.Key,
                    error = string.Join(";", ms.Value.Errors.Select(e => e.ErrorMessage))
                });
                JsonModel jsonModel = JsonModel.Fail(string.Empty, errorModel);
                filterContext.Result = new JsonResult
                {
                    Data = jsonModel
                };
            }
        }
    }
}
