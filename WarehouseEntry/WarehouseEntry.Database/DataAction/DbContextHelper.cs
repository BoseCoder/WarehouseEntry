using System.Transactions;
using WarehouseEntry.Database.DataModel;

namespace WarehouseEntry.Database.DataAction
{
    /// <summary>
    /// DbContext管理器
    /// </summary>
    public class DbContextHelper
    {
        /// <summary>
        /// 创建DbContext对象
        /// </summary>
        /// <returns></returns>
        public static WarehouseEntryDbContext CreateDbContext()
        {
            return new WarehouseEntryDbContext();
        }

        /// <summary>
        /// 创建TransactionScope对象
        /// </summary>
        /// <returns></returns>
        public static TransactionScope BeginTransaction()
        {
            return new TransactionScope();
        }

        /// <summary>
        /// 提交TransactionScope
        /// </summary>
        /// <param name="t"></param>
        public static void CompleteTransaction(TransactionScope t)
        {
            if (t != null)
            {
                t.Complete();
                t.Dispose();
            }
        }
    }
}
