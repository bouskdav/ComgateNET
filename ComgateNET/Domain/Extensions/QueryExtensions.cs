using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

namespace ComgateNET.Domain.Extensions
{
    public static class QueryExtensions
	{
		public static string ToQueryString(this NameValueCollection nvc)
		{
			//IEnumerable<string> segments = from key in nvc.AllKeys
			//	from value in nvc.GetValues(key)
			//	select string.Format("{0}={1}",
			//	WebUtility.UrlEncode(key),
			//	WebUtility.UrlEncode(value));

			IEnumerable<string> segments = nvc.AllKeys
				.SelectMany(k => nvc.GetValues(k).Select(v => $"{WebUtility.UrlEncode(k)}={WebUtility.UrlEncode(v)}"));

			return "?" + String.Join("&", segments);
		}
	}
}
