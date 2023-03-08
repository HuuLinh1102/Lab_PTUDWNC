
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Diagnostics;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;


// Tạo đối tượng DbContext để quản lý phiên làm việc // với CSDL và trạng thái của các đối tượng

//var context = new BlogDbContext();
//IBlogRepository blogRepo = new BlogRepository(context);

//var query = new PostQuery
//{
//	Year = 2022,
//};

//var pageNumber = 1;
//var pageSize = 10;

////var posts = await blogRepo.GetPopularArticlesAsync(5);
//var posts = await blogRepo.GetPagedPostsAsync(query,pageNumber,pageSize);

//foreach (var post in posts)
//{
//	Console.WriteLine("ID        : {0}",post.Id);
//	Console.WriteLine("Title     : {0}", post.Title);
//	Console.WriteLine("Author    : {0}", post.Author.FullName);
//	Console.WriteLine("Category  : {0}", post.Category.Name);
	
	Console.WriteLine("".PadRight(80, '-'));

//}

