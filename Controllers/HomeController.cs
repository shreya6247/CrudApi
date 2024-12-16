using CRUDusingApi.Models.BEL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CRUDusingApi.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewData["val"] = TempData["msg"];
			IEnumerable<Users> user = null;
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri("https://localhost:44341");
			var consumedata = client.GetAsync("/Api/User");
			consumedata.Wait();
			var dataRead = consumedata.Result;
			if (dataRead.IsSuccessStatusCode)
			{
				var results = dataRead.Content.ReadAsAsync<IList<Users>>();
				results.Wait();
				user = results.Result;
			}
			else
			{
				user = Enumerable.Empty<Users>();
				ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
			}
			return View(user);
		}

		public ActionResult SignUp()
		{
			ViewData["val"] = TempData["msg"];
			return View();
		}

		[HttpPost]

		public async Task<ActionResult> SignUp(Users users)
		{
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri("https://localhost:44341");
			
			var jsoncontent = JsonConvert.SerializeObject(users);
			var content = new StringContent(jsoncontent, Encoding.UTF8,"application/json");

			HttpResponseMessage resp = await client.PostAsync("Api/User",content);
			if (resp.IsSuccessStatusCode)
			{
				string responseMessage = await resp.Content.ReadAsStringAsync();
				TempData["msg"] = responseMessage;
				return RedirectToAction("Index");
			}

			else
			{
				string errorMessage = await resp.Content.ReadAsStringAsync();
				TempData["msg"] = errorMessage;
				return RedirectToAction("SignUp");
			}
		}

		public async Task<ActionResult> Delete(int id)
		{
			HttpClient httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri("https://localhost:44341");

			HttpResponseMessage resp = await httpClient.DeleteAsync($"/Api/User/{id}");
			if (resp.IsSuccessStatusCode)
			{
				string msg = await resp.Content.ReadAsStringAsync();
				TempData["msg"] = msg;
				return RedirectToAction("Index");
			}
			else
			{
				string msg = await resp.Content.ReadAsStringAsync();
				TempData["msg"] = msg;
				return RedirectToAction("Index");
			}


		}

		[HttpGet]
		public ActionResult Update(int id)
		{
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri("https://localhost:44341");

			var consumedata = client.GetAsync($"/Api/User/{id}");
			consumedata.Wait();

			//Users user = new Users();
			var dataread = consumedata.Result;
			var userdet = dataread.Content.ReadAsAsync<Users>().Result;
			return View(userdet);
		}
		[HttpPost]
		public async Task<ActionResult> Update(int id,Users users)
		{
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri("https://localhost:44341");

			var jsonContent = JsonConvert.SerializeObject(users);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			HttpResponseMessage resp = await client.PutAsync($"Api/User/{id}",content);

			string msg = await resp.Content.ReadAsStringAsync();
			TempData["msg"] = msg;
			return RedirectToAction("Index");

		}
		
	}
}
