using ImagePlatform.Models;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using System.Security.Claims;
using ImagePlatform.Repository.UserRepository;
using ImagePlatform.Services.EmailService;

namespace ImagePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IEmailService emailService;

        public CheckoutController(IUserRepository userRepository, IEmailService emailService)
        {
            this.userRepository = userRepository;
            this.emailService = emailService;
        }

        [HttpGet]
        public IActionResult PayPremium(string email, string token, string authToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            var user = userRepository.getUserByToken(authToken);
            user.isUserPremium = true;
            userRepository.UpdateUser(user);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = email,
                Source = token
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(8.99) * 100),
                Description = "Premium+ Membership",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                emailService.SendEmail(email);
                return Ok();
            }
                

            else return BadRequest("Payment Failed");
        }
    }
}
