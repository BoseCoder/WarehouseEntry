using WarehouseEntry.Business.Languages;
using WarehouseEntry.Business.Models;

namespace WarehouseEntry.Business.Exceptions
{
    /// <summary>
    /// 错误异常
    /// </summary>
    public class ErrorException : BaseException
    {
        private readonly object[] _paramValues;

        public ErrorException(string msgCode)
            : base(msgCode)
        { }

        public ErrorException(string msgCode, params object[] paramValues)
            : base(msgCode)
        {
            this._paramValues = paramValues;
        }

        /// <summary>
        /// 处理方法
        /// </summary>
        public override JsonModel Handle()
        {
            string msg = BusinessErrorResource.ResourceManager.GetString(base.MsgCode);
            if (string.IsNullOrWhiteSpace(msg))
            {
                msg = BusinessErrorResource.Unknow;
            }
            if (this._paramValues != null && this._paramValues.Length > 0)
            {
                return JsonModel.Fail(string.Format(msg, this._paramValues));
            }
            return JsonModel.Fail(msg);
        }
    }

    public class DatabaseObjectNotFoundException : ErrorException
    {
        public DatabaseObjectNotFoundException()
            : base("DatabaseObjectNotFound")
        { }
    }
}
