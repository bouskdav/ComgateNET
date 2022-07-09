using ComgateNET.Domain.Enums;
using ComgateNET.Domain.Models.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComgateNET.Domain.Models.Responses
{
	public class GetPaymentStatusResponse
	{
		public string merchant { get; set; }
		public string test { get; set; }
		public string price { get; set; }

		public Currency Currency { get; set; }
		public PaymentState Status { get; set; }
		public EetData EET { get; set; }


		public string label { get; set; }
		public string refId { get; set; }
		public string method { get; set; }
		public string email { get; set; }
		public string name { get; set; }
		public string transId { get; set; }
		public string secret { get; set; }
		public string fee { get; set; }
		public string vs { get; set; }
		public string payerId { get; set; }
		public string payerName { get; set; }
		public string payerAcc { get; set; }
		public string account { get; set; }
		public string phone { get; set; }
		public string eetData { get; set; }

	}
}
