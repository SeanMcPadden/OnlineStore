using christmasinbelfast.Data;
using christmasinbelfast.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace christmasinbelfast.Pages
{
    public class CenterpieceModel : PageModel
    {
        public readonly ApplicationDbContext _context;

        public CenterpieceModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Product> ProductList { get; set; }
        public List<Product> ProductsThatIncludeCenterpieces = new List<Product>();
        public List<Purchase> PurchaseList = new List<Purchase>();
        public int LastAddedToBasket = new int();


        public void OnGet()
        {
            PurchaseList = _context.Purchase.ToList();
            ProductList = _context.Product.ToList();

            foreach (Product product in ProductList)
            {
                if (product.ProductName.ToString().ToLower().Contains("centerpiece"))
                {
                    ProductsThatIncludeCenterpieces.Add(product);
                }
            }
        }

        [BindProperty]
        public Purchase purchase { get; set; }

        public void OnPost()
        {
            PurchaseList.Add(purchase);

            var basket = _context.Purchase.Add(purchase);

            _context.SaveChanges();

            var lastToBeAdded = PurchaseList.LastOrDefault();
            LastAddedToBasket = lastToBeAdded.ProductId;

            OnGet();
        }
    }
}
