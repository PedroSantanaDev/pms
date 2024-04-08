using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pms.app.Components;
using pms.app.Components.Account;
using pms.app.Data;
using pms.app.Models;
using pms.app.Repository;
using pms.app.Seed;
using pms.app.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Add authentication state provider for Blazor components
builder.Services.AddCascadingAuthenticationState();

// Add IdentityUserAccessor service
builder.Services.AddScoped<IdentityUserAccessor>();

// Add IdentityRedirectManager service
builder.Services.AddScoped<IdentityRedirectManager>();

// Add custom authentication state provider
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Get database connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add DbContext and configure it to use SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddQuickGridEntityFrameworkAdapter(); ;

// Add developer exception page for database errors
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add custom email sender
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// Register with the DI container
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRepository<Item>, Repository<Item>>();
builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
builder.Services.AddScoped<IRepository<Customer>, Repository<Customer>>();
builder.Services.AddScoped<IRepository<CustomerItem>, Repository<CustomerItem>>();

var app = builder.Build();

/*
 * Seeding the database.
 * This is so when a user runs the project for the first time they have some test data
 */
using (var scope = app.Services.CreateScope())
{
    // Seed Roles
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedDefaultRolesAsync(roleManager);

    // Seed Users
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    await UserSeeder.SeedUsersAndRolesAsync(userManager, roleManager);

    // Seed Categories
    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    await CategorySeeder.SeedCategoriesAsync(unitOfWork);

    // Seed Customers
    await CustomerSeeder.SeedCustomersAsync(unitOfWork);

    // Seed Items
    await ItemSeeder.SeedItemsAsync(unitOfWork);

    // Seed customer items
    await CustomerItemSeeder.SeedCustomerItemsAsync(unitOfWork);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Map Blazor components and enable interactive server render mode
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components
app.MapAdditionalIdentityEndpoints();

// Enable authentication and authorization middleware
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Add anti-forgery middleware here
app.UseAntiforgery();


app.Run();