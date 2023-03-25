
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        // Tìm bài viết có tên định danh là "slug"
        // và được đăng vào tháng 'month' năm year'

        Task<IList<Post>> GetPostsAsync(
        PostQuery condition,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

        Task<Post> GetPostAsync(
        string slug,
        CancellationToken cancellationToken = default);

        Task<Post> GetPostDetailAsync(
        PostQuery condition,
        CancellationToken cancellationToken = default);

        Task<Author> GetAuthorAsync(
            string slug,
            CancellationToken cancellationToken = default);

        Task<Author> GetAuthorByIdAsync(int authorId);

        Task<IList<AuthorItem>> GetAuthorsAsync(
            CancellationToken cancellationToken = default);


        // Tìm Top N bài viết phổ được nhiều người xem nhất
        Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts, 
            CancellationToken cancellationToken = default);

        Task<IList<Author>> GetPopularAuthorsAsync(
            int numAuthors,
            CancellationToken cancellationToken = default);


		Task<IList<Post>> GetRandomArticlesAsync(
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

        Task<Tag> GetTagAsync(
        string slug, CancellationToken cancellationToken = default);
		// Lấy danh sách tất cả các thẻ
		Task<IList<TagItem>> GetTagsAsync(
            CancellationToken cancellationToken = default);

        // Xóa một thẻ,danh mục theo mã cho trước.
        Task DeleteByIdAsync<T>(int id) where T : class, new();

        // Thêm hoặc cập nhật một chuyên mục/chủ đề.
        Task<Category> CreateOrUpdateCategoryAsync(
        Category category, CancellationToken cancellationToken = default);

		// Kiểm tra tên định danh (slug) của một chuyên mục đã tồn tại hay chưa.
		Task<bool> IsSlugExists(string slug);

        //Lấy và phân trang danh sách chuyên mục
        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        // Đếm số lượng bài viết trong N tháng gần nhất
        Task<IList<MonthlyPostCount>> GetMonthlyPostCountsAsync(int months);

        // Thêm hay cập nhật một bài viết. 
        Task<Post> CreateOrUpdatePostAsync(
        Post post, IEnumerable<string> tags,
        CancellationToken cancellationToken = default);



        // Chuyển đổi trạng thái Published của bài viết.
        Task<bool> TogglePublishedFlagAsync(
        int postId, CancellationToken cancellationToken = default);

        // Tìm và phân trang các bài viết thỏa mãn điều kiện tìm kiếm 
        Task<IPagedList<Post>> GetPagedPostsAsync(
            PostQuery condition,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);

        // t.
        Task<IPagedList<T>> GetPagedPostsAsync<T>(
            PostQuery condition,
            IPagingParams pagingParams,
            Func<IQueryable<Post>, IQueryable<T>> mapper);

        Task<Post> GetPostByIdAsync(
        int postId, bool includeDetails = false,
        CancellationToken cancellationToken = default);


        Task<List<Post>> GetRelatedPostsAsync(
            int postId, int categoryId);

	}
}
