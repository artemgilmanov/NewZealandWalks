using Microsoft.AspNetCore.Mvc;
using NZWalksUI.Models;
using NZWalksUI.Models.DTO;
using System.Text;
using System.Text.Json;

namespace NZWalksUI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            List<RegionDto> responce = new List<RegionDto>();
            try
            {
                //Get all Regions from Web API
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("http://localhost:5063/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                //var stringResponseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                responce.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());


                //ViewBag.Response = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>();
            }
            catch (Exception ex)
            {
                // log exception
            }

            return View(responce);
        }

        [HttpGet]
        public IActionResult Add() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel regionViewModel)
        {
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage() 
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:5063/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(regionViewModel), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response is not null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) 
        {
            var client = httpClientFactory.CreateClient();
           
            var response = await client.GetFromJsonAsync<RegionDto>($"http://localhost:5063/api/regions/{id.ToString()}");
            
            if (response is not null) 
            {
                return View(response);
            }

            return View(null);
        }
    }
}
