using ComgateNET.Domain.Enums;
using ComgateNET.Domain.Models.Api;
using ComgateNET.Domain.Models.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComgateNET.Domain.Models
{
	public class CreatePaymentRequest : BaseComgateRequest
	{
		public Lang Lang { get; set; }


		public BaseComgatePayment Payment { get; set; }

		public Payer Payer { get; set; }

		#region Someone prefer Fluent API settings

		public CreatePaymentRequest SetPreauth(bool? preauth = null)
		{
			this.Payment.Preauth = preauth;
			return this;
		}

		public CreatePaymentRequest SetEmbedded(bool? embedded = null)
		{
			this.Payment.Embedded = embedded;
			return this;
		}

		public CreatePaymentRequest SetVerification(bool? verification = null)
		{
			this.Payment.Verification = verification;
			return this;
		}


		public CreatePaymentRequest SetInitRecurring(bool? initRecurring = null)
		{
			this.Payment.InitRecurring = initRecurring;
			return this;
		}

		#endregion
	}
}
