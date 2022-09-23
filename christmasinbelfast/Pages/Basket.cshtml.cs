using christmasinbelfast.Data;
using christmasinbelfast.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace christmasinbelfast.Pages
{
    public class BasketModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BasketModel(ApplicationDbContext context)
        {
            _context = context;           
        }

        public List<Purchase> purchases { get; set; }
        public List<Product> PublicListOfProducts { get; set; }
        public decimal TotalAmount { get; set; }
        public List<Purchase> SortedList { get; set; }
        public List<Purchase> PurchasesForTotal { get; set; }
        public string SomePriceId { get; set; }

        public void SortingOutList()
        {           
            var firstPurchase = purchases.FirstOrDefault();

            var sameProductPurchases = purchases.FindAll(x => x.ProductId == firstPurchase.ProductId);

            int combinedQuantity = 0;

            if(sameProductPurchases != null && firstPurchase != null)
            {
                List<int> productPurchasesCombined = new List<int>();

                foreach (var purchase in sameProductPurchases)
                {
                    productPurchasesCombined.Add(purchase.Quantity);
                }

                combinedQuantity = 0;

                if(productPurchasesCombined.Count > 0)
                {
                    combinedQuantity = productPurchasesCombined.Sum();
                }
                else
                {
                    combinedQuantity = firstPurchase.Quantity;
                }
            }
            else
            {
                if(firstPurchase != null)
                {
                    combinedQuantity = firstPurchase.Quantity;
                }
            }

            if(firstPurchase != null)
            {
                Purchase sortedPurchase = new Purchase();

                sortedPurchase.ProductId = firstPurchase.ProductId;

                sortedPurchase.Quantity = combinedQuantity;

                SortedList.Add(sortedPurchase);

                purchases.RemoveAll(x => x.ProductId == firstPurchase.ProductId);
            }
        }

        public void OnGet()
        {
            PublicListOfProducts = _context.Product.ToList();
            purchases = _context.Purchase.ToList();
            PurchasesForTotal = _context.Purchase.ToList();
            SortedList = new List<Purchase>();
            List<decimal> QuantityByPrice = new List<decimal>();


            do
            {
                SortingOutList();
            }
            while (purchases.Count > 0);

            //clears database for new sorted list

            purchases = _context.Purchase.ToList();

            foreach(var purchase in purchases)
            {
                _context.Purchase.Remove(purchase);
                _context.SaveChanges();
            }

            //replaces single purchases with paired purchases
            foreach(var purchase in SortedList)
            {
                _context.Purchase.Add(purchase);
                _context.SaveChanges();
            }

            foreach (var p in PurchasesForTotal)
            {
                var findingPrice = PublicListOfProducts.Find(x => x.Id == p.ProductId);
                var UnitsByPrice = p.Quantity * findingPrice.Price;

                QuantityByPrice.Add(UnitsByPrice);
            }

            TotalAmount = QuantityByPrice.Sum();
        }

        public IActionResult OnPost()
        {
            purchases = _context.Purchase.ToList();
            if (purchases != null)
            {
                return new RedirectToPageResult("/CollectCustomerDetails");
            }
            return Page();
        }
    }
}
