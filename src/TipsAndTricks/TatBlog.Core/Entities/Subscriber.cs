using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities
{
	public class Subscriber
	{
		public Subscriber()
		{
			Comments = new List<Comment>();
		}
		public int Id { get; set; }
		public string Email { get; set; }
		public DateTime SubscriptionDate { get; set; }
		public DateTime? UnsubscribeDate { get; set; }
		public string UnsubscribeReason { get; set; }
		public bool IsUserInitiatedUnsubscribe { get; set; }
		public string AdminNote { get; set; }
		public IList<Comment> Comments { get; set; }
	}
}
