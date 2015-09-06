using System;
using System.Collections.Generic;
using System.Linq;

namespace WarehouseEntry.Database.DataAction
{
    public class PagingData
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public PagingData()
        {
            this.PageSize = 20;
        }

        public IEnumerable<TEntity> SelectTopPagingData<TEntity, TKey>(IQueryable<TEntity> query, Func<TEntity, TKey> orderFunc)
            where TEntity : class
        {
            int startRow = (this.PageIndex - 1) * this.PageSize;
            return query.OrderBy(orderFunc).Skip(startRow).Take(this.PageSize);
        }

        public IEnumerable<TEntity> SelectFootPagingData<TEntity, TKey>(IQueryable<TEntity> query, Func<TEntity, TKey> orderFunc)
            where TEntity : class
        {
            int startRow = (this.PageIndex - 1) * this.PageSize;
            return query.OrderByDescending(orderFunc).Skip(startRow).Take(this.PageSize);
        }
    }
}
