using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using WarehouseEntry.Business.Exceptions;
using WarehouseEntry.Business.Models;
using WarehouseEntry.Database.DataAction;
using WarehouseEntry.Database.DataDefine;
using WarehouseEntry.Database.DataModel;

namespace WarehouseEntry.Business.Functions.Task
{
    public class TaskManager
    {
        public static void CheckRequire(string currentHandler, string currentHandleUrl)
        {
            if (string.IsNullOrWhiteSpace(currentHandler))
            {
                throw new ArgumentNullException("currentHandler");
            }
            if (string.IsNullOrWhiteSpace(currentHandleUrl))
            {
                throw new ArgumentNullException("currentHandleUrl");
            }
        }

        public static void CheckRequire(string currentHandler, string currentHandleUrl, long processId)
        {
            CheckRequire(currentHandler, currentHandleUrl);
            if (processId < 1)
            {
                throw new ArgumentOutOfRangeException("processId");
            }
        }

        public static void CheckRequire(string currentHandler, string nextHandler,
            string currentHandleUrl, string nextHandleUrl)
        {
            if (string.IsNullOrWhiteSpace(currentHandler))
            {
                throw new ArgumentNullException("currentHandler");
            }
            if (string.IsNullOrWhiteSpace(nextHandler))
            {
                throw new ArgumentNullException("nextHandler");
            }
            if (string.IsNullOrWhiteSpace(currentHandleUrl))
            {
                throw new ArgumentNullException("currentHandleUrl");
            }
            if (string.IsNullOrWhiteSpace(nextHandleUrl))
            {
                throw new ArgumentNullException("nextHandleUrl");
            }
        }

        public static void CheckRequire(string currentHandler, string nextHandler,
            string currentHandleUrl, string nextHandleUrl, long processId)
        {
            CheckRequire(currentHandler, nextHandler, currentHandleUrl, nextHandleUrl);
            if (processId < 1)
            {
                throw new ArgumentOutOfRangeException("processId");
            }
        }

        public static FlowTask CreateFirstTask(string section, SecurityUser handler, string url, string taskName)
        {
            if (string.IsNullOrWhiteSpace(section))
            {
                throw new ArgumentNullException("section");
            }
            if (null == handler)
            {
                throw new ArgumentNullException("handler");
            }
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }
            if (string.IsNullOrWhiteSpace(taskName))
            {
                throw new ArgumentNullException("taskName");
            }
            return new FlowTask
            {
                SectionName = section,
                Handler = handler,
                HandleUrl = url,
                HandleTime = DateTime.Now,
                Name = taskName
            };
        }

