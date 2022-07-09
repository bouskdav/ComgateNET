using ComgateNET.Domain.Enums;
using ComgateNET.Domain.Models.Api;
using ComgateNET.Domain.Models.Payment;
using ComgateNET.Domain.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComgateNET.Abstraction.Api
{
	public interface IComgateApi
	{
		Task<ApiResponse<CreatePaymentResponse>> CreatePayment(BaseComgatePayment payment, Payer payer);
		Task<ApiResponse<GetPaymentMethodsResponse>> GetAvailebleMethods();
		Task<ApiResponse<GetPaymentStatusResponse>> GetPaymentStatus(string transId);
		Task<ApiResponse<bool>> RefundPayment(string transId, decimal price, Currency currency, bool test);
		Task<ApiResponse<RecurrentPaymentResponse>> RecurrentPayment(BaseComgatePayment payment, Payer payer);
		Task<ApiResponse<bool>> CapturePreauth(string transId);
		Task<ApiResponse<bool>> CancelPreauth(string transId);
	}
}
