

using TatBlog.Core.Contracts;

namespace TatBlog.WinApp
{
    internal class PagingParams : IPagingParams
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string SortColumn { get; set; } = "ID";
        public string SortOrder { get; set; } = "ASC";
    }
}
