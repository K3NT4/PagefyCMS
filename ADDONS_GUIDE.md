# PagefyCMS Addon System

## 📦 Överblick

PagefyCMS har nu ett komplett **addon/plugin-system** som låter dig utöka funktionaliteten utan att ändra kärnkoden. Addons är modulär, återanvändbar och lätt att utveckla.

---

## 🎯 Grundläggande Koncept

### 1. **IAddon Interface**
Alla addons implementerar `IAddon` och ärver från `BaseAddon`:

```csharp
public class MyAddon : BaseAddon
{
    public override string Id => "com.example.myaddon";
    public override string Name => "Min Addon";
    public override string Description => "Vad addonen gör";
    public override string Version => "1.0.0";
    public override string Author => "Din Namn";

    public override async Task InitializeAsync()
    {
        // Körs när addonen aktiveras
        await base.InitializeAsync();
    }

    public override async Task ShutdownAsync()
    {
        // Körs när addonen deaktiveras
        await base.ShutdownAsync();
    }
}
```

### 2. **Hook System**
Addons kan "koppla in" sig vid specifika eventos i systemet via **hooks**:

```csharp
public class MyHookAddon : HookableAddon
{
    public override IEnumerable<AddonHook> SupportedHooks => new[]
    {
        AddonHook.BeforePageSave,
        AddonHook.AfterPageRender
    };

    public override async Task ExecuteHookAsync(HookContext context)
    {
        if (context.Hook == AddonHook.BeforePageSave)
        {
            // Gör något innan en sida sparas
        }
    }
}
```

### 3. **AddonManager**
Hanterar inladdning, registrering och körning av addons:

```csharp
var addonManager = app.Services.GetRequiredService<AddonManager>();
await addonManager.LoadAddonsAsync();
addonManager.RegisterAddon(new MyAddon());
var addon = addonManager.GetAddon("com.example.myaddon");
```

---

## 🚀 Steg-för-steg: Skapa En Addon

### Steg 1: Skapa addon-klassen

```csharp
using PagefyCMS.Addons;

public class GreeterAddon : HookableAddon
{
    public override string Id => "com.example.greeter";
    public override string Name => "Greeter";
    public override string Description => "Hälsar användare på sidorna";
    public override string Version => "1.0.0";
    public override string Author => "Min Namn";

    public override IEnumerable<AddonHook> SupportedHooks => new[]
    {
        AddonHook.BeforeHomepageRender
    };

    public override async Task ExecuteHookAsync(HookContext context)
    {
        if (context.Hook == AddonHook.BeforeHomepageRender)
        {
            // Lägg till data som sedan visas på sidan
            context.Data["Greeting"] = "Välkommen till PagefyCMS!";
        }
        await Task.CompletedTask;
    }
}
```

### Steg 2: Registrera addonen

I `Program.cs`, efter `app.Build()`:

```csharp
var addonManager = app.Services.GetRequiredService<AddonManager>();
addonManager.RegisterAddon(new GreeterAddon());
```

### Steg 3: Använd addonen i dina sidor

```csharp
// I en PageModel
public class IndexModel : PageModel
{
    private readonly AddonManager _addonManager;

    public IndexModel(AddonManager addonManager)
    {
        _addonManager = addonManager;
    }

    public async Task OnGet()
    {
        var context = new HookContext { Hook = AddonHook.BeforeHomepageRender };
        await _addonManager.ExecuteHookAsync(context);
        
        // context.Data["Greeting"] innehåller nu "Välkommen till PagefyCMS!"
    }
}
```

---

## 📌 Tillgängliga Hooks

| Hook | Beskrivning | Data Tillgängligt |
|------|-------------|------------------|
| `BeforeHomepageRender` | Innan startsida renderas | Page content |
| `AfterHomepageRender` | Efter startsida renderas | Rendered HTML |
| `BeforePageRender` | Innan sida renderas | Page content |
| `AfterPageRender` | Efter sida renderas | Rendered HTML |
| `BeforeArticleRender` | Innan artikel renderas | Article content |
| `AfterArticleRender` | Efter artikel renderas | Rendered HTML |
| `BeforeArticleSave` | Innan artikel sparas i DB | Article model |
| `BeforePageSave` | Innan sida sparas i DB | Page model |
| `BeforeMediaSave` | Innan media sparas i DB | Media item |
| `BeforeMediaDelete` | Innan media tas bort | Media ID |
| `AdminMenuItems` | Lägg till meny-items | Admin menu |
| `SystemInitialize` | Vid systemstart | System info |

---

## 🎨 Exempel-Addons

### SEO Addon
```csharp
public class SeoAddon : HookableAddon
{
    public override string Id => "com.pagefy.seo";
    public override string Name => "SEO Optimizer";
    public override IEnumerable<AddonHook> SupportedHooks => new[] 
    { 
        AddonHook.BeforePageRender,
        AddonHook.BeforeArticleRender 
    };

    public override async Task ExecuteHookAsync(HookContext context)
    {
        // Lägg till automatiska meta-tags
        context.Data["MetaDescription"] = GenerateDescription();
        context.Data["MetaKeywords"] = GenerateKeywords();
        await Task.CompletedTask;
    }
}
```

