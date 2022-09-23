using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using TestingStripe.Models;
using System.Collections.Generic;
using TestingStripe.Data;
using System.Linq;

namespace TestingStripe.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly HttpClient _http;
        private readonly ApplicationDbContext _context;

        public CartService(HttpClient http,
            ApplicationDbContext context)
        {
            _http = http;
            _context = context;
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            var cart = _context.CartItems.ToList();
            return cart;
        }


        public  async Task<string> Checkout()
        {
            var result = await _http.PostAsJsonAsync("/checkout", await GetCartItems());
            var url = await result.Content.ReadAsStringAsync();
            return url;
        }
    }
}
