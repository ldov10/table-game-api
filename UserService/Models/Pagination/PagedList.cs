using System.Collections.Generic;

namespace UserService.Models.Pagination
{
    public class PagedList<TItem>
    {
        public PagedList() { }

        public PagedList(IList<TItem> items, long totalCount, PaginationParameters pagination)
        {
            Items = items;
            TotalCount = totalCount;
            PageSize = pagination.PageSize;
            PageIndex = pagination.PageIndex;
        }

        public IList<TItem> Items { get; set; }
        public long TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