### Analytics Addon
```csharp
public class AnalyticsAddon : HookableAddon
{
    public override string Id => "com.example.analytics";
    public override string Name => "Analytics Tracker";
    public override IEnumerable<AddonHook> SupportedHooks => new[] 
    { 
        AddonHook.AfterPageRender,
        AddonHook.AfterArticleRender 
    };

    public override async Task ExecuteHookAsync(HookContext context)
    {
        // Lägg till tracking-kod
        context.Data["TrackingCode"] = "<script>...</script>";
        await Task.CompletedTask;
    }
}
```

### Cache Addon
```csharp
public class CacheAddon : HookableAddon
{
    private Dictionary<string, object> _cache = new();

    public override IEnumerable<AddonHook> SupportedHooks => new[] 
    { 
        AddonHook.BeforePageRender,
        AddonHook.BeforeArticleRender 
    };

    public override async Task ExecuteHookAsync(HookContext context)
    {
        var key = GenerateCacheKey(context);
        if (_cache.TryGetValue(key, out var cached))
        {
            context.Data["Cached"] = cached;
        }
        await Task.CompletedTask;
    }
}
```

---

## 🔧 Avancerade Funktioner

### Addon-Konfiguration

Skapa en `addon.json` i addon-mappen:

```json
{
  "id": "com.example.myaddon",
  "name": "Min Addon",
  "enabled": true,
  "settings": {
    "option1": "värde1",
    "option2": "värde2"
  }
}
```

Läs konfiguration i din addon:

```csharp
var config = await File.ReadAllTextAsync("addon.json");
var options = JsonConvert.DeserializeObject<AddonConfig>(config);
```

### Addon-Beroenden

Om en addon behöver en annan addon:

```csharp
public class DependentAddon : BaseAddon
{
    public override async Task InitializeAsync()
    {
        var requiredAddon = _addonManager.GetAddon("com.example.required");
        if (requiredAddon == null)
            throw new Exception("Krävs com.example.required addon");
        
        await base.InitializeAsync();
    }
}
```

### Datadelning Mellan Addons

```csharp
public class SharedDataAddon : BaseAddon
{
    public static Dictionary<string, object> SharedData = new();

    public override async Task InitializeAsync()
    {
        SharedData["key"] = "value";
        await base.InitializeAsync();
    }
}
```

---

## 📊 Admin-Interface

Gå till `/Admin/Settings/Addons` för att:
- ✅ Se alla installerade addons
- ✅ Aktivera/deaktivera addons
- ✅ Se addon-information
- ✅ Läsa dokumentation

---

## 🔐 Säkerhet

### Addon Validering
```csharp
public abstract class ValidatedAddon : BaseAddon
{
    public override async Task InitializeAsync()
    {
        if (!ValidateAddon())
            throw new UnauthorizedAccessException("Addon validering misslyckades");
        await base.InitializeAsync();
    }

    protected virtual bool ValidateAddon() => true;
}
```

### Begränsad Åtkomst
Addons bör inte ha tillgång till känslig data utan autentisering:

```csharp
public override async Task ExecuteHookAsync(HookContext context)
{
    if (string.IsNullOrEmpty(context.UserId))
        return; // Ignorera om ingen användare är inloggad

    // Fortsätt med autentiserad logik
    await Task.CompletedTask;
}
```

---

## 🧪 Testa Din Addon

```csharp
[TestClass]
public class MyAddonTests
{
    [TestMethod]
    public async Task TestAddonInitialization()
    {
        var addon = new MyAddon();
        await addon.InitializeAsync();
        
        Assert.IsTrue(addon.IsEnabled);
    }

    [TestMethod]
    public async Task TestHookExecution()
    {
        var addon = new MyAddon();
        var context = new HookContext { Hook = AddonHook.BeforePageSave };
        
        await addon.ExecuteHookAsync(context);
        
        Assert.IsTrue(context.Data.ContainsKey("ExpectedKey"));
    }
}
```

---

## 📦 Distribuera Din Addon

1. Skapa en mapp `/Addons/MyAddon/`
2. Lägg din addon-kod där
3. Lägg till `addon.json` med metadata
4. Zipar mappen
5. Distribuera via PagefyCMS Addon Marketplace (framtida feature)

---

## ⚠️ Bästa Metoder

1. **Namn addons unikt** - Använd omvänd domain-notation: `com.yourcompany.featurename`
2. **Dokumentera hookar** - Berätta vilka hooks addonen använder
3. **Hantera fel** - Använd try-catch i ExecuteHookAsync
4. **Logga aktiviteter** - Använd AddressLogging för debugging
5. **Versionshantering** - Följa semver (1.0.0)
6. **Konfigurering** - Låt addons konfigureras via appsettings.json
7. **Testning** - Skriv unit-tests för addons
8. **Performance** - Optimera hook-exekvering

---

## 🚫 Vanliga Misstag

❌ **Inte hantera exceptions** - Kan krascha systemet
❌ **Lång-körande operationer i hooks** - Blockerar rendering
❌ **Hårdkodade värden** - Använd configuration istället
❌ **Ingen logging** - Svårt att debugga
❌ **Ignorera security** - Validera alltid användarinput

---

## 📞 Support & Resources

- 📚 [Addon Dokumentation](../ADDONS.md)
- 🔗 [Interface References](../Addons/)
- 💬 [GitHub Discussions](https://github.com/yourrepo/discussions)
