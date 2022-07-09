using ComgateNET.Domain.Models.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ComgateNET.Abstraction
{
    public interface IComgateContentSerializer
    {
		/// <summary>
		/// create http content
		/// </summary>
		FormUrlEncodedContent Serialize<T>(T request);

		/// <summary>
		///  Create response from url params
		/// </summary>
		/// <param name="url"></param>
		ApiResponse<T> Deserialize<T>(string responseContent);
	}
}
