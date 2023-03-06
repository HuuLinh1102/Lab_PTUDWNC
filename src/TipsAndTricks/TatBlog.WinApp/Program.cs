
using Microsoft.EntityFrameworkCore;
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
var category = context.Categories.Find(10);
var author = context.Authors.Find(8);
var tag1 = context.Tags.Find(13);
var tag2 = context.Tags.Find(14);
var tag3 = context.Tags.Find(10);

var post = new Post()
{
    Title = "The Future of Space Travel", 
    ShortDescription = "What's next for the exploration of the cosmos?", 
    Description = "With private companies like SpaceX and Blue Origin pushing the boundaries of space travel, " +
    "the future looks exciting for space exploration. Find out what's next for humanity's journey into the cosmos.", 
    Meta = "space travel, exploration, technology",
    Published = true,
    PostedDate = new DateTime(2022, 10, 6, 10, 20, 0),
    ModifiedDate = null,
    ViewCount = 0,
    Author = author,
    Category = category,
    Tags = new List<Tag>()
                    {
                        tag1, tag2, tag3
                    }
};


//await blogRepo.AddOrUpdatePostAsync(post);

//Console.WriteLine("{0,-5}{1,-50}{2,10}",
//    "ID", "NAME", "Count");

//foreach( var item in categories)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,10}",
//        item.Id, item.Name, item.PostCount);
//}    



