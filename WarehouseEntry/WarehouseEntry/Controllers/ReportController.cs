using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Resources;
using WarehouseEntry.Business.Functions.Entry;
using WarehouseEntry.Business.Models;
using WarehouseEntry.Controllers.Attributes;
using WarehouseEntry.Models;

namespace WarehouseEntry.Controllers
{
    public class ReportController : BaseController
    {
        [HttpGet]
        public ActionResult WeightReport()
        {
            ViewBaseModel model = new ViewBaseModel(this);
            return View(model);
        }

        [HttpPost]
        [ActionAjaxFilter]
        public ActionResult WeightReportData(DateTime? startDate, DateTime? endDate, bool completed)
        {
            Dictionary<string, decimal> data = ReportManager.GetWeightReport(startDate, endDate, completed);
            var model = new
            {
                aggregate = data.Sum(d => d.Value),
                chartData = new
                {
                    labels = data.Keys.ToArray(),
                    datasets = new[] { new { data = data.Values.ToArray() } }
                }
            };
            JsonModel jsonModel = JsonModel.Success();
            jsonModel.ObjectModel = model;
            return Json(jsonModel);
        }

        [HttpGet]
        public ActionResult ProductReport()
        {
            List<KeyValuePair<string, string>> projectNames = EntryManager.GetProjectNames()
                .Select(n => new KeyValuePair<string, string>(n, n)).ToList();
            projectNames.Insert(0, new KeyValuePair<string, string>(string.Empty, SiteCommonResource.TextAll));
            ViewProductReportModel model = new ViewProductReportModel(this)
            {
                ProjectNames = new SelectList(projectNames, "Key", "Value")
            };
            return View(model);
        }

        [HttpPost]
        [ActionAjaxFilter]
        public ActionResult ProductReportData(DateTime? startDate, DateTime? endDate, string projectName)
        {
            List<Tuple<string, bool>> data = ReportManager.GetProductReport(startDate, endDate, projectName);
            string[][] chartDataDetail =
            {
                data.Where(d => d.Item2).Select(d => d.Item1).ToArray(),
                data.Where(d => !d.Item2).Select(d => d.Item1).ToArray()
            };
            int[] chartData = chartDataDetail.Select(s => s.Count()).ToArray();
            var model = new
            {
                aggregate = chartData.Sum(),
                chartData = new
                {
                    labels = new[] { SiteCommonResource.TextCompleted, SiteCommonResource.TextUncompleted },
                    datasets = new[]
                    {
                        new
                        {
                            data = chartData,
                            dataDetail = chartDataDetail.Select(ns => string.Join(", ", ns.Distinct())).ToArray()
                        }
                    }
                }
            };
            JsonModel jsonModel = JsonModel.Success();
            jsonModel.ObjectModel = model;
            return Json(jsonModel);
        }
    }
}