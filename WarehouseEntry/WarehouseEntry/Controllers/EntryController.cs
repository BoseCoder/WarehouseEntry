using LinqToExcel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Resources;
using WarehouseEntry.Business.Exceptions;
using WarehouseEntry.Business.Functions.Entry;
using WarehouseEntry.Business.Functions.Security;
using WarehouseEntry.Business.Functions.Task;
using WarehouseEntry.Business.Models;
using WarehouseEntry.Controllers.Attributes;
using WarehouseEntry.Models;

namespace WarehouseEntry.Controllers
{
    public class EntryController : BaseController
    {
        [HttpGet]
        [ActionRightFilter]
        public ActionResult CreateEntry()
        {
            ViewEntryModel<EntryBaseModel> model = new ViewEntryModel<EntryBaseModel>(this)
            {
                DataModel = new EntryBaseModel(),
                Handlers = new SelectList(UserManager.GetRightUsers("EntryControllerCompleteEntry"), "UserName", "DisplayName"),
            };
            return View(model);
        }

        [HttpPost]
        [ActionAjaxFilter]
        [ActionRightFilter]
        public JsonResult CreateEntry(ViewEntryModel<EntryBaseModel> model)
        {
            JsonModel jsonModel;
            try
            {
                EntryManager.CreateEntry(base.LoginAccount, model.HandlerName,
                    "~/Entry/CreateEntry", "~/Entry/CompleteEntry", model.DataModel);
                jsonModel = JsonModel.Success(SiteCommonResource.MessageOperateSuccessfully);
            }
            catch (Exception ex)
            {
                jsonModel = BaseException.HandleException(ex);
            }
            return Json(jsonModel);
        }

        [HttpGet]
        [ActionRightFilter]
        public ActionResult EditEntry(long id)
        {
            string handlerName;
            ViewEntryModel<EntryBaseModel> model = new ViewEntryModel<EntryBaseModel>(this)
            {
                DataModel = EntryManager.GetEntryForEdit(id, out handlerName),
                Handlers = new SelectList(UserManager.GetRightUsers("EntryControllerCompleteEntry"), "UserName", "DisplayName"),
                HandlerName = handlerName
            };
            return View(model);
        }

        [HttpPost]
        [ActionAjaxFilter]
        [ActionRightFilter]
        public JsonResult EditEntry(long id, ViewEntryModel<EntryBaseModel> model)
        {
            JsonModel jsonModel;
            try
            {
                EntryManager.UpdateEntry(base.LoginAccount, model.HandlerName,
                    "~/Entry/EditEntry", "~/Entry/CompleteEntry", id, model.DataModel);
                jsonModel = JsonModel.Success(SiteCommonResource.MessageOperateSuccessfully);
            }
            catch (Exception ex)
            {
                jsonModel = BaseException.HandleException(ex);
            }
            return Json(jsonModel);
        }

        [HttpGet]
        [ActionRightFilter]
        public ActionResult CompleteEntry(long id)
        {
            ViewBaseModel<EntryCompleteModel> model = new ViewBaseModel<EntryCompleteModel>(this)
            {
                DataModel = EntryManager.GetEntryForComplete(id)
            };
            return View(model);
        }

        [HttpPost]
        [ActionAjaxFilter(ValidateModelState = false)]
        [ActionRightFilter]
        public ActionResult CompleteEntry(long id, ViewBaseModel<EntryCompleteModel> model)
        {
            try
            {
                JsonModel jsonModel;
                if (Request["flowType"] == "Reject")
                {
                    TaskManager.Reject(base.LoginAccount, "~/Entry/RejectEntry", "~/Entry/EditEntry", id);
                    jsonModel = JsonModel.Success(SiteCommonResource.MessageOperateSuccessfully);
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        var errorModel = ModelState.Select(ms => new
                        {
                            key = ms.Key,
                            error = string.Join(";", ms.Value.Errors.Select(e => e.ErrorMessage))
                        });
                        jsonModel = JsonModel.Fail(string.Empty, errorModel);
                    }
                    else
                    {
                        EntryManager.CompleteEntry(base.LoginAccount, "~/Entry/CompleteEntry", id, model.DataModel);
                        jsonModel = JsonModel.Success(SiteCommonResource.MessageOperateSuccessfully);
                    }
                }
                return Json(jsonModel);
            }
            catch (Exception ex)
            {
                return Json(BaseException.HandleException(ex));
            }
        }

        #region Entry Batch Process

        //private List<EntryBaseModel> EditingRecords
        //{
        //    get
        //    {
        //        List<EntryBaseModel> tmp = Session["EditingRecords"] as List<EntryBaseModel>;
        //        if (tmp == null)
        //        {
        //            tmp = new List<EntryBaseModel>();
        //            Session["EditingRecords"] = tmp;
        //        }
        //        return tmp;
        //    }
        //    set { Session["EditingRecords"] = value; }
        //}

