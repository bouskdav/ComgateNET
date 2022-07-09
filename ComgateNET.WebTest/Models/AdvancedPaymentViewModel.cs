using ComgateNET.Domain.Enums;
using ComgateNET.Domain.Models.Responses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComgateNET.WebTest.Models
{
	public class AdvancedPaymentViewModel
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

		public bool PreAuth { get; set; }

		public List<SelectListItem> PaymentMethods { get; set; }
		public PaymentMethod SelectedMethod { get; set; }

		public List<SelectListItem> Currencies { get; set; }
		public Currency SelectedCurrency { get; set; }

		public bool EetReport { get; set; }
		public Method[] Methods { get; internal set; }

		public string EETData { get; set; }
	}
}
