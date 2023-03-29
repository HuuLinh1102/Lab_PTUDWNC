using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
	public interface ICommentRepository
	{
		Task<Comment> AddCommentAsync(
			Comment comment,
			CancellationToken cancellationToken = default);

		Task<Comment> GetCommentByIdAsync(
			int commentId,
			CancellationToken cancellationToken = default);

		Task<IEnumerable<Comment>> GetCommentsForPostAsync(
			int postId,
			CancellationToken cancellationToken = default);

		Task<IEnumerable<Comment>> GetUnapprovedCommentsAsync(
			CancellationToken cancellationToken = default);

		Task ApproveCommentAsync(
			Comment comment,
			CancellationToken cancellationToken = default);

		Task DeleteCommentAsync(
			int commentId,
			CancellationToken cancellationToken = default);
	}
}
