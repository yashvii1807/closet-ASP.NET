namespace Closet_ASP.NET.Models
{
        public class Product
        {
            public string? Id { get; set; }
            public string? Title { get; set; }
            public string? MainCategory { get; set; }
            public string? SubCategory { get; set; }
            public string? Category { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public double AverageRating { get; set; }
            public int NumRatings { get; set; }
            public List<string>? Images { get; set; }
        }
    
}
