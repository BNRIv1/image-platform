using MailKit.Net.Smtp;
using MimeKit;

namespace ImagePlatform.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration config;

        public EmailService(IConfiguration config)
        {
            this.config = config;
        }

        public void SendEmail(string emailAddress)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("jude.walker@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(emailAddress));
            email.Subject = "Premium+ Subscription";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<h1>Thank you for becoming a Premium+ Member!</h1>"
            };

            using var smtp = new SmtpClient();
            smtp.Connect(config.GetSection("EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            //smtp.Connect("smtp.gmail.com");
            smtp.Authenticate(config.GetSection("EmailUsername").Value, config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
