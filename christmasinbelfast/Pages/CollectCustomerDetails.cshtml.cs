using christmasinbelfast.Data;
using christmasinbelfast.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace christmasinbelfast.Pages
{
    public class CollecttCustomerDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CollecttCustomerDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CartItem> cartItems { get; set; }
        public List<Purchase> purchases { get; set; }
        public List<Product> PublicListOfProducts { get; set; }
        public decimal TotalAmount { get; set; }
        public List<Purchase> SortedList = new List<Purchase>();
        public List<Purchase> PurchasesForTotal { get; set; }
        public string SomePriceId { get; set; }
        public List<CartItem> SortedItemsForPayment = new List<CartItem>();

        [BindProperty]
        public Customer customer { get; set; }

        public IActionResult OnPostAsync()
        {
            //string value = customer.FirstName;
            string message = string.Empty;
            try
            {
                decimal value = customer.TelephoneNumber;
            }
            catch(Exception ex)
            {
                throw ex;               
            }

            purchases = _context.Purchase.ToList();
            var savingCustomer = _context.Customer.Add(customer);

            _context.SaveChanges();

            SortPurchasesForPayment();

            CreateStripeSession();

            return new StatusCodeResult(303);
        }

        public ActionResult CreateStripeSession()
        {
            var lineItems = new List<SessionLineItemOptions>();
            SortedItemsForPayment.ForEach(ci => lineItems.Add(new SessionLineItemOptions
            {

                 Price =  ci.Price,
                 Quantity = ci.Quantity
            }));;

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:44370/Success",
                CancelUrl = "https://localhost:44370/Basket",
            };

            var mainService = new SessionService();
            Session session = mainService.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public void SortPurchasesForPayment()
        {
            SortedList = _context.Purchase.ToList();
            PublicListOfProducts = _context.Product.ToList();

            foreach (var purchase in SortedList)
            {
                CartItem cartItem = new CartItem();

                var findingPrice = PublicListOfProducts.Find(x => x.Id == purchase.ProductId);
                cartItem.Price = findingPrice.StripePriceId;

                cartItem.Quantity = purchase.Quantity;

                SortedItemsForPayment.Add(cartItem);
            }
        }
    }
}