        public static FlowTask CreateNexTask(string section, SecurityUser handler, string url)
        {
            if (string.IsNullOrWhiteSpace(section))
            {
                throw new ArgumentNullException("section");
            }
            if (null == handler)
            {
                throw new ArgumentNullException("handler");
            }
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }
            return new FlowTask
            {
                SectionName = section,
                Handler = handler,
                HandleUrl = url,
                HandleTime = DateTime.Now
            };
        }

        public static FlowTask CreateHandlingTask(FlowProcess process, SecurityUser handler, string taskName)
        {
            if (process == null)
            {
                throw new ArgumentNullException("process");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            if (process.CurrentHandler.UserName != handler.UserName)
            {
                throw new ErrorException("HandlerNoRight");
            }
            if (string.IsNullOrWhiteSpace(taskName))
            {
                throw new ArgumentNullException("taskName");
            }
            return new FlowTask
            {
                FlowProcess = process,
                SectionName = process.CurrentTaskSection,
                Handler = process.CurrentHandler,
                HandleUrl = process.CurrentHandleUrl,
                HandleTime = DateTime.Now,
                Name = taskName
            };
        }

        public static FlowProcess CreateProcess(FlowTask firstTask, FlowTask nexTask, string flowName)
        {
            if (firstTask == null)
            {
                throw new ArgumentNullException("firstTask");
            }
            if (string.IsNullOrWhiteSpace(flowName))
            {
                throw new ArgumentNullException("flowName");
            }
            FlowProcess flow = new FlowProcess
            {
                Name = flowName,
                SetupTime = DateTime.Now
            };
            if (nexTask == null)
            {
                flow.Status = (int)FlowStatus.Complete;
                flow.CompleteTime = flow.SetupTime;
            }
            else
            {
                flow.Status = (int)FlowStatus.Start;
                flow.CurrentHandleUrl = nexTask.HandleUrl;
                flow.CurrentTaskSection = nexTask.SectionName;
                flow.CurrentHandler = nexTask.Handler;
                nexTask.FlowProcess = flow;
            }
            firstTask.HandleTime = flow.SetupTime;
            firstTask.FlowProcess = flow;
            return flow;
        }

        public static void Next(FlowProcess flow, FlowTask nexTask = null)
        {
            if (flow == null)
            {
                throw new ArgumentNullException("flow");
            }
            if (nexTask == null)
            {
                flow.Status = (int)FlowStatus.Complete;
                flow.CompleteTime = flow.SetupTime;
                flow.CurrentHandleUrl = string.Empty;
                flow.CurrentTaskSection = string.Empty;
                flow.CurrentHandler = null;
            }
            else
            {
                flow.Status = (int)FlowStatus.Processing;
                flow.CurrentHandleUrl = nexTask.HandleUrl;
                flow.CurrentTaskSection = nexTask.SectionName;
                flow.CurrentHandler = nexTask.Handler;
            }
        }

        public static void Reject(string currentHandler, string currentHandleUrl, string rejectUrl, long processId)
        {
            if (string.IsNullOrWhiteSpace(currentHandler))
            {
                throw new ArgumentNullException("currentHandler");
            }
            if (processId < 1)
            {
                throw new ArgumentOutOfRangeException("processId");
            }
            using (TransactionScope t = DbContextHelper.BeginTransaction())
            {
                using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
                {
                    FlowProcess process = dbContext.FlowProcess.FirstOrDefault(p => p.ProcessID == processId);
                    if (process == null)
                    {
                        throw new DatabaseObjectNotFoundException();
                    }
                    SecurityUser currentUser = process.CurrentHandler;
                    FlowTask lastTask = dbContext.FlowTask.Where(ft => ft.FlowProcess.ProcessID == processId)
                        .OrderByDescending(ft => ft.TaskID).First();
                    SecurityUser nextUser = lastTask.Handler;
                    FlowTask handlingTask = CreateHandlingTask(process, currentUser, "Reject");
                    if (!string.IsNullOrWhiteSpace(currentHandleUrl))
                    {
                        handlingTask.HandleUrl = string.Format("{0}/{1}", currentHandleUrl.Trim('/'), processId);
                    }
                    dbContext.FlowTask.Add(handlingTask);
                    dbContext.SaveChanges();
                    FlowTask nextTask = CreateNexTask(CommonTaskSection.Edit.ToString(),
                        nextUser, string.Format("{0}/{1}", rejectUrl.Trim('/'), processId));
                    Next(process, nextTask);
                    dbContext.SaveChanges();
                    DbContextHelper.CompleteTransaction(t);
                }
            }
        }

        public static List<TaskModel> GetTaskList(string handler, string taskNameKey)
        {
            if (string.IsNullOrWhiteSpace(handler))
            {
                throw new ArgumentNullException("handler");
            }
            using (WarehouseEntryDbContext dbContext = DbContextHelper.CreateDbContext())
            {
                IQueryable<FlowProcess> query = dbContext.FlowProcess.Where(p => p.CurrentHandler.UserName == handler);
                if (!string.IsNullOrWhiteSpace(taskNameKey))
                {
                    taskNameKey = taskNameKey.Trim();
                    query = query.Where(p => p.Name.Contains(taskNameKey));
                }
                return query.ToList().Select(delegate(FlowProcess p)
                {
                    IQueryable<EntryRecord> records = dbContext.EntryRecord.Where(r => r.FlowProcess.ProcessID == p.ProcessID);
                    string taskName = string.Join(", ", records.Take(5).ToList()
                        .Select(r => string.Format("{0} -- {1}", r.ProjectName, r.ProductName)));
                    if (records.Count() > 5)
                    {
                        taskName = taskName + "...";
                    }
                    return new TaskModel
                    {
                        TaskName = taskName,
                        TaskUrl = p.CurrentHandleUrl,
                        CurrentHandler = new BusinessUserModel(p.CurrentHandler),
                        SetupTime = p.SetupTime
                    };
                }).ToList();
            }
        }
    }
}
