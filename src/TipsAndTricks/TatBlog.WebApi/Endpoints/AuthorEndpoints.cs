using AutoMapper;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using NLog.Web;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
	public static class AuthorEndpoints
	{
		private static readonly ILogger _logger = (ILogger)NLogBuilder
	.ConfigureNLog("nlog.config")
	.GetCurrentClassLogger();
		public static WebApplication MapAuthorEndpoints(
			this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/authors");

			routeGroupBuilder.MapGet("/", GetAuthors)
				.WithName("GetAuthors")
				.Produces<PaginationResult<AuthorItem>>();

			routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
				.WithName("GetAuthorById")
				.Produces<AuthorItem>()
				.Produces(404);

			routeGroupBuilder.MapGet(
					"/{slug:regex(^[a-z0-9 -]+$)}/posts",
					GetPostsByAuthorSlug)
				.WithName("GetPostsByAuthorSlug")
				.Produces<PaginationResult<PostDto>>();

			routeGroupBuilder.MapPost("/", AddAuthor)
				.WithName("AddNewAuthor")
				.AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
				.Produces(201)
				.Produces(400)
				.Produces(409);

			routeGroupBuilder.MapPost("/{id:int}/avatar", SetAuthorPicture)
				.WithName("SetAuthorPicture")
				.Accepts<IFormFile>("multipart/from-data")
				.Produces<string>()
				.Produces(400);

			routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
				.WithName("UpdateAnAuthor")
				.AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
				.Produces(204)
				.Produces(400)
				.Produces(409);

			routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
				.WithName("DeleteAnAuthor")
				.Produces(204)
				.Produces(404);

			routeGroupBuilder.MapGet("/best/{limit:int}", GetTopAuthors)
				.WithName("GetTopAuthors")
				.Produces<IEnumerable<AuthorDto>>()
				.Produces(404);

			return app;
		}

		private static async Task<IResult> GetAuthors(
			[AsParameters] AuthorFilterModel model,
			[FromServices] IAuthorRepository authorRepository)
		{
			_logger.LogInformation("Lấy danh sách tác giả từ CSDL");
			var authorsList = await authorRepository
				.GetPagedAuthorsAsync(model, model.Name);

			_logger.LogInformation("Trả về kết quả phân trang");
			var paginationResult =
				new PaginationResult<AuthorItem>(authorsList);

			_logger.LogInformation("Thông báo yêu cầu đc xử lý thành công và kèm theo kết quả");
			return Results.Ok(paginationResult);
		}

		private static async Task<IResult> GetAuthorDetails(
			int id,
			[FromServices] IAuthorRepository authorRepository,
			[FromServices] IMapper mapper)
		{
			var author = await authorRepository.GetCachedAuthorByIdAsync(id);
			return author == null
				? Results.NotFound($"Không tìm thấy tác giả có mã số {id}")
				: Results.Ok(mapper.Map<AuthorItem>(author));
		}

		private static async Task<IResult> GetPostsByAuthorId(
			int id,
			[AsParameters] PagingModel pagingModel,
			[FromServices] IBlogRepository blogRepository)
		{
			var postQuery = new PostQuery()
			{
				AuthorId = id,
				PublishedOnly = true
			};

			var postsList = await blogRepository.GetPagedPostsAsync(
				postQuery, pagingModel,
				posts => posts.ProjectToType<PostDto>());

			var paginationResult = new PaginationResult<PostDto>(postsList);

			return Results.Ok(paginationResult);
		}


		private static async Task<IResult> GetPostsByAuthorSlug(
			[FromRoute] string slug,
			[AsParameters] PagingModel pagingModel,
			[FromServices] IBlogRepository blogRepository)
		{
			var postQuery = new PostQuery()
			{
				AuthorSlug = slug,
				PublishedOnly = true
			};

			var postsList = await blogRepository.GetPagedPostsAsync(
				postQuery, pagingModel,
				posts => posts.ProjectToType<PostDto>());

			var paginationResult = new PaginationResult<PostDto>(postsList);

			return Results.Ok(paginationResult);
		}

		private static async Task<IResult> AddAuthor(
			[FromServices] AuthorEditModel model,
			[FromServices] IAuthorRepository authorRepository,
			[FromServices] IMapper mapper)
		{
			if (await authorRepository
				.IsAuthorSlugExistedAsync(0, model.UrlSlug))
			{
				return Results.Conflict(
					$"Slug '{model.UrlSlug}' đã được sử dụng");
			}

			var author = mapper.Map<Author>(model);
			await authorRepository.AddOrUpdateAsync(author);

			return Results.CreatedAtRoute(
				"GetAuthorById", new { author.Id },
				mapper.Map<AuthorItem>(author));
		}


		private static async Task<IResult> SetAuthorPicture(
			[FromRoute] int id, IFormFile imageFile,
			[FromServices] IAuthorRepository authorRepository,
			[FromServices] IMediaManager mediaManager)
		{
			var imageUrl = await mediaManager.SaveFileAsync(
				imageFile.OpenReadStream(),
				imageFile.FileName, imageFile.ContentType);

			if (string.IsNullOrWhiteSpace(imageUrl))
			{
				return Results.BadRequest("Không lưu được tập tin");
			}

			await authorRepository.SetImageUrlAsync(id, imageUrl);
			return Results.Ok(imageUrl);
		}


		private static async Task<IResult> UpdateAuthor(
			[FromRoute] int id,
			[FromServices] AuthorEditModel model,
			[FromServices] IAuthorRepository authorRepository,
			[FromServices] IMapper mapper)
		{

			if (await authorRepository
				.IsAuthorSlugExistedAsync(id, model.UrlSlug))
			{
				return Results.Conflict(
					$"Slug '{model.UrlSlug}' đã được sử dụng");
			}

			var author = mapper.Map<Author>(model);
			author.Id = id;

			return await authorRepository.AddOrUpdateAsync(author)
				? Results.NoContent()
				: Results.NotFound();
		}

		private static async Task<IResult> DeleteAuthor(
			[FromRoute] int id,
			[FromServices] IAuthorRepository authorRepository)
		{
			return await authorRepository.DeleteAuthorAsync(id)
				? Results.NoContent()
				: Results.NotFound($"Could not find author with id = {id}");
		}

		private static async Task<IResult> GetTopAuthors(
			int limit,
			[FromServices] IAuthorRepository authorRepository,
			[FromServices] IMapper mapper)
		{
			var authors = await authorRepository.GetPopularAuthorsAsync(limit);
			if (authors == null)
			{
				return Results.NotFound("No authors found.");
			}

			var authorsList = mapper.Map<IEnumerable<AuthorDto>>(authors);
			return Results.Ok(authorsList);
		}
	}
}
