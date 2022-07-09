using System;
using System.Collections.Generic;
using System.Text;

namespace ComgateNET.Domain.Models.Responses
{
	public class CreatePaymentResponse
	{
		public string TransactionId { get; set; }

		public string RedirectUrl { get; set; }
	}
}
