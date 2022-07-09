using ComgateNET.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ComgateNET.WebTest.Models
{
	public class RefundPaymentViewModel
	{
		public string TransactionId { get; set; }
		public decimal Amount { get; set; }
		public Currency Currency { get; set; }
		public List<SelectListItem> Currencies { get; set; } = new();
	}
}
