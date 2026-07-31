using Microsoft.EntityFrameworkCore;

namespace ODataTestService.Models;

public class MainContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    public MainContext(DbContextOptions<MainContext> options) : base(options)
    {
    }
}