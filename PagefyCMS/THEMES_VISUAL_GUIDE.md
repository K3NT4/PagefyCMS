# 🎨 Tema-Systemöversikt - Visuell Guide

## Alla 6 Teman på Ett Ögonkast

```
┌─────────────────────────────────────────────────────────────┐
│  PAGEFYCMS TEMA-SYSTEM - 6 MODERNA DESIGNS                  │
└─────────────────────────────────────────────────────────────┘

┌──────────────────────┐  ┌──────────────────────┐
│   🎨 FRAMTIDSDESIGN  │  │ ⚡ NEON CYBERPUNK   │
│  (Standard/Fallback) │  │  Sci-Fi & Tech      │
├──────────────────────┤  ├──────────────────────┤
│ Färg: Blå/Lila       │  │ Färg: Neon Grön     │
│       Gradient       │  │       Hot Pink      │
│ Fil: site.css        │  │ Fil: theme-neon...  │
│ För: Alla typer      │  │ För: Tech/Gaming    │
│ ✨ Modern            │  │ ⚡ Energisk         │
└──────────────────────┘  └──────────────────────┘

┌──────────────────────┐  ┌──────────────────────┐
│ 💼 PROFESSIONAL M.   │  │ 📰 NEWS HUB         │
│  Företags-klassiker  │  │  Nyhetsoptimerad    │
├──────────────────────┤  ├──────────────────────┤
│ Färg: Mörkblå        │  │ Färg: Röd/Orange    │
│ Fil: theme-prof...   │  │ Fil: theme-news...  │
│ För: Företag         │  │ För: Nyhetsidor     │
│ 👔 Pålitlig          │  │ 📰 Läsbar           │
└──────────────────────┘  └──────────────────────┘

┌──────────────────────┐  ┌──────────────────────┐
│ 🎮 GAMING EDGE      │  │ ✨ MINIMALIST CLEAN │
│  RGB & Gaming       │  │  Enkelt & Rent      │
├──────────────────────┤  ├──────────────────────┤
│ Färg: Lila/Cyan     │  │ Färg: Svart/Vit     │
│ Fil: theme-gaming.. │  │ Fil: theme-minimal..│
│ För: Gaming/Esports │  │ För: Portfolios     │
│ 🎮 Energisk         │  │ 📖 Fokuserad        │
└──────────────────────┘  └──────────────────────┘

                    ┌──────────────────────┐
                    │ 🌌 AURORA GRADIENT   │
                    │  Nordljus-inspirerad │
                    ├──────────────────────┤
                    │ Färg: Cyan → Lila    │
                    │ Fil: theme-aurora..  │
                    │ För: Creative/Sci-Fi │
                    │ ✨ Magisk            │
                    └──────────────────────┘
```

---

## Hur Man Aktiverar Ett Tema

```
STEG 1: Gå till Admin
┌──────────────────┐
│  Admin Dashboard │
└────────┬─────────┘
         │
         ▼
┌──────────────────┐
│ ⚙️  Inställningar │
└────────┬─────────┘
         │
         ▼
┌──────────────────────────────────┐
│ 🎨 Välj Tema från Dropdown       │
│                                  │
│ ☐ 🎨 Framtidsdesign             │
│ ☐ ⚡ Neon Cyberpunk              │
│ ☐ 💼 Professional Midnight       │
│ ☐ 📰 News Hub                    │
│ ☒ 🎮 Gaming Edge                 │ ← Valt
│ ☐ ✨ Minimalist Clean           │
│ ☐ 🌌 Aurora Gradient             │
└────────┬──────────────────────────┘
         │
         ▼
┌──────────────────┐
│ 💾 Spara         │
└────────┬─────────┘
         │
         ▼
✅ TEMA AKTIVERAT!
```

---

## Tema-Egenskaper i Snabböversikt

```
FRAMTIDSDESIGN
  🎨 Färger: Blå/Lila Gradient
  🌟 Features: Glassmorph, Blur, Modern
  ✅ Default: Ja
  
NEON CYBERPUNK
  ⚡ Färger: Neon Grön, Cyan, Hot Pink
  🌟 Features: Glow-effekter, Text-shadow
  ✨ Känsla: Sci-Fi, Futuristisk
  
PROFESSIONAL MIDNIGHT
  💼 Färger: Mörkblå, Elektrisk Blå
  🌟 Features: Rent layout, Subtilt
  📊 Perfekt för: Företag, Konsult
  
NEWS HUB
  📰 Färger: Röd, Orange
  🌟 Features: Kategori-badges, Datum
  📑 Perfekt för: Nyhetsväxlar, Bloggar
  
GAMING EDGE
  🎮 Färger: Lila, Cyan, Rosa
  🌟 Features: RGB-style, Pulse-animering
  🏆 Perfekt för: Gamers, Esports
  
MINIMALIST CLEAN
  ✨ Färger: Vit, Svart, Accent-blå
  🌟 Features: Minimal design, Fokus-text
  📖 Perfekt för: Portfolio, Personal-brand
  
AURORA GRADIENT
  🌌 Färger: Cyan, Blå, Lila, Rosa
  🌟 Features: Animerad aurora, Glitter
  ✨ Perfekt för: Creative-agencys, Design
```

