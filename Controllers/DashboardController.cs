using System.Diagnostics;
using Closet_ASP.NET.Models;
using Microsoft.AspNetCore.Mvc;

namespace Closet_ASP.NET.Controllers
{

    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(
            string? pageCategory,
            string selectedMainCategory = "All",
            string selectedSubCategory = "All",
            string sortBy = "name",
            string priceRange = "all",
            string viewMode = "grid")
        {
            var client = _httpClientFactory.CreateClient();
            var url = "http://localhost:4000/api/products";
            if (!string.IsNullOrEmpty(pageCategory))
                url += $"?mainCategory={pageCategory}";

            List<Product> products = new();
            try
            {
                var response = await client.GetFromJsonAsync<List<Product>>(url);
                products = response ?? new();
            }
            catch { TempData["Error"] = "Failed to load products"; }

            // Filter
            var filtered = products.AsQueryable();
            if (!string.IsNullOrEmpty(pageCategory))
                filtered = filtered.Where(p => p.MainCategory == pageCategory);
            else if (selectedMainCategory != "All")
                filtered = filtered.Where(p => p.MainCategory == selectedMainCategory);

            if (selectedSubCategory != "All")
                filtered = filtered.Where(p => p.SubCategory == selectedSubCategory);

            filtered = priceRange switch
            {
                "under500" => filtered.Where(p => p.Price < 500),
                "500-1000" => filtered.Where(p => p.Price >= 500 && p.Price <= 1000),
                "1000-1500" => filtered.Where(p => p.Price >= 1000 && p.Price <= 1500),
                "1500-2000" => filtered.Where(p => p.Price >= 1500 && p.Price <= 2000),
                "above2000" => filtered.Where(p => p.Price > 2000),
                _ => filtered
            };

            var sorted = sortBy switch
            {
                "price-low" => filtered.OrderBy(p => p.Price),
                "price-high" => filtered.OrderByDescending(p => p.Price),
                "rating" => filtered.OrderByDescending(p => p.AverageRating),
                _ => filtered.OrderBy(p => p.Title)
            };

            ViewBag.Products = sorted.ToList();
            ViewBag.AllProducts = products;
            ViewBag.PageCategory = pageCategory;
            ViewBag.SelectedMain = selectedMainCategory;
            ViewBag.SelectedSub = selectedSubCategory;
            ViewBag.SortBy = sortBy;
            ViewBag.PriceRange = priceRange;
            ViewBag.ViewMode = viewMode;

            return View();
        }
    }
}
