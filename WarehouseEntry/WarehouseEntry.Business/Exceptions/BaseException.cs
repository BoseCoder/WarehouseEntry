using System;
using log4net;
using WarehouseEntry.Business.Languages;
using WarehouseEntry.Business.Models;

namespace WarehouseEntry.Business.Exceptions
{
    /// <summary>
    /// 异常基类
    /// </summary>
    public abstract class BaseException : Exception
    {
        protected readonly string MsgCode;
        public delegate void ExternalHandlerDelegate(JsonModel jsonModel);
        public static ExternalHandlerDelegate ExternalHandler;
        public static readonly ILog ExceptionLogger;

        static BaseException()
        {
            ExceptionLogger = LogManager.GetLogger("ExceptionLogger");
        }

        protected BaseException(string errorCode)
        {
            MsgCode = errorCode;
        }

        /// <summary>
        /// 处理方法
        /// </summary>
        public virtual JsonModel Handle()
        {
            string msg = BusinessErrorResource.ResourceManager.GetString(this.MsgCode);
            if (string.IsNullOrWhiteSpace(msg))
            {
                msg = BusinessErrorResource.Unknow;
            }
            return JsonModel.Fail(msg);
        }

        /// <summary>
        /// 处理未知异常
        /// </summary>
        /// <param name="ex">未知异常</param>
        public static JsonModel HandleUnknowException(Exception ex)
        {
            return JsonModel.Fail(BusinessErrorResource.Unknow);
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="ex">异常</param>
        public static JsonModel HandleException(Exception ex)
        {
            BaseException knowException = ex as BaseException;
            JsonModel jsonModel = knowException != null ? knowException.Handle() : HandleUnknowException(ex);
            if (ExternalHandler != null)
            {
                ExternalHandler(jsonModel);
            }
            return jsonModel;
        }

        public static void RecordException(Exception ex)
        {
            ExceptionLogger.Error(ex.Message, ex);
        }
    }
}
