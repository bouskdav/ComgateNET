using ComgateNET.Abstraction;
using ComgateNET.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComgateNET.Services
{
	public class NullPaymentLogger : IPaymentLogger
	{
		public void LogPayment(CreatePaymentRequest request)
		{

		}
	}
}
