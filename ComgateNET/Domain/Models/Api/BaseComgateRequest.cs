namespace ComgateNET.Domain.Models.Api
{
    public class BaseComgateRequest
    {
		public string Merchant { get; set; }
		public string Secret { get; set; }
		public string TransactionID { get; set; }
		public bool Test { get; set; }


		#region Someone prefer Fluent API settings

		public virtual BaseComgateRequest SetMerchant(string merchant)
		{
			this.Merchant = merchant;
			return this;
		}

		public virtual BaseComgateRequest SetTransactionID(string transId)
		{
			this.TransactionID = transId;
			return this;
		}

		public virtual BaseComgateRequest SetSecret(string secret)
		{
			this.Secret = secret;
			return this;
		}

		public virtual BaseComgateRequest SetEnviroment(bool isTest)
		{
			this.Test = isTest;
			return this;
		}

		public virtual BaseComgateRequest SetProductionEnviroment()
		{
			this.Test = false;
			return this;
		}

		public virtual BaseComgateRequest SetTestEnviroment()
		{
			this.Test = true;
			return this;
		}

		#endregion
	}
}
