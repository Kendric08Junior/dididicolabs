using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

namespace DuelCoreLabs.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _config;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IConfiguration config, ILogger<HomeController> logger)
    {
        _config = config;
        _logger = logger;
    }

    public IActionResult Index() => View();
    public IActionResult Services() => View();
    public IActionResult Team() => View();
    public IActionResult Trust() => View();

    [HttpGet]
    public IActionResult Contact() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(string name, string email, string? phone, string? service, string message)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
        {
            TempData["ContactError"] = "Please fill in your name, email and message.";
            return RedirectToAction(nameof(Contact));
        }

        var smtp = _config.GetSection("Smtp");
        var host = smtp["Host"];
        var user = smtp["User"];
        var password = smtp["Password"];

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning("SMTP is not configured. Message from {Email} was not sent.", email);
            TempData["ContactError"] = "Email sending isn't set up yet. Please email us at hello@dicorelabs.com.";
            return RedirectToAction(nameof(Contact));
        }

        try
        {
            using var client = new SmtpClient(host, int.TryParse(smtp["Port"], out var port) ? port : 587)
            {
                Credentials = new NetworkCredential(user, password),
                EnableSsl = true
            };

            using var mail = new MailMessage
            {
                From = new MailAddress(user, "Di-Core Labs Website"),
                Subject = $"New enquiry from {name}",
                Body = $"Name: {name}\nEmail: {email}\nPhone: {phone}\nService: {service}\n\n{message}"
            };
            mail.To.Add("hello@dicorelabs.com");
            mail.ReplyToList.Add(new MailAddress(email));

            await client.SendMailAsync(mail);
            TempData["ContactSuccess"] = "Thanks! We'll get back to you within 24 hours.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send contact email");
            TempData["ContactError"] = "Sorry, your message couldn't be sent. Please email us at hello@dicorelabs.com.";
        }

        return RedirectToAction(nameof(Contact));
    }

    public IActionResult Error() => Content("Something went wrong. Please try again.");
}
