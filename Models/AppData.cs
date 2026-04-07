using Closet_ASP.NET.Models;
using Closet_ASP.NET.Controllers;

namespace Closet_ASP.NET.Models
{
    public static class AppData
    {
        public static List<Product> AllProducts = new List<Product>
        {
            new Product { Id = "1", Title = "Pantt", MainCategory = "Men", SubCategory = "Peater England", Category = "LOWER", Price = 2499, Stock = 95, Images = new List<string> { "/images/BUDA JEANS CO Men Mid Rise Straight Jeans_1.png" } },
            new Product { Id = "2", Title = "Saree", MainCategory = "Women", SubCategory = "Fabindia", Category = "SAREE", Price = 4499, Stock = 44, Images = new List<string> { "/images/Purple Zariwork Soft Silk Saree_1.png" } },
            new Product { Id = "3", Title = "T-Shirt", MainCategory = "Men", SubCategory = "U.S.Polo", Category = "TOP", Price = 699, Stock = 133, Images = new List<string> { "/images/Men's Light Blue Polo T-shirt_1.png" } }
        };

        public static List<UserViewModel> UserList = new List<UserViewModel> {
            new UserViewModel { Name = "abc", Email = "kjh@sf.com", Role = "User", Contact = "56453436545", JoinDate = "2/17/2026", HasLogo = false },
            new UserViewModel { Name = "YASHVI SIDAPARA", Email = "ysidapara688@rku.ac.in", Role = "Admin", Contact = "9875260012", JoinDate = "2/15/2026", HasLogo = false },
            new UserViewModel { Name = "yashvi", Email = "yashvisidapara7@gmail.com", Role = "User", Contact = "9865289629865", JoinDate = "2/2/2026", HasLogo = false },
            new UserViewModel { Name = "Anshil Bhuva", Email = "anshilbhuva@gmail.com", Role = "Admin", Contact = "1234567890", JoinDate = "1/18/2026", HasLogo = false },
            new UserViewModel { Name = "Prem Dipenbhai Shah", Email = "pshah627@rku.ac.in", Role = "User", Contact = "98977878787", JoinDate = "1/15/2026", HasLogo = false },
            new UserViewModel { Name = "Jenish Ramani", Email = "jramani181@rku.ac.in", Role = "Admin", Contact = "1234567890", JoinDate = "1/12/2026", HasLogo = true },
            new UserViewModel { Name = "Anshil", Email = "anshilbhuva9251@gmail.com", Role = "Admin", Contact = "1234567890", JoinDate = "1/12/2026", HasLogo = false }
        };
    }
}
