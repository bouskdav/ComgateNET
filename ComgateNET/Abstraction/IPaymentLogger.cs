using ComgateNET.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComgateNET.Abstraction
{
	public interface IPaymentLogger
	{
		void LogPayment(CreatePaymentRequest request);
	}
}
