using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.DTOs.BasketDTOs
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }              

        [Required]
        public string ProductName { get; set; } = null!;

        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }

        public string PictureUrl { get; set; } = null!;

        public string Brand { get; set; } = null!;

        public string Category { get; set; } = null!;
    }
}
