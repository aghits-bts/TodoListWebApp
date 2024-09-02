using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoListWebApp.Areas.Identity.Data;
using TodoListWebApp.Models;

namespace TodoListWebApp.Data;

public class TodoListDbContext : IdentityDbContext<TodoListUser>
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options)
    {
    }

    public DbSet<TodoModel> TodoModel { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
