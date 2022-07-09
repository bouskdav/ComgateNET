using ComgateNET.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ComgateNET.Factories
{
	public class HttpClientFactory
	{
		public static HttpClient CreateHttpClient(ComgateHttpClientType clientFlag, object param = null)
		{
			switch (clientFlag)
			{
				case ComgateHttpClientType.HttpClient: return CreateBasicHttpClient(); break;
				case ComgateHttpClientType.HttpClientWithFiddler: return CreateHttpClientWithFiddler(param); break;
				case ComgateHttpClientType.HttpClientTimeout: return CreateHttpClientTimeOut(); break;
				default: return new HttpClient();
			}
		}

		private static HttpClient CreateBasicHttpClient()
		{
			return new HttpClient();
		}

		private static HttpClient CreateHttpClientWithFiddler(object param)
		{
			WebProxy proxy = new WebProxy("127.0.0.1:8888");
			try
			{
				proxy = param as WebProxy;
			}
			catch (Exception ex)
			{

			}


			HttpClientHandler httpClientHandler = new HttpClientHandler()
			{
				Proxy = proxy
			};
			return new HttpClient(httpClientHandler);

		}

		private static HttpClient CreateHttpClientTimeOut()
		{
			return new HttpClient();
		}
	}
}
