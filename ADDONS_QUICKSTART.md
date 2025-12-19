# PagefyCMS Addon System - Snabbstart

## 🎯 Vad Är ett Addon-System?

Ett addon-system låter dig:
- ✅ Lägga till nya funktioner utan att ändra kärnkoden
- ✅ Aktivera/deaktivera features dinamiskt
- ✅ Dela addons mellan CMS-instanser
- ✅ Utveckla addons oberoende

---

## ⚡ Snabb Introduktion

### 1️⃣ **Förstå Strukturen**

```
PagefyCMS/
├── Addons/                    ← Interface & Manager
│   ├── IAddon.cs             ← Bas-interface
│   ├── IHookableAddon.cs     ← Hook-system
│   ├── AddonManager.cs       ← Hantering
│   └── BaseAddon.cs          ← Basklass
├── ExampleAddons/             ← Exempel-addons
│   ├── SeoAddon.cs
│   └── ActivityLogAddon.cs
└── Pages/Admin/Settings/
    └── Addons.cshtml          ← Admin-interface
```

### 2️⃣ **Skapa Din Första Addon** (2 minuter)

Skapa fil: `PagefyCMS/ExampleAddons/HelloWorldAddon.cs`

```csharp
using PagefyCMS.Addons;

public class HelloWorldAddon : BaseAddon
{
    public override string Id => "com.example.helloworld";
    public override string Name => "Hello World";
    public override string Description => "En enkel hälsnings-addon";
    public override string Version => "1.0.0";
    public override string Author => "Din Namn";

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        System.Diagnostics.Debug.WriteLine("✨ Hello World addon startad!");
    }
}
```

Registrera i `Program.cs`:

```csharp
var addonManager = app.Services.GetRequiredService<AddonManager>();
addonManager.RegisterAddon(new HelloWorldAddon());
```

**Resultat:** ✅ Din första addon är live!

### 3️⃣ **Med Hooks** (5 minuter)

```csharp
using PagefyCMS.Addons;

public class MyHookAddon : HookableAddon
{
    public override string Id => "com.example.hooks";
    public override string Name => "Hook Example";
    public override string Description => "Visar hur hooks fungerar";
    public override string Version => "1.0.0";
    public override string Author => "Din Namn";

    // Säg vilka hooks denna addon vill lyssna på
    public override IEnumerable<AddonHook> SupportedHooks => new[]
    {
        AddonHook.BeforePageSave,      // Innan sida sparas
        AddonHook.AfterPageRender      // Efter sida visas
    };

    // Denna körs automatiskt när någon av hooksen utlöses
    public override async Task ExecuteHookAsync(HookContext context)
    {
        switch (context.Hook)
        {
            case AddonHook.BeforePageSave:
                System.Diagnostics.Debug.WriteLine("📝 Sida sparas...");
                break;

            case AddonHook.AfterPageRender:
                System.Diagnostics.Debug.WriteLine("👀 Sida visades!");
                break;
        }
        
        await Task.CompletedTask;
    }
}
```

### 4️⃣ **Admin-Panel** (1 minut)

Besök: `http://localhost:5000/Admin/Settings/Addons`

Där kan du:
- 👁️ Se alla addons
- ✅ Aktivera/deaktivera
- 📖 Läsa dokumentation

---

## 📚 Hook-Guider

### Hook: `BeforePageSave` - Validera innan sparning

```csharp
public override async Task ExecuteHookAsync(HookContext context)
{
    if (context.Hook == AddonHook.BeforePageSave)
    {
        if (context.Data.TryGetValue("Page", out var page))
        {
            // Validera sida här
            System.Diagnostics.Debug.WriteLine("🔍 Validerar sida...");
        }
    }
    await Task.CompletedTask;
}
```

### Hook: `BeforeArticleRender` - Lägg till innehål

```csharp
public override async Task ExecuteHookAsync(HookContext context)
{
    if (context.Hook == AddonHook.BeforeArticleRender)
    {
        // Lägg till extra data som ska visas
        context.Data["CustomContent"] = "<div>Min Custom HTML</div>";
    }
    await Task.CompletedTask;
}
```

### Hook: `BeforeMediaSave` - Bearbeta filer

```csharp
public override async Task ExecuteHookAsync(HookContext context)
{
    if (context.Hook == AddonHook.BeforeMediaSave)
    {
        if (context.Data.TryGetValue("MediaItem", out var media))
        {
            // Bearbeta mediafil här
            System.Diagnostics.Debug.WriteLine("🖼️ Bearbetar bild...");
        }
    }
    await Task.CompletedTask;
}
```

---

## 🎯 Praktiska Addon-Idéer

### 1. SEO Optimizer
```
Gör:
- Lägg till automatiska meta-tags
- Valida URL-slugs
- Skapa sitemap.xml
```

### 2. Anti-Spam
```
Gör:
- Filtrera kommenter
- Validera formulär
- Blockera IP:ar
```

### 3. Backup Manager
```
Gör:
- Skapa dagliga backups
- Spara till cloud
- Versionshantering
```

### 4. Email Notifier
```
Gör:
- Skicka email vid ändringar
- Notify på felanmälningar
- Dagliga rapporter
```

### 5. Performance Monitor
```
Gör:
- Logga pageload-tid
- Monitora databas-queries
- Visa statistik i admin
```

---

## 🚀 Nästa Steg

1. **Läs Full Guide**: `ADDONS_GUIDE.md`
2. **Studera Exempel**: `ExampleAddons/`
3. **Skapa Din Addon**: Följ stegen ovan
4. **Testa Det**: Besök Admin-panelen
5. **Dela Det**: Distribuera till andra!

---

## 💡 Tips & Tricks

**Tip 1: Debug addons**
```csharp
System.Diagnostics.Debug.WriteLine($"🐛 DEBUG: {message}");
```

**Tip 2: Logga aktiviteter**
```csharp
// Lägg till detta i din addon
private readonly ILogger<MyAddon> _logger;

_logger.LogInformation("Addon körs...");
```

**Tip 3: Async-operationer**
```csharp
public override async Task ExecuteHookAsync(HookContext context)
{
    // Gör långsam operation utan att blockera
    await Task.Delay(1000);
    
    // Spara till databas
    // Gör API-calls
    // etc...
    
    await Task.CompletedTask;
}
```

---

## ⚠️ Viktigt!

❌ **Don't:**
- Kasta exceptions utan try-catch
- Gör långsamma operationer direkt (använd Task.Run)
- Modifiera globalt state utan locks
- Ignorera security

✅ **Do:**
- Hantera errors gracefully
- Logga vad som händer
- Dokumentera din addon
- Skriv enhetstester

---

## 📞 Behöver Du Hjälp?

- 📖 Läs: `ADDONS_GUIDE.md` (Komplett referens)
- 💬 Exempel-addons: `ExampleAddons/`
- 🔧 Admin-panel: `/Admin/Settings/Addons`

**Lycka Till! 🚀**
