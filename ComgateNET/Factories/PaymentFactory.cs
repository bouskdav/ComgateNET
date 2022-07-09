using ComgateNET.Domain.Enums;
using ComgateNET.Domain.Models.Payment;

namespace ComgateNET.Factories
{
    public class PaymentFactory
	{
		public static BaseComgatePayment GetBasePayment(int price, string referenceId, string label, PaymentMethod method = PaymentMethod.ALL, EetData eet = null, bool prepareOnly = true)
		{
			return new BaseComgatePayment()
			{
				Country = Country.CZ,
				Currency = Currency.CZK,
				PaymentCategory = PaymentCategory.PHYSICAL,
				Price = price,
				ReferenceId = referenceId,
				Method = method,
				Label = label,
				EET = eet,
				PrepareOnly = prepareOnly
			};
		}
	}
}
