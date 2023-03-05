
using System.Data;
using System.Diagnostics;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;


// Tạo đối tượng DbContext để quản lý phiên làm việc // với CSDL và trạng thái của các đối tượng
var context = new BlogDbContext();

IBlogRepository blogRepo = new BlogRepository(context);

//var pagingParams = new PagingParams
//{
//    PageNumber = 1,
//    PageSize = 5,
//    SortColumn = "Name",
//    SortOrder = "desc",
//};

var item = await blogRepo.FindBySlugAsync<Tag>("google");

Console.WriteLine("{0,-5}{1,-50}{2,10}",
    "ID", "NAME", "Count");


Console.WriteLine("{0,-5}{1,-50}{2,10}",
    item.Id, item.Name, item.Description);


