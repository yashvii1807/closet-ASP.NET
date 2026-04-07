using System.Diagnostics;
using Closet_ASP.NET.Models;
using Microsoft.AspNetCore.Mvc;

namespace Closet_ASP.NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private List<Product> _allProducts => AppData.AllProducts;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string pageCategory, string selectedSubCategory, string priceRange, string sortBy)
        {
            var products = _allProducts;

            var filtered = products.AsQueryable();

            if (!string.IsNullOrEmpty(pageCategory) && pageCategory != "All")
            {
                filtered = filtered.Where(p => p.MainCategory == pageCategory);
            }

            if (!string.IsNullOrEmpty(selectedSubCategory) && selectedSubCategory != "All")
            {
                filtered = filtered.Where(p => p.Category == selectedSubCategory);
            }

            if (!string.IsNullOrEmpty(priceRange) && priceRange != "all")
            {
                switch (priceRange)
                {
                    case "under500":
                        filtered = filtered.Where(p => p.Price < 500);
                        break;
                    case "500-1000":
                        filtered = filtered.Where(p => p.Price >= 500 && p.Price <= 1000);
                        break;
                    case "1000-1500":
                        filtered = filtered.Where(p => p.Price >= 1000 && p.Price <= 1500);
                        break;
                    case "1500-2000":
                        filtered = filtered.Where(p => p.Price >= 1500 && p.Price <= 2000);
                        break;
                    case "above2000":
                        filtered = filtered.Where(p => p.Price > 2000);
                        break;
                }
            }

            // Sorting Logic
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name":
                        filtered = filtered.OrderBy(p => p.Title);
                        break;
                    case "priceLow":
                        filtered = filtered.OrderBy(p => p.Price);
                        break;
                    case "priceHigh":
                        filtered = filtered.OrderByDescending(p => p.Price);
                        break;
                    case "rating":
                        filtered = filtered.OrderByDescending(p => p.AverageRating);
                        break;
                }
            }

            ViewBag.Products = filtered.ToList();
            ViewBag.AllProducts = products;
            ViewBag.PageCategory = pageCategory;
            ViewBag.SelectedSub = selectedSubCategory;
            ViewBag.PriceRange = priceRange;
            ViewBag.SortBy = sortBy;

            return View();
        }

        public IActionResult Details(string id)
        {
            var product = _allProducts.FirstOrDefault(p => p.Id == id) ?? _allProducts.First();
            return View(product);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password, string role)
        {
            HttpContext.Session.SetString("UserEmail", email);
            HttpContext.Session.SetString("UserRole", role ?? "User");
            
            if (role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string fullName, string email, string phone, string password, string role)
        {
            HttpContext.Session.SetString("UserEmail", email);
            HttpContext.Session.SetString("UserRole", role ?? "User");
            
            if (role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult RateProduct(string id, int rating)
        {
            var product = _allProducts.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                // Simple weighted average calculation
                double totalScore = (product.AverageRating * product.NumRatings) + rating;
                product.NumRatings++;
                product.AverageRating = Math.Round(totalScore / product.NumRatings, 1);
            }
            return Json(new { success = true, newAvg = product?.AverageRating, newCount = product?.NumRatings });
        }

        public IActionResult About()
        {
            return View();
        }

        public static List<Product> CartItems { get; set; } = new List<Product>();

        public IActionResult AddToCart(string id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail"))) return RedirectToAction("Login");
            
            var p = _allProducts.FirstOrDefault(x => x.Id == id);
            if (p != null)
            {
                var existing = CartItems.FirstOrDefault(c => c.Id == id);
                if (existing != null)
                {
                    existing.Quantity++;
                }
                else
                {
                    p.Quantity = 1;
                    CartItems.Add(p);
                }
            }
            return RedirectToAction("Cart");
        }

        public IActionResult UpdateQuantity(string id, int delta)
        {
            var item = CartItems.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                item.Quantity += delta;
                if (item.Quantity < 1) item.Quantity = 1;
            }
            return RedirectToAction("Cart");
        }

        public IActionResult RemoveFromCart(string id)
        {
            var item = CartItems.FirstOrDefault(c => c.Id == id);
            if (item != null) CartItems.Remove(item);
            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail"))) return RedirectToAction("Login");
            ViewBag.CartItems = CartItems;
            return View();
        }

        public IActionResult Checkout()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail"))) return RedirectToAction("Login");
            ViewBag.CartItems = CartItems;
            return View();
        }

        public IActionResult ClearCart()
        {
            CartItems.Clear();
            return RedirectToAction("Cart");
        }

        public IActionResult Contact()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Here you would typically send an email or save to a database
                TempData["ContactSuccess"] = "Your message has been sent successfully!";
                return RedirectToAction("Contact");
            }
            return View(model);
        }

        public static List<Product> WishlistItems { get; set; } = new List<Product>();

        public IActionResult AddToWishlist(string id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail"))) return RedirectToAction("Login");
            
            var p = _allProducts.FirstOrDefault(x => x.Id == id);
            if (p != null && !WishlistItems.Any(c => c.Id == id))
                WishlistItems.Add(p);
            return RedirectToAction("Wishlist");
        }

        public IActionResult RemoveFromWishlist(string id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail"))) return RedirectToAction("Login");
            var item = WishlistItems.FirstOrDefault(c => c.Id == id);
            if (item != null)
                WishlistItems.Remove(item);
            return RedirectToAction("Wishlist");
        }

        public IActionResult Wishlist()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail"))) return RedirectToAction("Login");
            ViewBag.WishlistItems = WishlistItems;
            return View();
        }

        public IActionResult Profile()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail)) return RedirectToAction("Login");

            // Fetch user from static data
            var user = AppData.UserList.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                // Fallback for demo purposes if not in static list
                return View(new UserProfileViewModel
                {
                    Name = "YASHVI SIDAPARA",
                    Email = userEmail,
                    Contact = "7185718581",
                    JoinDate = "2024"
                });
            }

            return View(new UserProfileViewModel
            {
                Name = user.Name,
                Email = user.Email,
                Contact = user.Contact,
                JoinDate = user.JoinDate ?? "2024"
            });
        }

        [HttpPost]
        public IActionResult Profile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Update user in static list
            var user = AppData.UserList.FirstOrDefault(u => u.Email == model.Email);
            if (user != null)
            {
                user.Name = model.Name;
                user.Contact = model.Contact;
            }

            // Add success message if possible, but let's just stay on view for now
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return View(model);
        }
        
        public IActionResult Orders()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail"))) return RedirectToAction("Login");
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
