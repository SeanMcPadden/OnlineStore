using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Linq;
using TestingStripe.Data;
using TestingStripe.Models;

namespace christmasinbelfast.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            StripeConfiguration.ApiKey = "sk_test_26PHem9AhJZvU623DfE1x4sd";
            _context = context;
        }

        public Session CreateCheckoutSession(List<CartItem> cartItems)
        {
            cartItems = _context.CartItems.ToList();

            var lineItems = new List<SessionLineItemOptions>();
            cartItems.ForEach(ci => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {

                    UnitAmountDecimal = ci.Price,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = ci.ProductName,
                        Images = new List<string> { ci.Image }
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
    }
}
