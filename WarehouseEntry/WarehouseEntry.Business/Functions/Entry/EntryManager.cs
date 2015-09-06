using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using WarehouseEntry.Business.Exceptions;
using WarehouseEntry.Business.Functions.Security;
using WarehouseEntry.Business.Functions.Task;
using WarehouseEntry.Business.Models;
using WarehouseEntry.Database.DataAction;
using WarehouseEntry.Database.DataDefine;
using WarehouseEntry.Database.DataModel;

namespace WarehouseEntry.Business.Functions.Entry
{
    public class EntryManager
    {
        public static List<string> GetProjectNames()
        {
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                return dbContext.EntryRecord.Where(r => !string.IsNullOrEmpty(r.ProjectName))
                    .Select(r => r.ProjectName).Distinct().OrderBy(n => n).ToList();
            }
        }

        private static EntryRecord ReadEntry<T>(long processId, CommonTaskSection section, out T model, long entryId = 0)
            where T : EntryBaseModel, new()
        {
            List<T> models;
            List<EntryRecord> records = ReadEntries(processId, section, out models, entryId);
            if (records.Count < 1)
            {
                throw new DatabaseObjectNotFoundException();
            }
            model = models[0];
            return records[0];
        }

        private static List<EntryRecord> ReadEntries<T>(long processId, CommonTaskSection section,
            out List<T> models, long entryId = 0)
            where T : EntryBaseModel, new()
        {
            if (processId < 1)
            {
                throw new ArgumentOutOfRangeException("processId");
            }
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                IQueryable<EntryRecord> records = dbContext.EntryRecord.Where(r => r.FlowProcess.ProcessID == processId
                    && r.FlowProcess.CurrentTaskSection == section.ToString());
                if (entryId != 0)
                {
                    records = records.Where(r => r.RecordID == entryId);
                }
                List<EntryRecord> result = records.ToList();
                models = records.Select(record => new T
                {
                    ModelId = record.RecordID,
                    ProjectName = record.ProjectName,
                    ProjectNum = record.ProjectNum,
                    ProductName = record.ProductName,
                    SuiteCount = record.SuiteCount,
                    ProductImgNum = record.ProductImgNum,
                    Sequence = record.Sequence,
                    ImgNum = record.ImgNum,
                    Height = record.Height,
                    Width = record.Width,
                    StomachWeight = record.StomachWeight,
                    WingWeight = record.WingWeight,
                    Length = record.Length,
                    PieceCount = record.PieceCount,
                    Weight = record.Weight,
                    AssemblageDate = record.AssemblageDate,
                    SolderingDate = record.SolderingDate,
                    CorrectionDate = record.CorrectionDate,
                    InspectionDate = record.InspectionDate,
                }).ToList();
                if (result.Count < 1)
                {
                    throw new DatabaseObjectNotFoundException();
                }
                if (result[0].Completer == null)
                { }
                return result;
            }
        }

        private static void WriteEntry(EntryBaseModel model, EntryRecord record)
        {
            record.ProjectName = model.ProjectName;
            record.ProjectNum = model.ProjectNum;
            record.ProductName = model.ProductName;
            record.SuiteCount = model.SuiteCount;
            record.ProductImgNum = model.ProductImgNum;
            record.Sequence = model.Sequence;
            record.ImgNum = model.ImgNum;
            record.Height = model.Height;
            record.Width = model.Width;
            record.StomachWeight = model.StomachWeight;
            record.WingWeight = model.WingWeight;
            record.Length = model.Length;
            record.PieceCount = model.PieceCount;
            record.Weight = model.Weight;
            record.AssemblageDate = model.AssemblageDate;
            record.SolderingDate = model.SolderingDate;
            record.CorrectionDate = model.CorrectionDate;
            record.InspectionDate = model.InspectionDate;
        }

        public static void CreateEntry(string currentHandler, string nextHandler,
            string currentHandleUrl, string nextHandleUrl, EntryBaseModel model)
        {
            TaskManager.CheckRequire(currentHandler, nextHandler, currentHandleUrl, nextHandleUrl);
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            using (TransactionScope t = DbContextHelper.BeginTransaction())
            {
                using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
                {
                    SecurityUser currentUser = UserManager.GetSecurityUser(dbContext, currentHandler);
                    SecurityUser nextUser = UserManager.GetSecurityUser(dbContext, nextHandler);
                    FlowTask firstTask = TaskManager.CreateFirstTask(CommonTaskSection.Create.ToString(),
                        currentUser, currentHandleUrl, "Create");
                    FlowTask nextTask = TaskManager.CreateNexTask(CommonTaskSection.Complete.ToString(),
                        nextUser, nextHandleUrl);
                    const string flowName = "Create entry record.";
                    FlowProcess flow = TaskManager.CreateProcess(firstTask, nextTask, flowName);
                    firstTask.FlowProcess = flow;
                    dbContext.FlowTask.Add(firstTask);
                    dbContext.SaveChanges();
                    flow.CurrentHandleUrl = string.Format("{0}/{1}", nextHandleUrl.Trim('/'), flow.ProcessID);
                    EntryRecord record = new EntryRecord
                    {
                        CreationTime = firstTask.HandleTime,
                        Creator = firstTask.Handler,
                        Completer = flow.CurrentHandler,
                        FlowProcess = flow
                    };
                    WriteEntry(model, record);
                    dbContext.EntryRecord.Add(record);
                    dbContext.SaveChanges();
                    DbContextHelper.CompleteTransaction(t);
                }
            }
        }

        public static EntryBaseModel GetEntryForEdit(long processId, out string handlerName)
        {
            EntryBaseModel model;
            EntryRecord record = ReadEntry(processId, CommonTaskSection.Edit, out model);
            handlerName = record.Completer == null ? string.Empty : record.Completer.UserName;
            return model;
        }

        public static void UpdateEntry(string currentHandler, string nextHandler,
            string currentHandleUrl, string nextHandleUrl, long processId, EntryBaseModel model)
        {
            TaskManager.CheckRequire(currentHandler, nextHandler, currentHandleUrl, nextHandleUrl, processId);
            if (model == null || model.ModelId < 1)
            {
                throw new ArgumentNullException("model");
            }
            using (TransactionScope t = DbContextHelper.BeginTransaction())
            {
                using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
                {
                    EntryRecord record = dbContext.EntryRecord.FirstOrDefault(r => r.FlowProcess.ProcessID == processId
                        && r.FlowProcess.CurrentTaskSection == CommonTaskSection.Edit.ToString()
                        && r.FlowProcess.CurrentHandler.UserName == currentHandler);
                    if (record == null)
                    {
                        throw new DatabaseObjectNotFoundException();
                    }
                    SecurityUser currentUser = record.FlowProcess.CurrentHandler;
                    SecurityUser nextUser = UserManager.GetSecurityUser(dbContext, nextHandler);
                    FlowTask handlingTask = TaskManager.CreateHandlingTask(record.FlowProcess, currentUser, "Edit");
                    if (!string.IsNullOrWhiteSpace(currentHandleUrl))
                    {
                        handlingTask.HandleUrl = string.Format("{0}/{1}", currentHandleUrl.Trim('/'), processId);
                    }
                    dbContext.FlowTask.Add(handlingTask);
                    dbContext.SaveChanges();
                    FlowTask nextTask = TaskManager.CreateNexTask(CommonTaskSection.Complete.ToString(),
                        nextUser, string.Format("{0}/{1}", nextHandleUrl.Trim('/'), processId));
                    TaskManager.Next(record.FlowProcess, nextTask);
                    dbContext.SaveChanges();
                    WriteEntry(model, record);
                    record.Completer = record.FlowProcess.CurrentHandler;
                    dbContext.SaveChanges();
                    DbContextHelper.CompleteTransaction(t);
                }
            }
        }

        public static EntryCompleteModel GetEntryForComplete(long processId)
        {
            EntryCompleteModel model;
            EntryRecord record = ReadEntry(processId, CommonTaskSection.Complete, out model);
            if (record.CompletionDate.HasValue)
            {
                model.CompletionDate = record.CompletionDate.Value;
            }
            if (record.DespatchDate.HasValue)
            {
                model.DespatchDate = record.DespatchDate.Value;
            }
            return model;
        }

        public static void CompleteEntry(string currentHandler, string currentHandleUrl, long processId, EntryCompleteModel model)
        {
            TaskManager.CheckRequire(currentHandler, currentHandleUrl, processId);
            if (model == null || model.ModelId < 1)
            {
                throw new ArgumentNullException("model");
            }
            using (TransactionScope t = DbContextHelper.BeginTransaction())
            {
                using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
                {
                    EntryRecord record = dbContext.EntryRecord.FirstOrDefault(r => r.FlowProcess.ProcessID == processId
                        && r.FlowProcess.CurrentTaskSection == CommonTaskSection.Complete.ToString()
                        && r.FlowProcess.CurrentHandler.UserName == currentHandler);
                    if (record == null)
                    {
                        throw new DatabaseObjectNotFoundException();
                    }
                    SecurityUser currentUser = record.FlowProcess.CurrentHandler;
                    FlowTask handlingTask = TaskManager.CreateHandlingTask(record.FlowProcess, currentUser, "Complete");
                    if (!string.IsNullOrWhiteSpace(currentHandleUrl))
                    {
                        handlingTask.HandleUrl = string.Format("{0}/{1}", currentHandleUrl.Trim('/'), processId);
                    }
                    dbContext.FlowTask.Add(handlingTask);
                    dbContext.SaveChanges();
                    TaskManager.Next(record.FlowProcess);
                    dbContext.SaveChanges();
                    WriteEntry(model, record);
                    record.CompletionDate = model.CompletionDate;
                    record.DespatchDate = model.DespatchDate;
                    record.Completer = record.FlowProcess.CurrentHandler;
                    record.CompletionTime = handlingTask.HandleTime;
                    dbContext.SaveChanges();
                    DbContextHelper.CompleteTransaction(t);
                }
            }
        }

        public static void BatchCreateEntry(string currentHandler, string nextHandler,
            string currentHandleUrl, string nextHandleUrl, List<EntryBaseModel> models)
        {
            TaskManager.CheckRequire(currentHandler, nextHandler, currentHandleUrl, nextHandleUrl);
            if (models == null || models.Count < 1)
            {
                throw new ArgumentNullException("models");
            }
            using (TransactionScope t = DbContextHelper.BeginTransaction())
            {
                using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
                {
                    SecurityUser currentUser = UserManager.GetSecurityUser(dbContext, currentHandler);
                    SecurityUser nextUser = UserManager.GetSecurityUser(dbContext, nextHandler);
                    FlowTask firstTask = TaskManager.CreateFirstTask(CommonTaskSection.Create.ToString(),
                        currentUser, currentHandleUrl, "Batch Create");
                    FlowTask nextTask = TaskManager.CreateNexTask(CommonTaskSection.Complete.ToString(),
                        nextUser, nextHandleUrl);
                    const string flowName = "Batch create entry records.";
                    FlowProcess flow = TaskManager.CreateProcess(firstTask, nextTask, flowName);
                    firstTask.FlowProcess = flow;
                    dbContext.FlowTask.Add(firstTask);
                    dbContext.SaveChanges();
                    flow.CurrentHandleUrl = string.Format("{0}/{1}", nextHandleUrl.Trim('/'), flow.ProcessID);
                    dbContext.SaveChanges();
                    foreach (EntryBaseModel model in models)
                    {
                        EntryRecord record = new EntryRecord
                        {
                            CreationTime = firstTask.HandleTime,
                            Creator = firstTask.Handler,
                            Completer = flow.CurrentHandler,
                            FlowProcess = flow
                        };
                        WriteEntry(model, record);
                        dbContext.EntryRecord.Add(record);
                    }
                    dbContext.SaveChanges();
                    DbContextHelper.CompleteTransaction(t);
                }
            }
        }


        public static List<EntryBaseModel> GetEntriesForEdit(long processId, out string handlerName)
        {
            List<EntryBaseModel> models;
            List<EntryRecord> records = ReadEntries(processId, CommonTaskSection.Edit, out models);
            handlerName = records.Count < 1 || records[0].Completer == null
                ? string.Empty
                : records[0].Completer.UserName;
            return models;
        }

        public static void BatchUpdateEntry(string currentHandler, string nextHandler,
            string currentHandleUrl, string nextHandleUrl, long processId, List<EntryBaseModel> models)
        {
            TaskManager.CheckRequire(currentHandler, nextHandler, currentHandleUrl, nextHandleUrl, processId);
            int listCount = models.Count;
            if (models == null || listCount < 1)
            {
                throw new ArgumentNullException("models");
            }
            using (TransactionScope t = DbContextHelper.BeginTransaction())
            {
                using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
                {
                    List<EntryRecord> records = dbContext.EntryRecord.Where(r => r.FlowProcess.ProcessID == processId
                        && r.FlowProcess.CurrentTaskSection == CommonTaskSection.Edit.ToString()
                        && r.FlowProcess.CurrentHandler.UserName == currentHandler).OrderBy(r => r.RecordID).ToList();
                    if (records.Count != listCount)
                    {
                        throw new DatabaseObjectNotFoundException();
                    }
                    EntryRecord firstRecord = records[0];
                    SecurityUser currentUser = firstRecord.FlowProcess.CurrentHandler;
                    SecurityUser nextUser = UserManager.GetSecurityUser(dbContext, nextHandler);
                    FlowTask handlingTask = TaskManager.CreateHandlingTask(firstRecord.FlowProcess, currentUser, "Edit");
                    if (!string.IsNullOrWhiteSpace(currentHandleUrl))
                    {
                        handlingTask.HandleUrl = string.Format("{0}/{1}", currentHandleUrl.Trim('/'), processId);
                    }
                    dbContext.FlowTask.Add(handlingTask);
                    dbContext.SaveChanges();
                    FlowTask nextTask = TaskManager.CreateNexTask(CommonTaskSection.Complete.ToString(),
                        nextUser, string.Format("{0}/{1}", nextHandleUrl.Trim('/'), processId));
                    TaskManager.Next(firstRecord.FlowProcess, nextTask);
                    dbContext.SaveChanges();
                    models = models.OrderBy(m => m.ModelId).ToList();
                    for (int i = 0; i < listCount; i++)
                    {
                        EntryRecord record = records[i];
                        WriteEntry(models[i], record);
                        record.Completer = record.FlowProcess.CurrentHandler;
                    }
                    dbContext.SaveChanges();
                    DbContextHelper.CompleteTransaction(t);
                }
            }
        }

        public static List<EntryCompleteModel> GetEntriesForComplete(long processId)
        {
            List<EntryCompleteModel> models;
            ReadEntries(processId, CommonTaskSection.Complete, out models);
            return models;
        }

        public static void CompleteEntries(string currentHandler, string currentHandleUrl,
            long processId, List<EntryCompleteModel> models)
        {
            TaskManager.CheckRequire(currentHandler, currentHandleUrl, processId);
            int listCount = models.Count;
            if (models == null || listCount < 1)
            {
                throw new ArgumentNullException("models");
            }
            using (TransactionScope t = DbContextHelper.BeginTransaction())
            {
                using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
                {
                    List<EntryRecord> records = dbContext.EntryRecord.Where(r => r.FlowProcess.ProcessID == processId
                        && r.FlowProcess.CurrentTaskSection == CommonTaskSection.Complete.ToString()
                        && r.FlowProcess.CurrentHandler.UserName == currentHandler).OrderBy(r => r.RecordID).ToList();
                    if (records.Count != listCount)
                    {
                        throw new DatabaseObjectNotFoundException();
                    }
                    EntryRecord firstRecord = records[0];
                    SecurityUser currentUser = firstRecord.FlowProcess.CurrentHandler;
                    FlowTask handlingTask = TaskManager.CreateHandlingTask(firstRecord.FlowProcess, currentUser, "Complete");
                    if (!string.IsNullOrWhiteSpace(currentHandleUrl))
                    {
                        handlingTask.HandleUrl = string.Format("{0}/{1}", currentHandleUrl.Trim('/'), processId);
                    }
                    dbContext.FlowTask.Add(handlingTask);
                    dbContext.SaveChanges();
                    TaskManager.Next(firstRecord.FlowProcess);
                    dbContext.SaveChanges();
                    for (int i = 0; i < listCount; i++)
                    {
                        EntryCompleteModel model = models[i];
                        EntryRecord record = records[i];
                        WriteEntry(model, record);
                        record.CompletionDate = model.CompletionDate;
                        record.DespatchDate = model.DespatchDate;
                        record.Completer = record.FlowProcess.CurrentHandler;
                        record.CompletionTime = handlingTask.HandleTime;
                    }
                    dbContext.SaveChanges();
                    DbContextHelper.CompleteTransaction(t);
                }
            }
        }
    }
}