---

## CSS-Filstruktur

```
wwwroot/css/
│
├── site.css
│   └─ 🎨 Framtidsdesign (Standard)
│      ├─ --primary-gradient: Blå/Lila
│      ├─ --dark-bg: Mörkblå
│      └─ --text-light: Ljusblå
│
├── theme-neon-cyberpunk.css
│   └─ ⚡ Neon Cyberpunk
│      ├─ --primary-gradient: Grön/Cyan
│      ├─ --dark-bg: Mycket mörkt
│      └─ Effekter: Text-shadow glow
│
├── theme-professional-midnight.css
│   └─ 💼 Professional
│      ├─ --primary-gradient: Blå
│      ├─ --dark-bg: Mörkblå
│      └─ Stil: Klassisk & ren
│
├── theme-news-hub.css
│   └─ 📰 News Hub
│      ├─ --primary-gradient: Röd/Orange
│      ├─ --dark-bg: Mycket mörkt
│      └─ Special: Artikel-styling
│
├── theme-gaming-edge.css
│   └─ 🎮 Gaming Edge
│      ├─ --primary-gradient: Lila/Cyan
│      ├─ --dark-bg: Mycket mörkt
│      └─ Features: RGB, Pulse-anim
│
├── theme-minimalist-clean.css
│   └─ ✨ Minimalist
│      ├─ --primary-gradient: Svart
│      ├─ --dark-bg: Vit
│      └─ Style: Ultra-minimal
│
└── theme-aurora-gradient.css
    └─ 🌌 Aurora Gradient
       ├─ --primary-gradient: Cyan→Lila
       ├─ --dark-bg: Mörk
       └─ Effects: Aurora animation
```

---

## Implementering i Kod

```html
<!-- I _Layout.cshtml & _AdminLayout.cshtml -->

@{
    // Hämta aktivt tema från databas
    var activeTheme = Context.Settings
        .FirstOrDefault(s => s.Key == "ActiveTheme")?.Value ?? "site";
    
    // Bygg tema-filens sökväg
    var themeFile = activeTheme == "site" 
        ? "~/css/site.css" 
        : $"~/css/theme-{activeTheme}.css";
}

<!-- Ladda temat -->
<link rel="stylesheet" href="@themeFile" />
```

```csharp
// I Settings.cshtml.cs

[BindProperty]
public string ActiveTheme { get; set; } = "site";

public IActionResult OnPost()
{
    SaveSetting("ActiveTheme", ActiveTheme);
    return Page();
}
```

---

## Tema-Övergång

```
FÖRE:           EFTER:
┌─────────────┐ ┌─────────────┐
│ site.css    │ │ theme-      │
│             │ │ gaming-     │
│ Blå/Lila    │ │ edge.css    │
│             │ │             │
│             │ │ Lila/Cyan   │
└─────────────┘ └─────────────┘
    Sparad          Sparad
    i DB            i DB
    KeyName:        KeyName:
    "ActiveTheme"   "ActiveTheme"
```

---

## CSS-Variabler (Varje Tema)

```css
/* Alla teman använder samma variabel-struktur */

:root {
    /* Gradienter */
    --primary-gradient: linear-gradient(...);
    --secondary-gradient: linear-gradient(...);
    
    /* Bakgrunder */
    --dark-bg: #...;
    --card-bg: #...;
    --border-color: #...;
    
    /* Text */
    --text-light: #...;
    --text-muted: #...;
    
    /* Accenter */
    --accent-blue: #...;
    --accent-pink: #...;
    
    /* Tillstånd */
    --success-color: #...;
    --warning-color: #...;
    --danger-color: #...;
}
```

Det gör det enkelt att bygga vidare!

---

## Snabbguide: Skapa Eget Tema

```bash
# 1. Kopiera befintligt tema
cp theme-gaming-edge.css theme-mitt-tema.css

# 2. Redigera färger i CSS-variabler
# 3. Spara filen i wwwroot/css/

# 4. Uppdatera Settings.cshtml
<option value="mitt-tema">🎨 Mitt Tema</option>

# 5. Aktivera i admin
# Admin → Inställningar → Tema → "mitt-tema" → Spara
```

---

## 🎉 Du Är Redo!

Alla 6 teman är nu installerade och klara att användas.

**Nästa steg:**
1. Gå till Admin → Inställningar
2. Välj ditt favoritema
3. Spara ändringar
4. Se webbplatsen förändras!

**Happy Theming!** ✨🎨
