using christmasinbelfast.Data;
using christmasinbelfast.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace christmasinbelfast.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CheckoutModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Session CreateCheckoutSession(List<Purchase> cartItems)
        {

            List<Models.Product> products = new List<Models.Product>();
            products = _context.Product.ToList();
            cartItems = _context.Purchase.ToList();

            var lineItems = new List<SessionLineItemOptions>();
            cartItems.ForEach(ci => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {

                    UnitAmountDecimal = 1000,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Whats The Story",
                        Images = new List<string> { ci.ToString() } 
                    }
                },
                Quantity = ci.Quantity
            }));

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:5001/Success",
                CancelUrl = "https://localhost:5001/Basket",
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }

        public void OnGet()
        {

        }

        public void OnPost(List<Purchase> cartItems)
        {
            CreateCheckoutSession(cartItems);

            //var session = _paymentService.CreateCheckoutSession(cartItems);
            //return OkResult(session.Url);

            //var domain = "https://localhost:4242";
            //var options = new SessionCreateOptions
            //{
            //    LineItems = new List<SessionLineItemOptions>
            //    {
            //        new SessionLineItemOptions
            //        {
            //            Price = "10.00",
            //            Quantity = 1,
            //        },
            //    },
            //    Mode = "Payment",
            //    SuccessUrl = domain + "/Success",
            //    CancelUrl = domain + "/Cancel",
            //};
            //var service = new SessionService();
            //Session session = service.Create(options);

            //Response.Headers.Add("Location", session.Url);
            //var done = new StatusCodeResult(303);
        }
    }
}
