using ComgateNET.Abstraction;
using ComgateNET.Domain.Enums;
using ComgateNET.Domain.Models;
using ComgateNET.Domain.Models.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComgateNET.Services
{
	public class ComgateRequestBuilder : IComgateRequestBuilder
	{
		public CreatePaymentRequest CreatePaymentRequest(BaseComgatePayment payment, Payer payer, Lang lang = Lang.cs)
		{
            CreatePaymentRequest request = new CreatePaymentRequest
            {
                Payment = payment,
                Payer = payer
            };

            request.Payment.Embedded = payment.Embedded;
			request.Payment.InitRecurring = payment.InitRecurring;
			request.Payment.Preauth = payment.Preauth;
			request.Payment.Verification = payment.Verification;
			request.Lang = lang;
			request.Payment.PrepareOnly = payment.PrepareOnly;

			return request;
		}


		public GetPaymentMethodsRequest CreateGetPaymentMethodsRequest()
		{
            GetPaymentMethodsRequest request = new GetPaymentMethodsRequest
            {
                DataType = DataType.JSON,
                Currency = Currency.CZK,
                Lang = Lang.cs
            };

            return request;
		}

	}
}
