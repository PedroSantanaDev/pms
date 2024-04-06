using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pms.app.Components;
using pms.app.Components.Account;
using pms.app.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register Razor Components
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
builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
}).AddCookie(IdentityConstants.ApplicationScheme, options =>
   {
       options.LoginPath = "/Account/Login";
       options.AccessDeniedPath = "/Account/AccessDenied";
   }).AddCookie("Identity.External", options =>
{
    // Configure options for external sign-in cookie
    options.Cookie.Name = "Identity.External";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Adjust this according to your security requirements
});

// Add authorization services
builder.Services.AddAuthorization();

// Get database connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add DbContext and configure it to use SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Add developer exception page for database errors
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add ApplicationUser class and configure Identity services
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// Add custom email sender
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

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
app.UseAntiforgery();

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