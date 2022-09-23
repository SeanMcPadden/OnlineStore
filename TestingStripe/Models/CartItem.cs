﻿namespace TestingStripe.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Image { get; set; }
    }
}
