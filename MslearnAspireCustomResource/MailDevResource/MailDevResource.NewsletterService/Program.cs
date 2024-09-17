using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapPost("/subscribe", async (SmtpClient smtpClient, string email) =>
{   
    using var message = new MailMessage("newsletter@yourcompany.com", email)
    {
        Subject = "Welcome to our newsletter!",
        Body = "Thank you for subscribing to our newsletter!"
    };

    var smtpUri = new Uri(builder.Configuration.GetConnectionString("maildev")!);
    smtpClient = new SmtpClient(smtpUri.Host, smtpUri.Port);

    await smtpClient.SendMailAsync(message);
});

app.MapPost("/unsubscribe", async (SmtpClient smtpClient, string email) =>
{
    using var message = new MailMessage("newsletter@yourcompany.com", email)
    {
        Subject = "You are unsubscribed from our newsletter!",
        Body = "Sorry to see you go. We hope you will come back soon!"
    };

    var smtpUri = new Uri(builder.Configuration.GetConnectionString("maildev")!);
    smtpClient = new SmtpClient(smtpUri.Host, smtpUri.Port);

    await smtpClient.SendMailAsync(message);
});

app.Run();