using ComgateNET.Domain.Enums;

namespace ComgateNET.Domain.Models.Api
{
    public class ApiResponse<T>
	{
		/// <summary>
		///  response from ComGate API server 
		/// </summary>
		public T Response { get; set; }

		/// <summary>
		/// State of response (indicate if server response correctly) 
		/// </summary>
		public bool Success { get; set; }
		/// <summary>
		/// Determinate error
		/// </summary>
		public ErrorCode Code { get; set; }
		/// <summary>
		/// Additional error data 
		/// </summary>
		public string Message { get; set; }

	}
}
