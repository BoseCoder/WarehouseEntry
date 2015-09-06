using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseEntry.Database.DataModel;

namespace WarehouseEntry.Database.DataAction
{
    public static class ReportData
    {
        public static Dictionary<string, decimal> SelectWeightReport(DateTime? startDate, DateTime? endDate, bool completed)
        {
            if (startDate.HasValue)
            {
                startDate = startDate.Value.Date;
            }
            if (endDate.HasValue)
            {
                endDate = endDate.Value.Date.AddDays(1);
            }
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                return dbContext.EntryRecord.Where(r => (!completed || r.CompletionTime != null)
                    && (startDate == null || r.CreationTime >= startDate)
                    && (endDate == null || r.CreationTime < endDate))
                    .GroupBy(r => r.ProjectName, (key, g) => new
                    {
                        projectName = key,
                        subTotal = g.Sum(gr => gr.Weight * gr.PieceCount)
                    }).ToDictionary(g => g.projectName, g => g.subTotal);
            }
        }

        public static List<EntryRecord> SelectProductReport(DateTime? startDate, DateTime? endDate, string projectName)
        {
            if (startDate.HasValue)
            {
                startDate = startDate.Value.Date;
            }
            if (endDate.HasValue)
            {
                endDate = endDate.Value.Date.AddDays(1);
            }
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                IQueryable<EntryRecord> query = dbContext.EntryRecord.Where(r =>
                    (startDate == null || r.CreationTime >= startDate)
                    && (endDate == null || r.CreationTime < endDate));
                if (!string.IsNullOrWhiteSpace(projectName))
                {
                    query = query.Where(r => r.ProjectName == projectName);
                }
                return query.ToList();
            }
        }
    }
}
