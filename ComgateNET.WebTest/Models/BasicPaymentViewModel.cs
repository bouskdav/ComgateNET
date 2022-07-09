using System.ComponentModel.DataAnnotations;

namespace ComgateNET.WebTest.Models
{
	public class BasicPaymentViewModel
	{
		[Required]
		public decimal Price { get; set; }
		[Required]
		public string ReferenceId { get; set; }
		[Required]
		public string Label { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Name { get; set; }
	}
}
