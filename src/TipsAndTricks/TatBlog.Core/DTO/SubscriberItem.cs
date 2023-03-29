using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
	public class SubscriberItem
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public DateTime SubscriptionDate { get; set; }
		public DateTime? UnsubscribeDate { get; set; }
		public string UnsubscribeReason { get; set; }
		public bool IsUserInitiatedUnsubscribe { get; set; }
		public string AdminNote { get; set; }
	}
}
