using ComgateNET.Abstraction;
using ComgateNET.Abstraction.Api;
using ComgateNET.Domain.Enums;
using ComgateNET.Domain.Models;
using ComgateNET.Domain.Models.Api;
using ComgateNET.Domain.Models.Payment;
using ComgateNET.Domain.Models.Responses;
using ComgateNET.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComgateNET.Services
{
	public class ComgateApiConnector : IComgateApi
	{
		#region fields 

		private readonly IComgateContentSerializer _serializer;
		private readonly IPaymentLogger _paymentLogger;
		private readonly IComgateRequestBuilder _requestBuilder;

		public string BaseUrl { get; set; } = "https://payments.comgate.cz/v1.0/";
		public string Merchant { get; set; }
		public bool IsTestEnviroment { get; set; } = false;
		public Lang Lang { get; set; }
		public bool PrepareOnly { get; set; }
		public string Secret { get; set; }
		public bool? Preauth { get; set; }
		public bool? InitRecurring { get; set; }
		public bool? Verification { get; set; }
		public bool? Embedded { get; set; }

		#endregion

		#region Ctors 

		public ComgateApiConnector()
		{
			_serializer = new ComgateContentSerializer();
			_paymentLogger = new NullPaymentLogger();
			_requestBuilder = new ComgateRequestBuilder();
		} 

		public ComgateApiConnector(string baseUrl)
		{
			_serializer = new ComgateContentSerializer();
			_paymentLogger = new NullPaymentLogger();
			_requestBuilder = new ComgateRequestBuilder();

			if (!String.IsNullOrEmpty(baseUrl))
				BaseUrl = baseUrl;
		}

		public ComgateApiConnector(IComgateContentSerializer serializer, IPaymentLogger paymentLogger, IComgateRequestBuilder requestBuilder)
		{
			_serializer = serializer;
			_paymentLogger = paymentLogger;
			_requestBuilder = requestBuilder;
		}

		#endregion

		#region Fluent API

		public static ComgateApiConnector CreateConnector(string baseUrl)
		{
			return new ComgateApiConnector(baseUrl);
		}

		public ComgateApiConnector SetBaseUrl(string baseUrl)
		{
			this.BaseUrl = baseUrl;
			return this;
		}

		public ComgateApiConnector SetMerchant(string merchant)
		{
			this.Merchant = merchant;
			return this;
		}

		public ComgateApiConnector TestEnviroment(bool IsTest = true)
		{
			this.IsTestEnviroment = IsTest;
			return this;
		}

		public ComgateApiConnector SetLang(Lang lang = Lang.cs)
		{
			this.Lang = lang;
			return this;
		}

		public ComgateApiConnector SetPrepareOnly(bool prepareOnly = true)
		{
			this.PrepareOnly = prepareOnly;
			return this;
		}

		public ComgateApiConnector SetSecret(string secret) { this.Secret = secret; return this; }
		public ComgateApiConnector SetPreauth(bool preauth) { this.Preauth = preauth; return this; }
		public ComgateApiConnector SetInitRecurring(bool initRecurring) { this.InitRecurring = initRecurring; return this; }
		public ComgateApiConnector SetVerification(bool verification) { this.Verification = verification; return this; }
		public ComgateApiConnector SetEmbedded(bool embedded) { this.Embedded = embedded; return this; }

		#endregion

		#region API methods

		public async Task<ApiResponse<CreatePaymentResponse>> CreatePayment(BaseComgatePayment payment, Payer payer)
		{
			CreatePaymentRequest paymentRequest = (CreatePaymentRequest)_requestBuilder
				.CreatePaymentRequest(payment, payer)
				.SetMerchant(this.Merchant)
				.SetEnviroment(this.IsTestEnviroment)
				.SetSecret(this.Secret);

            using var httpClient = HttpClientFactory.CreateHttpClient(ComgateHttpClientType.HttpClient);

            _paymentLogger.LogPayment(paymentRequest);
            var content = _serializer.Serialize<CreatePaymentRequest>(paymentRequest);

            httpClient.BaseAddress = new Uri(BaseUrl);

            var response = await httpClient.PostAsync("create", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return _serializer.Deserialize<CreatePaymentResponse>(responseContent);
            }
            else
            {
                throw new Exception("Cannot create payment");
            }
        }

		public async Task<ApiResponse<GetPaymentMethodsResponse>> GetAvailebleMethods()
		{
			using (var httpClient = HttpClientFactory.CreateHttpClient(ComgateHttpClientType.HttpClient))
			{

				GetPaymentMethodsRequest methodsRequest = (GetPaymentMethodsRequest)_requestBuilder
					.CreateGetPaymentMethodsRequest()
					.SetMerchant(this.Merchant)
					.SetSecret(this.Secret);

				var content = _serializer.Serialize<GetPaymentMethodsRequest>(methodsRequest);

				httpClient.BaseAddress = new Uri(BaseUrl);

				var response = await httpClient.PostAsync("methods", content);

				if (response.IsSuccessStatusCode)
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					return _serializer.Deserialize<GetPaymentMethodsResponse>(responseContent);
				}
				else
				{
					throw new Exception("Cannot create method list");
				}
			}
		}

		public async Task<ApiResponse<GetPaymentStatusResponse>> GetPaymentStatus(string transId)
		{
			throw new NotImplementedException();
			//using (var httpClient = HttpClientFactory.CreateHttpClient(ComgateHttpClientType.HttpClient))
			//{

			//	GetPaymentStatusRequest statusRequest = new GetPaymentStatusRequest()
			//		.SetMerchant(this.Merchant)
			//		.SetSecret(this.Secret)
			//		.SetTransactionId(transId);


			//	var content = _serializer.Serialize<GetPaymentStatusRequest>(statusRequest);

			//	httpClient.BaseAddress = new Uri(BaseUrl);

			//	var response = await httpClient.PostAsync("status", content);

			//	if (response.IsSuccessStatusCode)
			//	{
			//		var responseContent = await response.Content.ReadAsStringAsync();
			//		return _serializer.Deserialize<GetPaymentStatusResponse>(responseContent);
			//	}
			//	else
			//	{
			//		throw new Exception("Cannot create method list");
			//	}
			//}
		}

		public async Task<ApiResponse<bool>> RefundPayment(string transId, decimal price, Currency currency, bool test)
		{
			throw new NotImplementedException();
			//using (var httpClient = HttpClientFactory.CreateHttpClient(ComgateHttpClientType.HttpClient))
			//{
			//	RefundRequest refundRequest = new RefundRequest()
			//		.SetMerchant(this.Merchant)
			//		.SetSecret(this.Secret)
			//		.SetCurrency(currency)
			//		.SetTransactionID(transId)
			//		.SetAmount(price)
			//		.SetTest(test);

			//	var content = _serializer.Serialize<RefundRequest>(refundRequest);

			//	httpClient.BaseAddress = new Uri(BaseUrl);

			//	var response = await httpClient.PostAsync("refund", content);

			//	if (response.IsSuccessStatusCode)
			//	{
			//		var responseContent = await response.Content.ReadAsStringAsync();
			//		return _serializer.Deserialize<bool>(responseContent);
			//	}
			//	else
			//	{
			//		throw new Exception("Cannot create method list");
			//	}
			//}
		}

		public async Task<ApiResponse<RecurrentPaymentResponse>> RecurrentPayment(BaseComgatePayment payment, Payer payer)
		{
			throw new NotImplementedException();
		}

		public async Task<ApiResponse<bool>> CapturePreauth(string transId)
		{
			using (var httpClient = HttpClientFactory.CreateHttpClient(ComgateHttpClientType.HttpClient))
			{
				CapturePreAuthRequest capturePreauthRequest = (CapturePreAuthRequest)new CapturePreAuthRequest()
					.SetMerchant(this.Merchant)
					.SetSecret(this.Secret)
					.SetTransactionID(transId);

				var content = _serializer.Serialize<CapturePreAuthRequest>(capturePreauthRequest);

				httpClient.BaseAddress = new Uri(BaseUrl);

				var response = await httpClient.PostAsync("capturePreauth", content);

				if (response.IsSuccessStatusCode)
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					return _serializer.Deserialize<bool>(responseContent);
				}
				else
				{
					throw new Exception("Cannot create method list");
				}
			}
		}

		public async Task<ApiResponse<bool>> CancelPreauth(string transId)
		{

			using (var httpClient = HttpClientFactory.CreateHttpClient(ComgateHttpClientType.HttpClient))
			{

				CancelPreAuthRequest cancelPreauthRequest = (CancelPreAuthRequest)new CancelPreAuthRequest()
					.SetMerchant(this.Merchant)
					.SetSecret(this.Secret)
					.SetTransactionID(transId);

				var content = _serializer.Serialize<CancelPreAuthRequest>(cancelPreauthRequest);

				httpClient.BaseAddress = new Uri(BaseUrl);

				var response = await httpClient.PostAsync("cancelPreauth", content);

				if (response.IsSuccessStatusCode)
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					return _serializer.Deserialize<bool>(responseContent);
				}
				else
				{
					throw new Exception("Cannot create method list");
				}
			}
		}
		#endregion
	}
}
