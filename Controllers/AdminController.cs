using System.Diagnostics;
using Closet_ASP.NET.Models;
using Microsoft.AspNetCore.Mvc;

namespace Closet_ASP.NET.Controllers
{

    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Products()
        {
            var client = _httpClientFactory.CreateClient();
            var url = "http://localhost:4000/api/products";
            
            List<Product> products = new();
            try
            {
                var response = await client.GetFromJsonAsync<List<Product>>(url);
                products = response ?? new();
            }
            catch 
            { 
               // For testing/mocking if API fails based on the screenshot
               products = new List<Product>
               {
                   new Product { Title = "Shirst", MainCategory = "Women", SubCategory = "Fabindia", Category = "SAREE", Price = 4499, Stock = 44 },
                   new Product { Title = "Pantt", MainCategory = "Men", SubCategory = "Peater England", Category = "LOWER", Price = 2499, Stock = 95 },
                   new Product { Title = "T-Shirt", MainCategory = "Men", SubCategory = "U.S.Polo", Category = "TOP", Price = 699, Stock = 133 }
               };
            }

            ViewBag.Products = products;
            return View();
        }
    }
}
