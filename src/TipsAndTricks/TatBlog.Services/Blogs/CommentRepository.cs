using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Blogs
{
	public class CommentRepository : ICommentRepository
	{
		private readonly BlogDbContext _context;

		public CommentRepository(BlogDbContext context)
		{
			_context = context;
		}

		public async Task<Comment> AddCommentAsync(
			Comment comment, 
			CancellationToken cancellationToken = default)
		{
			_context.Comments.Add(comment);
			await _context.SaveChangesAsync(cancellationToken);
			return comment;
		}

		public async Task<Comment> GetCommentByIdAsync(
			int commentId, 
			CancellationToken cancellationToken = default)
		{
			var comment = await _context.Comments
				.Include(c => c.Post)
				.Include(c => c.Author)
				.FirstOrDefaultAsync(c => c.Id == commentId, cancellationToken);
			return comment;
		}

		public async Task<IEnumerable<Comment>> GetCommentsForPostAsync(
			int postId, 
			CancellationToken cancellationToken = default)
		{
			var comments = await _context.Comments
				.Include(c => c.Post)
				.Include(c => c.Author)
				.Where(c => c.PostId == postId && c.IsApproved)
				.OrderByDescending(c => c.CreatedDate)
				.ToListAsync(cancellationToken);
			return comments;
		}

		public async Task<IEnumerable<Comment>> GetUnapprovedCommentsAsync(
			CancellationToken cancellationToken = default)
		{
			var unapprovedComments = await _context.Comments
				.Include(c => c.Post)
				.Include(c => c.Author)
				.Where(c => !c.IsApproved)
				.ToListAsync(cancellationToken);
			return unapprovedComments;
		}

		public async Task ApproveCommentAsync(
			Comment comment, 
			CancellationToken cancellationToken = default)
		{
			comment.IsApproved = true;
			await _context.SaveChangesAsync(cancellationToken);
		}

		public async Task DeleteCommentAsync(
			int commentId, 
			CancellationToken cancellationToken = default)
		{
			var comment = await _context.Comments.FindAsync(commentId);
			_context.Comments.Remove(comment);
			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
