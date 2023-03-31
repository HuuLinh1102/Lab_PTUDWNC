using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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

		public static WebApplication MapAuthorEndpoints(
			this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/authors");

			routeGroupBuilder.MapGet("/", GetAuthors)
				.WithName("GetAuthors")
				.Produces<ApiResponse<PaginationResult<AuthorItem>>>();

			routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
				.WithName("GetAuthorById")
				.Produces< ApiResponse<AuthorItem>>();

			routeGroupBuilder.MapGet(
					"/{slug:regex(^[a-z0-9 -]+$)}/posts",
					GetPostsByAuthorSlug)
				.WithName("GetPostsByAuthorSlug")
				.Produces<ApiResponse<PaginationResult<PostDto>>>();

			routeGroupBuilder.MapPost("/", AddAuthor)
				.AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
				.WithName("AddNewAuthor")
				.Produces(401)
				.Produces<ApiResponse<AuthorItem>>();

			routeGroupBuilder.MapPost("/{id:int}/avatar", SetAuthorPicture)
				.WithName("SetAuthorPicture")
				.Accepts<IFormFile>("multipart/from-data")
				.Produces(401)
				.Produces<ApiResponse<AuthorItem>>();

			routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
				.WithName("UpdateAnAuthor")
				.AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
				.Produces(401)
				.Produces<ApiResponse<AuthorItem>>();

			routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
				.WithName("DeleteAnAuthor")
				.Produces(204)
				.Produces(404);

			routeGroupBuilder.MapGet("/best/{limit:int}", GetTopAuthors)
				.WithName("GetTopAuthors")
				.Produces(401)
				.Produces<ApiResponse<PaginationResult<AuthorItem>>>();

			return app;
		}

		private static async Task<IResult> GetAuthors(
			[AsParameters] AuthorFilterModel model,
			[FromServices] IAuthorRepository authorRepository)
		{
			var authorsList = await authorRepository
				.GetPagedAuthorsAsync(model, model.Name);

			var paginationResult =
				new PaginationResult<AuthorItem>(authorsList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> GetAuthorDetails(
			int id,
			[FromServices] IAuthorRepository authorRepository,
			[FromServices] IMapper mapper)
		{
			var author = await authorRepository.GetCachedAuthorByIdAsync(id);
			
			return author == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, 
				$"Could not find author with Id {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author)));
		}

		private static async Task<IResult> GetPostsByAuthor(
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

			return Results.Ok(ApiResponse.Success(paginationResult));
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

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> AddAuthor(
			[FromServices] AuthorEditModel model,
			[FromServices] IAuthorRepository authorRepository,
			[FromServices] IMapper mapper)
		{
			if (await authorRepository
				.IsAuthorSlugExistedAsync(0, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
				$"Slug '{model.UrlSlug}' already exist"));
				
			}

			var author = mapper.Map<Author>(model);
			await authorRepository.AddOrUpdateAsync(author);

			return Results.Ok(ApiResponse.Success(
				mapper.Map<AuthorItem>(author),
				HttpStatusCode.Created));
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
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.BadRequest, "File could not be saved"));
			}

			await authorRepository.SetImageUrlAsync(id, imageUrl);
			return Results.Ok(ApiResponse.Success(imageUrl));
		}


		private static async Task<IResult> UpdateAuthor(
			[FromRoute] int id,
			[FromServices] AuthorEditModel model,
			[FromServices] IValidator<AuthorEditModel> validator,
			[FromServices] IAuthorRepository authorRepository,
			[FromServices] IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.BadRequest, validationResult));
			}

			if (await authorRepository.IsAuthorSlugExistedAsync(
				id, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.Conflict,
					$"Slug '{model.UrlSlug}' already exist"));
			}
			
			var author = mapper.Map<Author>(model);
			author.Id = id;

			return await authorRepository.AddOrUpdateAsync(author)
				? Results.Ok(ApiResponse.Success("Author is updated",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find author"));
		}

		private static async Task<IResult> DeleteAuthor(
			[FromRoute] int id,
			[FromServices] IAuthorRepository authorRepository)
		{
			return await authorRepository.DeleteAuthorAsync(id)
				? Results.Ok(ApiResponse.Success("Author is deleted",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find author"));
		}

		private static async Task<IResult> GetTopAuthors(
			[FromRoute] int limit,
			[FromServices] IAuthorRepository authorRepository,
			[FromServices] IMapper mapper,
			[AsParameters] PagingModel pagingModel)
		{
			var authorsList = await authorRepository.GetPopularAuthorsAsync(limit, pagingModel);
			if (authorsList == null)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find author"));
			}
			var paginationResult =
				new PaginationResult<AuthorItem>(authorsList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}
	}
}
