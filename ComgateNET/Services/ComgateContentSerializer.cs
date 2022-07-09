using ComgateNET.Abstraction;
using ComgateNET.Domain.Enums;
using ComgateNET.Domain.Models;
using ComgateNET.Domain.Models.Api;
using ComgateNET.Domain.Models.Payment;
using ComgateNET.Domain.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace ComgateNET.Services
{
	public class ComgateContentSerializer : IComgateContentSerializer
	{
		public FormUrlEncodedContent Serialize<T>(T request)
		{
			if (request == null)
			{
				throw new ArgumentException("invalid ComgateRequest");
			}

			Type requestType = typeof(T);

			List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

			if (requestType == typeof(CreatePaymentRequest))
			{
				parameters.AddRange(CreatePaymentMandatoryParams((CreatePaymentRequest)(object)request));
				parameters.AddRange(CreatePaymentOptionalParams((CreatePaymentRequest)(object)request));
			}

			if (requestType == typeof(GetPaymentStatusRequest))
			{
				parameters.AddRange(CreateStatusMandatoryParams((GetPaymentStatusRequest)(object)request));
			}

			if (requestType == typeof(CapturePreAuthRequest))
			{
				parameters.AddRange(CreatePreauthMandatoryParams((CapturePreAuthRequest)(object)request));
			}

			if (requestType == typeof(CancelPreAuthRequest))
			{
				parameters.AddRange(CreateCancelPreauthMandatoryParams((CancelPreAuthRequest)(object)request));

			}

			if (requestType == typeof(GetPaymentMethodsRequest))
			{
				parameters.AddRange(CreateMethodsMandatoryParams((GetPaymentMethodsRequest)(object)request));
			}

			if (requestType == typeof(RefundRequest))
			{
				parameters.AddRange(CreateRefundMandatoryParams((RefundRequest)(object)request));
			}

			return new FormUrlEncodedContent(parameters);
		}

		private IEnumerable<KeyValuePair<string, string>> CreateCancelPreauthMandatoryParams(CancelPreAuthRequest request)
		{
			List<KeyValuePair<string, string>> mandatoryParameters = new List<KeyValuePair<string, string>>();
			mandatoryParameters.Add(new KeyValuePair<string, string>("merchant", request.Merchant));
			mandatoryParameters.Add(new KeyValuePair<string, string>("secret", request.Secret));
			mandatoryParameters.Add(new KeyValuePair<string, string>("transId", request.TransactionID));

			return mandatoryParameters;
		}

		private IEnumerable<KeyValuePair<string, string>> CreatePreauthMandatoryParams(CapturePreAuthRequest request)
		{
			List<KeyValuePair<string, string>> mandatoryParameters = new List<KeyValuePair<string, string>>();
			mandatoryParameters.Add(new KeyValuePair<string, string>("merchant", request.Merchant));
			mandatoryParameters.Add(new KeyValuePair<string, string>("secret", request.Secret));
			mandatoryParameters.Add(new KeyValuePair<string, string>("transId", request.TransactionID));

			return mandatoryParameters;
		}

		private IEnumerable<KeyValuePair<string, string>> CreateRefundMandatoryParams(RefundRequest request)
		{
			List<KeyValuePair<string, string>> mandatoryParameters = new List<KeyValuePair<string, string>>();
			mandatoryParameters.Add(new KeyValuePair<string, string>("merchant", request.Merchant));
			mandatoryParameters.Add(new KeyValuePair<string, string>("secret", request.Secret));
			mandatoryParameters.Add(new KeyValuePair<string, string>("transId", request.TransactionID));
			mandatoryParameters.Add(new KeyValuePair<string, string>("amount", request.Amount.ToString()));
			mandatoryParameters.Add(new KeyValuePair<string, string>("test", request.Test.ToString().ToLower()));


			return mandatoryParameters;
		}

		private IEnumerable<KeyValuePair<string, string>> CreateStatusMandatoryParams(GetPaymentStatusRequest request)
		{
			List<KeyValuePair<string, string>> mandatoryParameters = new List<KeyValuePair<string, string>>();
			mandatoryParameters.Add(new KeyValuePair<string, string>("merchant", request.Merchant));
			mandatoryParameters.Add(new KeyValuePair<string, string>("secret", request.Secret));
			mandatoryParameters.Add(new KeyValuePair<string, string>("transId", request.TransactionID));

			return mandatoryParameters;
		}

		private IEnumerable<KeyValuePair<string, string>> CreateMethodsMandatoryParams(GetPaymentMethodsRequest request)
		{
			List<KeyValuePair<string, string>> mandatoryParameters = new List<KeyValuePair<string, string>>();
			mandatoryParameters.Add(new KeyValuePair<string, string>("merchant", request.Merchant));
			mandatoryParameters.Add(new KeyValuePair<string, string>("secret", request.Secret));
			mandatoryParameters.Add(new KeyValuePair<string, string>("lang", request.Lang.ToString().ToLower()));
			mandatoryParameters.Add(new KeyValuePair<string, string>("type", request.DataType.ToString().ToLower()));


			return mandatoryParameters;
		}

		private List<KeyValuePair<string, string>> CreatePaymentOptionalParams(CreatePaymentRequest request)
		{
			List<KeyValuePair<string, string>> optionalParameters = new List<KeyValuePair<string, string>>();

			//optionalParameters.Add(new KeyValuePair<string, string>("lang", request.Lang.ToString().ToLower()));

			if (!string.IsNullOrEmpty(request.Payment.Account))
				optionalParameters.Add(new KeyValuePair<string, string>("account", request.Payment.Account));
			// zaslani EET
			if (request.Payment.EetReport)
			{
				optionalParameters.Add(new KeyValuePair<string, string>("eetReport", request.Payment.EetReport.ToString().ToLower()));
				optionalParameters.Add(new KeyValuePair<string, string>("eetData", JsonConvert.SerializeObject(request.Payment.EET)));
			}
			if (request.Payment.Embedded != null)
				optionalParameters.Add(new KeyValuePair<string, string>("embedded", request.Payment.Embedded.ToString().ToLower()));
			if (request.Payment.InitRecurring != null)
				optionalParameters.Add(new KeyValuePair<string, string>("initRecurring", request.Payment.InitRecurring.ToString().ToLower()));


			if (request.Payer != null)
			{
				optionalParameters.Add(new KeyValuePair<string, string>("payerId", request.Payer.PayerId));

				if (request.Payer.Contact != null)
				{
					if (!string.IsNullOrEmpty(request.Payer.Contact.Email))
						optionalParameters.Add(new KeyValuePair<string, string>("email", request.Payer.Contact.Email));
					if (!string.IsNullOrEmpty(request.Payer.Contact.Name))
						optionalParameters.Add(new KeyValuePair<string, string>("name", request.Payer.Contact.Name));
					if (!string.IsNullOrEmpty(request.Payer.Contact.Phone))
						optionalParameters.Add(new KeyValuePair<string, string>("phone", request.Payer.Contact.Phone));

				}
			}

			if (request.Payment.Preauth != null)
				optionalParameters.Add(new KeyValuePair<string, string>("preauth", request.Payment.Preauth.ToString().ToLower()));

			if (request.Payment.Verification != null)
				optionalParameters.Add(new KeyValuePair<string, string>("verification", request.Payment.Verification.ToString().ToLower()));


			return optionalParameters;
		}

		private List<KeyValuePair<string, string>> CreatePaymentMandatoryParams(CreatePaymentRequest request)
		{
			List<KeyValuePair<string, string>> mandatoryParameters = new List<KeyValuePair<string, string>>();
			mandatoryParameters.Add(new KeyValuePair<string, string>("merchant", request.Merchant));
			mandatoryParameters.Add(new KeyValuePair<string, string>("price", request.Payment.Price.ToString()));
			mandatoryParameters.Add(new KeyValuePair<string, string>("curr", request.Payment.Currency.ToString()));
			mandatoryParameters.Add(new KeyValuePair<string, string>("label", WebUtility.HtmlDecode(request.Payment.Label)));
			mandatoryParameters.Add(new KeyValuePair<string, string>("refId", request.Payment.ReferenceId));
			mandatoryParameters.Add(new KeyValuePair<string, string>("cat", request.Payment.PaymentCategory.ToString()));
			mandatoryParameters.Add(new KeyValuePair<string, string>("method", request.Payment.Method.ToString()));
			mandatoryParameters.Add(new KeyValuePair<string, string>("prepareOnly", request.Payment.PrepareOnly.ToString().ToLower()));
			mandatoryParameters.Add(new KeyValuePair<string, string>("secret", request.Secret));
			mandatoryParameters.Add(new KeyValuePair<string, string>("test", request.Test.ToString().ToLower()));
			return mandatoryParameters;
		}

		public ApiResponse<T> Deserialize<T>(string responseContent)
		{
			try
			{
				Type responseType = typeof(T);
				ApiResponse<T> response = null;

				// check if response contains error 
				if (!responseContent.Contains("code=0") && responseType != typeof(GetPaymentMethodsResponse))
				{
					var parsedResponse = HttpUtility.ParseQueryString(responseContent);

					return new ApiResponse<T>()
					{
						Code = (ErrorCode)Enum.Parse(typeof(ErrorCode), parsedResponse["code"], true),
						Message = parsedResponse["message"],
						Success = false,
						Response = default(T)
					};
				}

				if (responseType == typeof(GetPaymentMethodsResponse))
				{
					response = (ApiResponse<T>)ParseMethods(responseContent);
				}

				if (responseType == typeof(CreatePaymentResponse))
				{
					response = (ApiResponse<T>)ParsePayment(responseContent);
				}

				if (responseType == typeof(GetPaymentStatusResponse))
				{
					response = (ApiResponse<T>)ParseStatus(responseContent);
				}

				if (responseType == typeof(bool))
				{
					response = (ApiResponse<T>)Parse(responseContent);
				}

				return response;

			}
			catch (Exception ex)
			{
				throw new Exception("Cannot parse payment response " + responseContent);

			}
		}

		private object Parse(string responseContent)
		{
			object response;
			var parsedResponse = HttpUtility.ParseQueryString(responseContent);

			response = new ApiResponse<bool>()
			{
				Code = (ErrorCode)Enum.Parse(typeof(ErrorCode), parsedResponse["code"], true),
				Message = parsedResponse["message"],
				Response = true,
				Success = true
			};
			return response;

		}

		private object ParseStatus(string responseContent)
		{
			object response;
			var parsedResponse = HttpUtility.ParseQueryString(responseContent);

			response = new ApiResponse<GetPaymentStatusResponse>()
			{
				Code = (ErrorCode)Enum.Parse(typeof(ErrorCode), parsedResponse["code"], true),
				Message = parsedResponse["message"],
				Response = new GetPaymentStatusResponse()
				{
					price = parsedResponse["price"],
					email = parsedResponse["email"],
					Status = (PaymentState)Enum.Parse(typeof(PaymentState), parsedResponse["status"], true),
					Currency = (Currency)Enum.Parse(typeof(Currency), parsedResponse["curr"], true),
					fee = parsedResponse["fee"],
					name = parsedResponse["name"],
					refId = parsedResponse["refId"],
					transId = parsedResponse["transId"],
					payerName = parsedResponse["payerName"],
					vs = parsedResponse["vs"],
					method = parsedResponse["method"],
					account = parsedResponse["account"],
					eetData = parsedResponse["eetData"],
					label = parsedResponse["label"],
					payerAcc = parsedResponse["payerAcc"],
					payerId = parsedResponse["payerId"],
					test = parsedResponse["test"],
					phone = parsedResponse["phone"],
					merchant = parsedResponse["merchant"],
					secret = parsedResponse["secret"]
				}
				,
				Success = true
			};

			// parse EET data 
			if (!string.IsNullOrEmpty(parsedResponse["eetData"]))
			{
				try
				{
					((ApiResponse<GetPaymentStatusResponse>)response).Response.EET = JsonConvert.DeserializeObject<EetData>(parsedResponse["eetData"]);
				}
				catch (Exception ex)
				{

				}
			}


			return response;
		}

		private object ParseMethods(string responseContent)
		{
			ApiResponse<GetPaymentMethodsResponse> response = new ApiResponse<GetPaymentMethodsResponse>();
			// should be JSON object 
			if (responseContent.Contains("error"))
			{
				var error = JsonConvert.DeserializeObject<ComgateError>(responseContent);
				response.Code = (ErrorCode)error.error.code;
				response.Message = error.error.message + Environment.NewLine + error.error.extraMessage;
				response.Success = false;
			}
			else
			{
				var data = JsonConvert.DeserializeObject<GetPaymentMethodsResponse>(responseContent);
				response.Response = data;
				response.Success = true;
				response.Code = ErrorCode.OK;
			}

			return response;

		}

		private object ParsePayment(string responseContent)
		{
			object response;
			var parsedResponse = HttpUtility.ParseQueryString(responseContent);

			response = new ApiResponse<CreatePaymentResponse>()
			{
				Code = (ErrorCode)Enum.Parse(typeof(ErrorCode), parsedResponse["code"], true),
				Message = parsedResponse["message"],
				Response = new CreatePaymentResponse()
				{
					RedirectUrl = parsedResponse["redirect"],
					TransactionId = parsedResponse["transId"]
				},
				Success = true
			};
			return response;
		}

	}
}
