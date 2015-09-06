using WarehouseEntry.Business.Models;

namespace WarehouseEntry.Business.Exceptions
{
    /// <summary>
    /// 消息异常
    /// </summary>
    public class InfoException : BaseException
    {
        public InfoException(string errorCode)
            : base(errorCode)
        { }

        public override JsonModel Handle()
        {
            return base.Handle();
        }
    }
}
