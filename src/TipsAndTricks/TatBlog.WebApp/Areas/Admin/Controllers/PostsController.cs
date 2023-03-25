using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IBlogRepository _blogRepository;
        private readonly IMediaManager _mediaManager;
        private readonly IMapper _mapper;

        public PostsController(
            ILogger<PostsController> logger,
            IBlogRepository blogRepository,
            IMediaManager mediaManager,
            IMapper mapper)
        {
            _logger = logger;
            _blogRepository = blogRepository;
            _mediaManager = mediaManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(
            PostFilterModel model,
            [FromQuery(Name = "k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10)
        {
            _logger.LogInformation("Tạo điều kiện truy vấn");


            var postQuery = _mapper.Map<PostQuery>(model);

            _logger.LogInformation("Lấy danh sách bài viết từ CSDL");

            ViewBag.PostsList = await _blogRepository
                .GetPagedPostsAsync(postQuery, pageNumber, pageSize);

            _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

            //await PopulatePostFilterModelAsync(model);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0)
        {
            // ID = 0 <=> Thêm bài viết
            // ID > 0 : Đọc dữ liệu của bài viết từ CSDL
            var post = id > 0
                ? await _blogRepository.GetPostByIdAsync(id, true)
                : null;

            // Tạo view model từ dữ liệu của bài viết
            var model = post == null
                ? new PostEditModel()
                : _mapper.Map<PostEditModel>(post);

            // Gán các giá trị khác cho view model
            await PopulatePostEditModeAsync(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            [FromServices]IValidator<PostEditModel> postValidator,
            PostEditModel model)
        {
            var validationResult = await postValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
            }    

            if (!ModelState.IsValid)
            {
                await PopulatePostEditModeAsync(model);
                return View(model);
            }

            var post = model.Id > 0
                ? await _blogRepository.GetPostByIdAsync(model.Id)
                : null;

            if (post == null)
            {
                post = _mapper.Map<Post>(model);

                post.Id = 0;
                post.PostedDate = DateTime.Now;
            }    
            else
            {
                _mapper.Map(model, post);

                post.Category = null;
                post.ModifiedDate = DateTime.Now;
            }

            // Nếu người dùng có upload hình ảnh minh họa cho bài viết
            if (model.ImageFile?.Length > 0)
            {
                var newImagePath = await _mediaManager.SaveFileAsync(
                    model.ImageFile.OpenReadStream(),
                    model.ImageFile.FileName,
                    model.ImageFile.ContentType);

                if (!string.IsNullOrWhiteSpace(newImagePath))
                {
                    await _mediaManager.DeleteFileAsync(post.ImageUrl);
                    post.ImageUrl = newImagePath;
                }    
            }    

            await _blogRepository.CreateOrUpdatePostAsync(
                post, model.GetSelectedTag());

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPostSlug(
            int id, string urlSlug)
        {
            var slugExisted = await _blogRepository
                .IsPostSlugExistedAsync(id, urlSlug);

            return slugExisted
                ? Json($"slug '{urlSlug}' đã được sử dụng")
                : Json(true);
        }


        [HttpPost]
        public async Task<IActionResult> TogglePublished(int postId)
        {
          
            var post= await _blogRepository.GetPostByIdAsync(postId);
            if (post == null)
            {
                return NotFound();
            }
            await _blogRepository.TogglePublishedFlagAsync(postId);

            return RedirectToAction(nameof(Index));
        }


        private async Task PopulatePostFilterModelAsync(PostFilterModel model)
        {
            var authors = await _blogRepository.GetAuthorsAsync();
            var categories = await _blogRepository.GetCategoriesAsync();

            model.AuthorList = authors.Select(a => new SelectListItem()
            {
                Text = a.FullName,
                Value = a.Id.ToString()
            });
            
            model.CategoryList = categories.Select(a => new SelectListItem()
            {
                Text = a.Name,
                Value = a.Id.ToString()
            });
        }

		private async Task PopulatePostEditModeAsync(PostEditModel model)
		{
			var authors = await _blogRepository.GetAuthorsAsync();
			var categories = await _blogRepository.GetCategoriesAsync();

			model.AuthorList = authors.Select(a => new SelectListItem()
			{
				Text = a.FullName,
				Value = a.Id.ToString()
			});

			model.CategoryList = categories.Select(a => new SelectListItem()
			{
				Text = a.Name,
				Value = a.Id.ToString()
			});
		}


        





    }
}
