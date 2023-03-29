

namespace TatBlog.Core.Contracts
{
    public interface IPagedList
    {
        //Tổng số trang(số tập con)
        int PageCount { get; }
        //Tổng số phần tử trở về từ truy vấn
        int TotalItemCount { get; } 
        // Chỉ số trang hiện tại (bắt đầu từ 8)
        int PageIndex { get; }
        //Vị trí của trang hiện tại(bắt đầu từ 1)
        int PageNumber { get; } 
        // số lượng phần từ tối đa trên 1 trang
        int PageSize { get; } 
        // Kiểm tra có trang trước hay không 
        bool HasPreviousPage { get; } 
        // Kiểm tra có trang tiếp theo hay không 
        bool HasNextPage { get; }
        // Trang hiện tại có phải là trang đầu tiên không
        bool IsFirstPage { get; }
        // Trang hiện tại có phải là trang cuối cùng không
        bool IsLastPage { get; }
        // Thứ tự của phần tử đầu trang trong truy vấn (bắt đầu từ 1)
        int FirstItemIndex { get; }
        // Thứ tự của phần từ cuối trang trong truy vấn (bd từ 1)
        int LastItemIndex { get; }
    }

    public interface IPagedList<out T> : IPagedList, IEnumerable<T>
    {
        T this[int index] { get; }
        int Count { get; }
    }    
}
