using christmasinbelfast.Data;
using christmasinbelfast.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace christmasinbelfast.Pages
{
    public class wreathsModel : PageModel
    {
        public readonly ApplicationDbContext _context;

        public wreathsModel(ApplicationDbContext context)
        {
            _context = context;            
        }

        public List<Product> ProductList { get; set; }
        public List<Product> ProductsThatIncludeWreaths = new List<Product>();
        public List<Purchase> PurchaseList = new List<Purchase>();
        public int LastAddedToBasket = new int();


        public void OnGet()
        {
            PurchaseList = _context.Purchase.ToList();
            ProductList = _context.Product.ToList();
            LastAddedToBasket = 0;

            foreach (Product product in ProductList)
            {
                if (product.ProductName.ToString().ToLower().Contains("wreath"))
                {
                    ProductsThatIncludeWreaths.Add(product);
                }
            }
            if(PurchaseList.Count == 0)
            {

            }
            else
            {
                var lastToBeAdded = PurchaseList.LastOrDefault();
                LastAddedToBasket = lastToBeAdded.ProductId;
            }
        }

        [BindProperty]
        public Purchase purchase { get; set; }

        public void OnPost()
        {
            PurchaseList.Add(purchase);

            var basket = _context.Purchase.Add(purchase);

            _context.SaveChanges();

            OnGet();
        }


    }
}
