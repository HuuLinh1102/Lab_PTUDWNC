
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        // Tìm bài viết có tên định danh là "slug"
        // và được đăng vào tháng 'month' năm year'

        Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default);

        // Tìm Top N bài viết phổ được nhiều người xem nhất
        Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts, 
            CancellationToken cancellationToken = default);

        // Kiểm tra xem tên định danh của bài viết đã có hay chưa

        Task<bool> IsPostSlugExistedAsync(
            int postId, string slug, 
            CancellationToken cancellationToken = default); 
        // Tăng số lượt xem của một bài viết
        Task IncreaseViewCountAsync(
            int postId, 
            CancellationToken cancellationToken = default);

        //Lấy danh sách chuyên mục và số lượng bài viết
        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false, 
            CancellationToken cancellationToken = default);

        // Lấy danh sách từ khóa/thẻ và phân trang theo
        // các tham số pagingParams
        Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams, 
            CancellationToken cancellationToken = default);


        // Tìm một thẻ, chuyên mục, bài viết theo tên định danh (slug)
        Task<T> FindBySlugAsync<T>(string slug) where T : class, IEntity;

        // Tìm một thẻ, chuyên mục, bài viết theo id
        Task<T> FindByIdAsync<T>(int id) where T : class, IEntity;


        // Lấy danh sách tất cả các thẻ
        Task<IList<TagItem>> GetTagsAsync(
            CancellationToken cancellationToken = default);

        // Xóa một thẻ,danh mục theo mã cho trước.
        Task DeleteByIdAsync<T>(int id) where T : class, new();

        // Thêm hoặc cập nhật một chuyên mục/chủ đề.
        Task<Category> AddOrUpdateCategoryAsync(int id, 
            string name, 
            string description);

        // Kiểm tra tên định danh (slug) của một chuyên mục đã tồn tại hay chưa.
        Task<bool> IsSlugExists(string slug);

        //Lấy và phân trang danh sách chuyên mục
        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        // Đếm số lượng bài viết trong N tháng gần nhất
        Task<IList<MonthlyPostCount>> GetMonthlyPostCountsAsync(int months);
    }
}
