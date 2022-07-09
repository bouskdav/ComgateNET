using ComgateNET.Domain.Enums;
using ComgateNET.Domain.Models.Api;

namespace ComgateNET.Domain.Models
{
    public class RefundRequest : BaseComgateRequest
	{
		public Currency Currency { get; set; }

		public decimal Amount { get; set; }

		public bool Test { get; set; }


		#region Someone prefer Fluent API settings

		public RefundRequest SetAmount(decimal amount)
		{
			this.Amount = amount;
			return this;
		}

		public RefundRequest SetCurrency(Enums.Currency currency)
		{
			this.Currency = currency;
			return this;
		}

		public RefundRequest SetTest(bool test)
		{
			this.Test = test;
			return this;
		}

		#endregion

	}
}
