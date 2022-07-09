using System;
using System.Collections.Generic;
using System.Text;

namespace ComgateNET.Domain.Models.Responses
{
    public class GetPaymentMethodsResponse
	{
		public Method[] methods { get; set; }
	}

	public class Method
	{
		public string id { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public string logo { get; set; }
	}
}
