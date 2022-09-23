using christmasinbelfast.Data;
using christmasinbelfast.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace christmasinbelfast.Pages
{
    public class SuccessModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public SuccessModel(ApplicationDbContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public List<Purchase> purchases { get; set; }
        public List<CartItem> cartItems { get; set; }

        public void OnGet()
        {
            purchases = _context.Purchase.ToList();

            foreach (var purchase in purchases)
            {
                _context.Purchase.Remove(purchase);
            }
            _context.SaveChanges();

            SendEmail("Thank you for your order. Your product(s) will be with you shortly." +
                "Merry Christmas.");
        }

        public void SendEmail(string body)
        {
            var customers = _context.Customer.ToList();

            var customer = customers.LastOrDefault();

            var customerEmail = customer.EmailAddress.ToString(); 

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(customerEmail));
            email.Subject = "Thank you for your order.";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);            
        }
    }
}
