using Stripe.Checkout;
using System.Collections.Generic;
using TestingStripe.Models;

namespace christmasinbelfast.Services
{
    public interface IPaymentService
    {
        Session CreateCheckoutSession(List<CartItem> cartItems); 
    }
}
