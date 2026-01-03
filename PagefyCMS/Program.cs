using PagefyCMS.Data;
using PagefyCMS.Addons;
using PagefyCMS.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<PagefyDbContext>(options =>
    options.UseSqlite("Data Source=pagefy.db")
        .ConfigureWarnings(w => 
            w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Session timeout
});

// Register addon system
builder.Services.AddSingleton<AddonManager>();
builder.Services.AddScoped<AddonInstaller>();

// Register theme system
builder.Services.AddScoped<IThemeManager, ThemeManager>();

// Register language service
builder.Services.AddScoped<ILanguageService, LanguageService>();

// Register Asset Usage Service
builder.Services.AddScoped<AssetUsageService>();

// Register Update Services
builder.Services.AddSingleton<IVersionService, VersionService>();
builder.Services.AddScoped<IUpdateService, MockUpdateService>();

var app = builder.Build();

// Initialize addon system
var addonManager = app.Services.GetRequiredService<AddonManager>();
await addonManager.LoadAddonsAsync();
await addonManager.InitializeAllAddonsAsync();

// Initialize DB tables (Manual Migration for AssetUsage)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PagefyDbContext>();
    context.Database.Migrate();
    DbInitializer.EnsureAssetUsageTable(context);
}

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
app.MapControllers();

// Health Check Endpoint
app.MapGet("/health", () => Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow }));

app.Run();
