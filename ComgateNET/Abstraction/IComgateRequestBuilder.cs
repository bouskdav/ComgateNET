using ComgateNET.Domain.Enums;
using ComgateNET.Domain.Models;
using ComgateNET.Domain.Models.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComgateNET.Abstraction
{
	public interface IComgateRequestBuilder
	{
		/// <summary>
		/// Create CreatePaymentRequest
		/// </summary>
		/// <param name="payment"></param>
		/// <param name="payer"></param>
		/// <param name="urlEndpoint"></param>
		/// <param name="lang"></param>
		/// <param name="embedded"></param>
		/// <param name="initRecurring"></param>
		/// <param name="preauth"></param>
		/// <param name="verification"></param>
		/// <param name="prepareOnly"></param>
		/// <returns></returns>
		CreatePaymentRequest CreatePaymentRequest(BaseComgatePayment payment, Payer payer, Lang lang = Lang.cs);

		/// <summary>
		/// Create request for supported payments methods
		/// </summary>
		/// <returns></returns>
		GetPaymentMethodsRequest CreateGetPaymentMethodsRequest();

	}
}
