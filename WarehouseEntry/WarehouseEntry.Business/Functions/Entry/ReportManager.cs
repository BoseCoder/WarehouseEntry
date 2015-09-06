using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseEntry.Database.DataAction;

namespace WarehouseEntry.Business.Functions.Entry
{
    public class ReportManager
    {
        public static Dictionary<string, decimal> GetWeightReport(DateTime? startDate, DateTime? endDate, bool completed)
        {
            return ReportData.SelectWeightReport(startDate, endDate, completed);
        }

        public static List<Tuple<string, bool>> GetProductReport(DateTime? startDate, DateTime? endDate, string projectName)
        {
            return ReportData.SelectProductReport(startDate, endDate, projectName)
                .Select(r => new Tuple<string, bool>(r.ProductName, r.CompletionDate != null)).ToList();
        }
    }
}
