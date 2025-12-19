using PagefyCMS.Data;
using PagefyCMS.Addons;
using PagefyCMS.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<PagefyDbContext>(options =>
    options.UseSqlite("Data Source=pagefy.db"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Tiden man är inloggad
});

// Registrera addon-system
builder.Services.AddSingleton<AddonManager>();
builder.Services.AddScoped<AddonInstaller>();

// Registrera tema-system
builder.Services.AddScoped<IThemeManager, ThemeManager>();

var app = builder.Build();

// Initalisera addon-systemet
var addonManager = app.Services.GetRequiredService<AddonManager>();
await addonManager.LoadAddonsAsync();
await addonManager.InitializeAllAddonsAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
