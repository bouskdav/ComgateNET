using ComgateNET.Abstraction.Api;
using ComgateNET.Domain.Enums;
using ComgateNET.Domain.Models.Payment;
using ComgateNET.Factories;
using ComgateNET.Services;
using ComgateNET.WebTest.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ComgateNET.WebTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public HomeController(
			ILogger<HomeController> logger,
			IConfiguration configuration,
			IWebHostEnvironment environment)
        {
            _logger = logger;
            _configuration = configuration;
            _environment = environment;
        }

		public async Task<IActionResult> Index()
		{
			return View();
		}


		public async Task<IActionResult> Methods()
		{
			// create Comgate connector 
			IComgateApi comGateAPI = ComgateApiConnector.CreateConnector(_configuration.GetValue<string>("api"))
				.TestEnviroment()
				.SetLang()
				.SetMerchant(_configuration.GetValue<string>("merchantId"))
				.SetSecret(_configuration.GetValue<string>("secret"));

			// get available payment methods
			var apiResponse = await comGateAPI.GetAvailebleMethods();

			return View(apiResponse);
		}

		public async Task<IActionResult> CreatePayment()
		{
			BasicPaymentViewModel viewModel = new BasicPaymentViewModel()
			{
				Email = "EmailPlatce@email.cz",
				Name = "Jméno zákazníka",
				Label = "Krátký popis platby",
				ReferenceId = Guid.NewGuid().ToString()
			};
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> CreatePayment(BasicPaymentViewModel model)
		{
			// create Comgate connector 
			IComgateApi comGateAPI = ComgateApiConnector.CreateConnector(_configuration.GetValue<string>("api"))
				.TestEnviroment()
				.SetLang()
				.SetMerchant(_configuration.GetValue<string>("merchantId"))
				.SetSecret(_configuration.GetValue<string>("secret"));

			var cents = (int)(model.Price * 100);

			BaseComgatePayment payment = PaymentFactory.GetBasePayment(cents, model.ReferenceId, model.Label, PaymentMethod.ALL);

			Payer customer = new Payer();

			customer.Contact = new Contact()
			{
				Email = model.Email,
				Name = model.Name
			};

			var response = await comGateAPI.CreatePayment(payment, customer);

			return Redirect(response.Response.RedirectUrl);
		}


		public async Task<IActionResult> Payment()
		{

			AdvancedPaymentViewModel viewModel = new AdvancedPaymentViewModel()
			{
				Email = "EmailPlatce@email.cz",
				Name = "Jméno zákazníka",
				Label = "Krátký popis platby",
				ReferenceId = Guid.NewGuid().ToString(),
				Currencies = (Enum.GetValues(typeof(Currency)).Cast<Currency>().Select(
					enu => new SelectListItem() { Text = enu.ToString(), Value = enu.ToString() })).ToList(),

				PaymentMethods = (Enum.GetValues(typeof(PaymentMethod)).Cast<PaymentMethod>().Select(
					enu => new SelectListItem() { Text = enu.ToString(), Value = enu.ToString() })).ToList()

			};

			// test eet value
			viewModel.Price = 495;
			viewModel.EETData = "{\"celk_trzba\": \"49500\",\"zakl_nepodl_dph\": \"0\",\"zakl_dan1\": \"4132\",\"dan1\": \"868\",\"zakl_dan2\": \"10000\",\"dan2\": \"1500\",\"zakl_dan3\": \"0\",\"dan3\": \"0\",\"cest_sluz\": \"0\",\"pouzit_zboz1\": \"33000\",\"pouzit_zboz2\": \"0\",\"pouzit_zboz3\": \"0\",\"urceno_cerp_zuct\": \"0\",\"cerp_zuct\": \"0\"}";

			IComgateApi comGateAPI = ComgateApiConnector.CreateConnector(_configuration.GetValue<string>("api"))
			.TestEnviroment()
			.SetLang()
			.SetMerchant(_configuration.GetValue<string>("merchantId"))
			.SetSecret(_configuration.GetValue<string>("secret"));

			var apiResponse = await comGateAPI.GetAvailebleMethods();

			if (apiResponse.Success)
			{
				viewModel.PaymentMethods = apiResponse.Response.methods.Select(e => new SelectListItem() { Value = e.name, Text = e.id }).ToList();
				viewModel.Methods = apiResponse.Response.methods;
			}

			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Payment(AdvancedPaymentViewModel model)
		{
			// create Comgate connector 
			IComgateApi comGateAPI = ComgateApiConnector.CreateConnector(_configuration.GetValue<string>("api"))
				.TestEnviroment()
				.SetLang()
				.SetMerchant(_configuration.GetValue<string>("merchantId"))
				.SetSecret(_configuration.GetValue<string>("secret"));

			var cents = (int)(model.Price * 100);

			BaseComgatePayment payment = PaymentFactory.GetBasePayment(cents, model.ReferenceId, model.Label, model.SelectedMethod);

			payment.Currency = model.SelectedCurrency;
			payment.Preauth = model.PreAuth;
			payment.PrepareOnly = true;
			payment.EET = JsonConvert.DeserializeObject<EetData>(model.EETData);
			payment.EetReport = true;

			Payer customer = new Payer();
			customer.Contact = new Contact() { Email = model.Email, Name = model.Name };

			var response = await comGateAPI.CreatePayment(payment, customer);


			return Redirect(response.Response.RedirectUrl);
		}


		public async Task<IActionResult> GetPaymentStatus(string transId)
		{

			// create Comgate connector 
			IComgateApi comGateAPI = ComgateApiConnector.CreateConnector(_configuration.GetValue<string>("api"))
				.TestEnviroment()
				.SetLang()
				.SetMerchant(_configuration.GetValue<string>("merchantId"))
				.SetSecret(_configuration.GetValue<string>("secret"));

			var apiResponse = await comGateAPI.GetPaymentStatus(transId);


			return View(apiResponse);

		}

		public async Task<IActionResult> PaymentsList()
		{
			// get payments log 

			var directory = new DirectoryInfo(Path.Combine(_environment.ContentRootPath, "DRead"));
			var files = directory.GetFiles("*.txt");
			List<CreatePaymentViewModel> paymentList = new List<CreatePaymentViewModel>();

			foreach (var file in files)
			{
				paymentList.Add(ParseFile(file));
			}

			return View(paymentList);

		}


		private CreatePaymentViewModel ParseFile(FileInfo file)
		{

			List<KeyValuePair<string, string>> output = new List<KeyValuePair<string, string>>();

			var lines = System.IO.File.ReadLines(file.FullName);

			var transId = string.Empty;
			var refId = string.Empty;
			var amount = string.Empty;

			foreach (var line in lines)
			{
				var keyValue = line.Split(':');
				// secret nezobrazime
				if (keyValue[0] != "secret")
				{
					output.Add(new KeyValuePair<string, string>(keyValue[0], keyValue[1]));
				}
			}

			return new CreatePaymentViewModel()
			{
				TransId = output.First(e => e.Key == "transId").Value,
				RefId = output.First(e => e.Key == "refId").Value,
				Amount = output.First(e => e.Key == "price").Value,
				State = (PaymentState)Enum.Parse(typeof(PaymentState), output.First(e => e.Key == "status").Value, true),
			};

		}

		public async Task<IActionResult> RefundPayment(string transId)
		{
			RefundPaymentViewModel model = new RefundPaymentViewModel();
			model.TransactionId = transId;
			model.Amount = 0;
			model.Currencies = (Enum.GetValues(typeof(Currency)).Cast<Currency>().Select(
			   enu => new SelectListItem() { Text = enu.ToString(), Value = enu.ToString() })).ToList();

			return View(model);

		}

		[HttpPost]
		public async Task<IActionResult> RefundPayment(RefundPaymentViewModel refund)
		{
			// create Comgate connector 
			IComgateApi comGateAPI = ComgateApiConnector.CreateConnector(_configuration.GetValue<string>("api"))
				.TestEnviroment()
				.SetLang()
				.SetMerchant(_configuration.GetValue<string>("merchantId"))
				.SetSecret(_configuration.GetValue<string>("secret"));

			var cents = (int)(refund.Amount * 100);

			var response = await comGateAPI.RefundPayment(refund.TransactionId, cents, refund.Currency, true);
			return View("RefundComplete", response);

		}


		public async Task<IActionResult> CancelPreAuth(string transId)
		{
			// create Comgate connector 
			IComgateApi comGateAPI = ComgateApiConnector.CreateConnector(_configuration.GetValue<string>("api"))
				.TestEnviroment()
				.SetLang()
				.SetMerchant(_configuration.GetValue<string>("merchantId"))
				.SetSecret(_configuration.GetValue<string>("secret"));

			var response = await comGateAPI.CancelPreauth(transId);
			return View(response);
		}

		public async Task<IActionResult> CapturePreAuth(string transId)
		{
			// create Comgate connector 
			IComgateApi comGateAPI = ComgateApiConnector.CreateConnector(_configuration.GetValue<string>("api"))
				.TestEnviroment()
				.SetLang()
				.SetMerchant(_configuration.GetValue<string>("merchantId"))
				.SetSecret(_configuration.GetValue<string>("secret"));

			var response = await comGateAPI.CapturePreauth(transId);
			return View(response);

		}


		[HttpPost]
		public async Task<IActionResult> Status()
		{
			var path = Path.Combine(_environment.ContentRootPath, "DRead", Request.Form["refId"] + ".txt");

			// check if file exists
			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}

			//using (System.IO.StreamWriter file =
			//new System.IO.StreamWriter(path, true))
			//{
			//	// Write data to FILE

			//	var items = Request.Form.Keys.SelectMany(Request.Form.GetValues, (k, v) => new { key = k, value = v });
			//	foreach (var item in items)
			//		file.WriteLine("{0}:{1}", item.key, item.value);

			//}
			return Content("code=0&message=OK", "application/x-www-form-urlencoded; charset=utf-8");
		}


		public async Task<IActionResult> PaymentCallback(string id, string refId)
		{

			List<KeyValuePair<string, string>> output = new List<KeyValuePair<string, string>>();

			var lines = System.IO.File.ReadLines(Path.Combine(_environment.ContentRootPath, "DRead", refId + ".txt"));
			foreach (var line in lines)
			{
				var keyValue = line.Split(':');

				// secret nezobrazime
				if (keyValue[0] != "secret")
				{
					output.Add(new KeyValuePair<string, string>(keyValue[0], keyValue[1]));
				}

			}

			return View(output);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
