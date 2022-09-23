//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;

//namespace christmasinbelfast.Services
//{
//    public class CartService
//    {
//        private readonly HttpClient _http;
//        private readonly ApplicationDbContext _context;

//        public CartService(HttpClient http, ApplicationDbContext context)
//        {
//            _http = http;
//            _context = context;
//        }

//        public List<Purchase> purchases { get; set; }

//        public async Task<List<Purchase>> GetCartItems()
//        {
//            purchases = _context.Purchase.ToList();
//            if (purchases == null)
//            {
//                return new List<Purchase>();
//            }
//            return purchases;
//        }

//        public async Task<string> Checkout()
//        {
//            var result = await _http.PostAsJsonAsync("api/payment/checkout",
//                await GetCartItems());
//            var url = await result.Content.ReadAsStringAsync();
//            return url;
//        }
//    }
//}
