# 🎨 PagefyCMS Tema-Systemöversikt

## Snabb Guide - 6 Moderna Teman

| Tema | Fil | Färger | Bäst för |
|------|-----|--------|----------|
| **🎨 Framtidsdesign** | site.css | Blå/Lila | Alla webbplatser |
| **⚡ Neon Cyberpunk** | theme-neon-cyberpunk.css | Neon Grön/Cyan | Tech, Gaming |
| **💼 Professional** | theme-professional-midnight.css | Mörkblå | Företag, Konsult |
| **📰 News Hub** | theme-news-hub.css | Röd/Orange | Nyheter, Bloggar |
| **🎮 Gaming Edge** | theme-gaming-edge.css | Lila/Cyan | Gaming, Esports |
| **✨ Minimalist** | theme-minimalist-clean.css | Svart/Vit | Portfolio |
| **🌌 Aurora** | theme-aurora-gradient.css | Cyan/Lila | Creative, Sci-Fi |

---

## Aktivera Tema

### Via Admin-Panelen (Enklast)
1. **Admin → Inställningar → Tema**
2. Välj tema från dropdown
3. Spara ändringar

### Via Databas
```sql
UPDATE CmsSetting SET Value = 'gaming-edge' WHERE Key = 'ActiveTheme'
```

---

## Temat Laddar Från

**Public:** `Pages/Shared/_Layout.cshtml`
**Admin:** `Pages/Admin/_AdminLayout.cshtml`

Båda laddar tema dynamiskt:
```html
@{
    var theme = Context.Settings
        .FirstOrDefault(s => s.Key == "ActiveTheme")?.Value ?? "site";
}
<link rel="stylesheet" href="~/css/theme-@theme.css" />
```

---

## Filstruktur

```
wwwroot/css/
├── site.css                      ← Standard framtidsdesign
├── theme-neon-cyberpunk.css      ← Neon
├── theme-professional-midnight.css ← Klassisk professionell
├── theme-news-hub.css            ← Nyhetsoptimerad
├── theme-gaming-edge.css         ← Gaming-fokuserad
├── theme-minimalist-clean.css    ← Minimalist
└── theme-aurora-gradient.css     ← Nordljus-inspirerad
```

---

## Tema-Egenskaper

### Framtidsdesign (site.css)
- ✨ Glassmorph & blur-effekter
- 🌈 Gradient-bakgrund
- ⚙️ Moderna animationer
- 📱 Fullständigt responsiv

### Neon Cyberpunk
- ⚡ Neon-glöd på allt
- 🔆 Höga kontraster
- 🎮 Sci-fi-känsla
- 💫 Färganimationer

### Professional Midnight
- 📊 Företags-stil
- 🎯 Fokus på läsbarhet
- 💼 Klassisk & pålitlig
- 👔 Minimala effekter

### News Hub
- 📰 Artikel-fokuserad
- 📅 Datum & tid prominent
- 🏷️ Kategori-badges
- 👁️ Snabb scanbarhet

### Gaming Edge
- 🎮 Gamer-optimerad
- 🎨 RGB-inspirerad
- ⚡ Dynamiska effekter
- 🏆 Esports-känsla

### Minimalist Clean
- ⚪ Vit bakgrund
- 📝 Fokus på text
- 🎯 Noll visuellt brus
- 📖 Maximal läsbarhet

### Aurora Gradient
- 🌌 Nordljus-effekt
- ✨ Magisk känsla
- 💫 Glow & glitter
- 🎆 Animerad bakgrund

---

## Skapa Eget Tema

1. **Kopiera existerande tema:**
   ```bash
   cp theme-professional-midnight.css theme-mitt-tema.css
   ```

2. **Uppdatera CSS-variabler:**
   ```css
   :root {
       --primary-gradient: linear-gradient(135deg, #YOUR_COLOR1, #YOUR_COLOR2);
       --dark-bg: #YOUR_DARK_COLOR;
       /* ... resten av variablerna ... */
   }
   ```

3. **Lägg in i admin:**
   - Värde i dropdown: `mitt-tema`
   - Motsvarande fil: `theme-mitt-tema.css`

4. **Aktivera:**
   - Admin → Inställningar → Tema → "mitt-tema"

---

## CSS-Variable System

```css
:root {
    --primary-gradient: /* Primär färg */
    --secondary-gradient: /* Sekundär färg */
    --dark-bg: /* Bakgrund */
    --card-bg: /* Kort/panel */
    --border-color: /* Border */
    --text-light: /* Huvudtext */
    --text-muted: /* Svag text */
    --accent-blue: /* Accent */
    --success-color: /* Grönt */
    --warning-color: /* Orange */
    --danger-color: /* Rött */
}
```

---

## Bra Att Veta

✅ **Tema-switching är omedelbar** - Cache-reset rekommenderas för gamla browsers
✅ **Alla teman är mobilvänliga** - Bootstrap + responsive CSS
✅ **Admin använder samma tema** - Konsistent upplevelse överallt
✅ **Nedgångskompatibilitet** - Fallback till "site" om tema saknas
✅ **CSS-variabler gör det enkelt** - Anpassa färger utan att ändra HTML

---

## Exempel: Tema-aktivering via kod

```csharp
// I C#-kod
var themeSetting = context.Settings.FirstOrDefault(s => s.Key == "ActiveTheme");
if (themeSetting == null)
    context.Settings.Add(new CmsSetting { Key = "ActiveTheme", Value = "gaming-edge" });
else
    themeSetting.Value = "gaming-edge";
context.SaveChanges();
```

---

## Vanliga Frågor

**F: Kan jag använda flera teman samtidigt?**
A: Nej, bara ett tema åt gången. Använd CSS-override för små ändringar.

**F: Kommer temat att sparas om jag uppdaterar?**
A: Ja! Det lagras i databasen under CmsSetting.

**F: Kan jag mixa färger från olika teman?**
A: Ja! Skapa en ny temafil med CSS-variabler från flera teman.

**F: Hur testar jag mitt eget tema?**
A: Lägg tema-filen i wwwroot/css/, uppdatera dropdown, aktivera i admin.

---

**📚 För mer information, se:** `THEMES_GUIDE.md`
