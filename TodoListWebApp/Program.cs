using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using TodoListWebApp.Areas.Identity.Data;
using TodoListWebApp.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<TodoListDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<TodoListUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TodoListDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    var roles = new[] { "Admin", "User" };

//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new IdentityRole(role));
//        }
//    }
//}

//using (var scope = app.Services.CreateScope()) 
//{
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TodoListUser>>();

//    string firstName = "admin";
//    string lastName = "1";
//    string email = "admin1@example.com";
//    string password = "Admin@123";

//    if (await userManager.FindByEmailAsync(email) == null)
//    {
//        var user = new TodoListUser();
//        user.FirstName = firstName;
//        user.LastName = lastName;
//        user.UserName = email;
//        user.Email = email;

//        await userManager.CreateAsync(user, password);

//        await userManager.AddToRoleAsync(user, "Admin");
//    }
//}

//using (var scope = app.Services.CreateScope())
//{
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TodoListUser>>();

//    string firstName = "user";
//    string lastName = "1";
//    string email = "user@example.com";
//    string password = "User@123";

//    if (await userManager.FindByEmailAsync(email) == null)
//    {
//        var user = new TodoListUser();
//        user.FirstName = firstName;
//        user.LastName = lastName;
//        user.UserName = email;
//        user.Email = email;

//        await userManager.CreateAsync(user, password);

//        await userManager.AddToRoleAsync(user, "User");
//    }
//}

app.Run();
