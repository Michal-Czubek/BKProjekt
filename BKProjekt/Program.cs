using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BKProjekt.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BKProjektDbContextConnection") ?? throw new InvalidOperationException("Connection string 'BKProjektDbContextConnection' not found.");

builder.Services.AddDbContext<BKProjektDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<BKProjektUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BKProjektDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "user", "admin", "keeper" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager =
        scope.ServiceProvider.GetRequiredService<UserManager<BKProjektUser>>();

    string AdmEmail = "Admin@wp.pl";
    string AdmPassword = "H@slo123";
    string AdmFirstname = "Admin";
    string AdmLastname = "Admin";

    if (await userManager.FindByEmailAsync(AdmEmail) == null)
    {
        var user = new BKProjektUser();
        user.UserName = AdmEmail;
        user.Email = AdmEmail;
        user.FirstName = AdmFirstname;
        user.LastName = AdmLastname;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, AdmPassword);

        await userManager.AddToRoleAsync(user, "admin");
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager =
        scope.ServiceProvider.GetRequiredService<UserManager<BKProjektUser>>();

    string KeeperEmail = "Keeper@wp.pl";
    string KeeperPassword = "H@slo123";
    string KeeperFirstname = "Keeper";
    string KeeperLastname = "Keeper";

    if (await userManager.FindByEmailAsync(KeeperEmail) == null)
    {
        var user = new BKProjektUser();
        user.UserName = KeeperEmail;
        user.Email = KeeperEmail;
        user.FirstName = KeeperFirstname;
        user.LastName = KeeperLastname;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, KeeperPassword);

        await userManager.AddToRoleAsync(user, "keeper");
    }
}

app.Run();
