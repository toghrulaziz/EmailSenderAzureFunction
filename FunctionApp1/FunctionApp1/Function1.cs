using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;

namespace FunctionApp1
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("EmailSender")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            // Read the request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string name = data?.name;
            string email = data?.email;
            string message = data?.message;

            if (!string.IsNullOrEmpty(email))
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("togrul1609@gmail.com", "dxtu ztzl iwef lzlm"),
                    EnableSsl = true,
                };

                // Create the email message
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("togrul1609@gmail.com"),
                    Subject = "New message from your website contact form",
                    Body = $"Name: {name}\nEmail: {email}\nMessage: {message}",
                };
                mailMessage.To.Add(new MailAddress(email));

                smtpClient.Send(mailMessage);
            }

            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}