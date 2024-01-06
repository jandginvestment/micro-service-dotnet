using Ecom.EmailAPI.Models;
using ECOM.Services.EmailAPI.Data;
using ECOM.Services.EmailAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Ecom.EmailAPI.Services;

public class EmailService : IEmailService
{
    private DbContextOptions<AppDBContext> _dbOptions;

    public EmailService(DbContextOptions<AppDBContext> dbOptions)
    {
        _dbOptions = dbOptions;
    }

    public async Task EmailCartAndLog(ShoppingCartDTO shoppingCart)
    {
        StringBuilder message = new StringBuilder();

        message.AppendLine("<br/> <h5> Cart Email Requested </h5>");
        message.AppendLine($"<br/> <b> Total:   </b>    {shoppingCart.CartHeader.CartTotal}");
        message.AppendLine("<br/><ul>");
        foreach (var detail in shoppingCart?.CartDetails)
        {
            message.AppendLine("<li>");
            message.Append($"<b> item Name:    </b>   {detail.Product?.Name},  <b> count: </b> {detail.Count}");
            message.AppendLine("</li>");
        }
        message.AppendLine("</ul>");
        await LogAndEmail(message.ToString(), shoppingCart.CartHeader.Email);
    }


    public async Task EmailRegisteredUserAndLog(string email)
    {

        await LogAndEmail("new user registered:", email: email);
    }

    private async Task<bool> LogAndEmail(string message, string email)
    {
        try
        {
            EmailLogger emailLog = new() { Email = email, EmailSentOn = DateTime.Now, Message = message };

            await using var _db = new AppDBContext(_dbOptions);
            await _db.EmailLoggers.AddAsync(emailLog);
            await _db.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {

            return false;

        }
    }
}
