namespace TatBlog.WebApp.Extensions
{
	public static class RouteExtensions
	{
		// Định nghĩa route template, route constraint cho các
		// endpoint kết hợp với các action trong các controller
		public static IEndpointRouteBuilder UseBlogRoutes(
			this IEndpointRouteBuilder endpoints) 
		{
			
			endpoints.MapControllerRoute(
				name: "posts-by-category",
				pattern: "blog/category/{slug}",
				defaults: new { controller = "Blog", action = "Category" });

			endpoints.MapControllerRoute(
<<<<<<< HEAD
				name: "posts-by-author",
				pattern: "blog/author/{slug}",
				defaults: new { controller = "Blog", action = "Author" });

			endpoints.MapControllerRoute(
=======
>>>>>>> 8f78ca59d326612ec5d6d800c3a2375fe0af6af1
				name: "posts-by-tag",
				pattern: "blog/tag/{slug}",
				defaults: new { controller = "Blog", action = "Tag" });

			endpoints.MapControllerRoute(
				name: "single-post",
				pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
				defaults: new { controller = "Blog", action = "Post" });

			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{Controller=Blog}/{action=Index}/{id?}");

			return endpoints;
		}
	}
}
