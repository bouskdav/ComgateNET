using ComgateNET.Domain.Enums;
using ComgateNET.Domain.Models.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComgateNET.Domain.Models
{
	public class GetPaymentMethodsRequest : BaseComgateRequest
	{
		public DataType DataType { get; set; }

		public Currency Currency { get; set; }

		public Lang Lang { get; set; }
	}
}
