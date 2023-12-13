using Ecom.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECOM.Services.EmailAPI.Data;

public class AppDBContext :DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {

    }
   

    public DbSet<EmailLogger> EmailLoggers { get; set; }
}