        private IEnumerable<EntryBaseModel> ImportExcelFile(HttpPostedFileBase fuExcel)
        {
            string ext = string.Format("{0}", Path.GetExtension(fuExcel.FileName)).ToLower().Trim('.');
            if (ext == "xls" || ext == "xlsx")
            {
                string filePath = Server.MapPath("~/Temp/UploadFiles");
                try
                {
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    filePath = Path.Combine(filePath, fuExcel.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    fuExcel.SaveAs(filePath);
                    ExcelQueryFactory factory = new ExcelQueryFactory(filePath);
                    ExcelColumnNameAttribute.AddMapping(factory, typeof(EntryBaseModel));
                    List<EntryBaseModel> models = factory.Worksheet<EntryBaseModel>(0).ToList();
                    return models;
                }
                finally
                {
                    System.IO.File.Delete(filePath);
                }
            }
            return new List<EntryBaseModel>();
        }

        [HttpPost]
        public ActionResult BatchPartialEntryForEdit(List<EntryBaseModel> models)
        {
            try
            {
                if (models == null)
                {
                    models = new List<EntryBaseModel>();
                }
                HttpPostedFileBase fuExcel = Request.Files["fuExcel"];
                if (fuExcel != null && fuExcel.ContentLength > 0)
                {
                    models.AddRange(ImportExcelFile(fuExcel));
                }
                return PartialView(models);
            }
            catch (Exception ex)
            {
                BaseException.RecordException(ex);
                return PartialView(models);
            }
        }

        [HttpGet]
        [ActionRightFilter]
        public ActionResult BatchCreateEntry()
        {
            ViewEntryModel<List<EntryBaseModel>> model = new ViewEntryModel<List<EntryBaseModel>>(this)
            {
                DataModel = new List<EntryBaseModel>(),
                Handlers = new SelectList(UserManager.GetRightUsers("EntryControllerBatchCompleteEntry"), "UserName", "DisplayName")
            };
            return View(model);
        }

        [HttpPost]
        [ActionAjaxFilter]
        [ActionRightFilter]
        public JsonResult BatchCreateEntry(ViewEntryModel<List<EntryBaseModel>> model)
        {
            JsonModel jsonModel;
            try
            {
                EntryManager.BatchCreateEntry(base.LoginAccount, model.HandlerName,
                    "~/Entry/BatchCreateEntry", "~/Entry/BatchCompleteEntry", model.DataModel);
                jsonModel = JsonModel.Success(SiteCommonResource.MessageOperateSuccessfully);
            }
            catch (Exception ex)
            {
                jsonModel = BaseException.HandleException(ex);
            }
            return Json(jsonModel);
        }

        [HttpGet]
        [ActionRightFilter]
        public ActionResult BatchEditEntry(long id)
        {
            string handlerName;
            ViewEntryModel<List<EntryBaseModel>> model = new ViewEntryModel<List<EntryBaseModel>>(this)
            {
                DataModel = EntryManager.GetEntriesForEdit(id, out handlerName),
                Handlers = new SelectList(UserManager.GetRightUsers("EntryControllerBatchCompleteEntry"), "UserName", "DisplayName"),
                HandlerName = handlerName
            };
            return View(model);
        }

        [HttpPost]
        [ActionAjaxFilter]
        [ActionRightFilter]
        public ActionResult BatchEditEntry(long id, ViewEntryModel<List<EntryBaseModel>> model)
        {

            JsonModel jsonModel;
            try
            {
                EntryManager.BatchUpdateEntry(base.LoginAccount, model.HandlerName,
                    "~/Entry/BatchEditEntry", "~/Entry/BatchCompleteEntry", id, model.DataModel);
                jsonModel = JsonModel.Success(SiteCommonResource.MessageOperateSuccessfully);
            }
            catch (Exception ex)
            {
                jsonModel = BaseException.HandleException(ex);
            }
            return Json(jsonModel);
        }

        [HttpGet]
        [ActionRightFilter]
        public ActionResult BatchCompleteEntry(long id)
        {
            ViewBaseModel<List<EntryCompleteModel>> model = new ViewBaseModel<List<EntryCompleteModel>>(this)
            {
                DataModel = EntryManager.GetEntriesForComplete(id)
            };
            return View(model);
        }

        [HttpPost]
        [ActionAjaxFilter]
        [ActionRightFilter]
        public ActionResult BatchCompleteEntry(long id, ViewBaseModel<List<EntryCompleteModel>> model)
        {
            try
            {
                if (Request["flowType"] == "Reject")
                {
                    TaskManager.Reject(base.LoginAccount, "~/Entry/BatchCompleteEntry", "~/Entry/BatchEditEntry", id);
                }
                else
                {
                    EntryManager.CompleteEntries(base.LoginAccount, "~/Entry/BatchCompleteEntry", id, model.DataModel);
                }
                JsonModel jsonModel = JsonModel.Success(SiteCommonResource.MessageOperateSuccessfully);
                return Json(jsonModel);
            }
            catch (Exception ex)
            {
                return Json(BaseException.HandleException(ex));
            }
        }

        #endregion
    }
}
