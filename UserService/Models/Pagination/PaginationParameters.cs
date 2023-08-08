using UserService.Exceptions;

namespace UserService.Models.Pagination
{
    public class PaginationParameters
    {
        public PaginationParameters(int? pageIndex, int? pageSize)
        {
            if (pageSize == null || pageIndex == null)
            {
                PageIndex = pageIndex ?? 1;
                PageSize = pageSize ?? int.MaxValue;
            }
            else
            {
                ValidateParameters(pageIndex.Value, pageSize.Value);
                
                PageIndex = pageIndex.Value;
                PageSize = pageSize.Value;
            }
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        private static void ValidateParameters(int pageIndex, int pageSize)
        {
            if (pageSize <= 0)
            {
                throw new InternalException("Page size can not be 0 or less then 0.");
            }

            if (pageIndex <= 0)
            {
                throw new InternalException("Page index can not be 0 or less then 0.");
            }
        }
    }
}
