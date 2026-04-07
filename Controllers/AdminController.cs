using System.Diagnostics;
using Closet_ASP.NET.Models;
using Microsoft.AspNetCore.Mvc;

namespace Closet_ASP.NET.Controllers
{

    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        // Reference centralized data
        public static List<UserViewModel> UserList => AppData.UserList;
        public static List<Product> ProductList => AppData.AllProducts;


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
            
            // For testing/mocking if API fails or simply to use local list
            ViewBag.Products = ProductList;
            return View();
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product newProduct)
        {
            if (newProduct != null)
            {
                newProduct.Id = (ProductList.Count + 1).ToString();
                newProduct.Images = new List<string> { "/images/Men's Light Blue Polo T-shirt_1.png" };
                newProduct.Stock = 100;
                ProductList.Insert(0, newProduct);
            }
            return RedirectToAction("Products");
        }

        [HttpPost]
        public IActionResult DeleteProduct(string title)
        {
            var product = ProductList.FirstOrDefault(p => p.Title == title);
            if (product != null)
            {
                ProductList.Remove(product);
            }
            return RedirectToAction("Products");
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(UserViewModel newUser)
        {
            if (newUser != null)
            {
                newUser.JoinDate = DateTime.Now.ToShortDateString();
                newUser.HasLogo = false;
                UserList.Add(newUser);
            }
            return RedirectToAction("Users");
        }

        [HttpPost]
        public IActionResult DeleteUser(string email)
        {
            var user = UserList.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                UserList.Remove(user);
            }
            return RedirectToAction("Users");
        }

        public IActionResult Users()
        {
            ViewBag.Users = UserList;
            return View();
        }
        
        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult Reviews()
        {
            return View();
        }

        public IActionResult Messages()
        {
            return View();
        }
    }

    public class UserViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Contact { get; set; }
        public string JoinDate { get; set; }
        public bool HasLogo { get; set; }
    }
}
