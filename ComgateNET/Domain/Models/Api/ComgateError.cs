namespace ComgateNET.Domain.Models.Api
{
    public class ComgateError
	{
		public Error error { get; set; }
	}

	public class Error
	{
		public int code { get; set; }
		public string message { get; set; }
		public string extraMessage { get; set; }
	}
}
