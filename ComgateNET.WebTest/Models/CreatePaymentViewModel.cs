using ComgateNET.Domain.Enums;

namespace ComgateNET.WebTest.Models
{
	public class CreatePaymentViewModel
	{
		public string TransId { get; set; }
		public string RefId { get; set; }
		public string Amount { get; set; }
		public PaymentState State { get; set; }
	}
}
