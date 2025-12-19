# PagefyCMS Tema-System 🎨

## 6 Moderna Teman

PagefyCMS levereras med 6 olika futuristiska, moderna och professionella teman som du kan välja mellan direkt i admin-panelen.

### 1. **🎨 Framtidsdesign (Standard)**
- **Fil:** `site.css` (StandardLayout)
- **Färger:** Blå/lila gradienter (#667eea → #764ba2)
- **Känsla:** Futuristisk, modern, universell
- **Bäst för:** Alla typer av webbplatser
- **Features:**
  - Glassmorph-design (backdrop blur)
  - Gradient-bakgrund
  - Smooth animations
  - Responsive design

### 2. **⚡ Neon Cyberpunk**
- **Fil:** `theme-neon-cyberpunk.css`
- **Färger:** Neon-grönt, cyan, hot pink (#00ff88, #00ffff, #ff0099)
- **Känsla:** Cyberpunk, futuristisk, energisk
- **Bäst för:** Tech-startups, gaming, innovation-fokuserade webbplatser
- **Features:**
  - Neon-glöd på alla element
  - Höga kontraster
  - Sci-fi atmosfär
  - Animerad bakgrund med färgförskjutning

### 3. **💼 Professional Midnight**
- **Fil:** `theme-professional-midnight.css`
- **Färger:** Mörkblå (#0f172a), elektrisk blå (#3b82f6)
- **Känsla:** Professionell, pålitlig, klassisk
- **Bäst för:** Företag, konsultering, finansiella tjänster
- **Features:**
  - Rent och organiserat
  - Låga kontraster för ögonen
  - Fokus på läsbarhet
  - Konservativ design

### 4. **📰 News Hub**
- **Fil:** `theme-news-hub.css`
- **Färgor:** Mörkrött (#dc2626), orange (#ea580c)
- **Känsla:** Modern nyhetswebbplats, aktivitet, brådska
- **Bäst för:** Nyhetswebbplatser, bloggar, mediasajter
- **Features:**
  - Optimerad typografi för läsning
  - Stort fokus på artikelöversikter
  - Kategori-badges
  - Datum- och författarinformation
  - Snabb scanbarhet

### 5. **🎮 Gaming Edge**
- **Fil:** `theme-gaming-edge.css`
- **Färger:** Lila (#7c3aed), rosa (#ec4899), cyan (#06b6d4)
- **Känsla:** Energisk, modern, gamer-oriented
- **Bäst för:** Gaming-nyheter, esports, streaming-relaterat
- **Features:**
  - Högkontrastiga växtlande knappar
  - Game-badges och pulse-animationer
  - RGB-inspirerad design
  - Dynamiska effekter

### 6. **✨ Minimalist Clean**
- **Fil:** `theme-minimalist-clean.css`
- **Färgor:** Vit, svart, blå (#0066cc)
- **Känsla:** Enkelt, rent, fokus på innehål
- **Bäst för:** Portfolios, personliga sidor, minimal design
- **Features:**
  - Maximal läsbarhet
  - Minimal visuell brus
  - Fokus på typografi
  - Klassisk layout

### 7. **🌌 Aurora Gradient**
- **Fil:** `theme-aurora-gradient.css`
- **Färgor:** Cyan (#00d4ff), blå (#0099ff), lila (#6600ff)
- **Känsla:** Magisk, nordljus-inspirerad, futuristisk
- **Bäst för:** Creative-agencies, design-portals, sci-fi-relaterat
- **Features:**
  - Animerad aurora-effekt i bakgrunden
  - Dynamiska gradienter
  - Glow-effekter på text
  - Interaktiva elementer

---

## Så Använder Du Temana

### Admin-panelen
1. Navigera till **Admin → Inställningar**
2. Bläddra till **Tema**-sektionen
3. Välj ditt favoritema från dropdown
4. Klicka **Spara ändringar**

Temat tillämpas direkt på hela webbplatsen (både front-end och admin).

### Manuell Aktivering
Om du vill aktivera ett tema via databas:
```sql
INSERT INTO CmsSetting (Key, Value) VALUES ('ActiveTheme', 'neon-cyberpunk')
-- Eller uppdatera befintlig:
UPDATE CmsSetting SET Value = 'gaming-edge' WHERE Key = 'ActiveTheme'
```

---

## CSS-Variabel System

Varje tema använder CSS-variabler för enkel anpassning:

```css
:root {
    --primary-gradient: linear-gradient(...);
    --dark-bg: #...;
    --card-bg: #...;
    --border-color: #...;
    --text-light: #...;
    --text-muted: #...;
    --accent-blue: #...;
    --success-color: #...;
    --danger-color: #...;
}
```

---

## Bygga Vidare på Temana

Varje tema är baserat på Bootstrap och site.css, så du kan enkelt:

1. **Kopiera en temafil** - Skapa `theme-mitt-tema.css`
2. **Anpassa färger** - Ändra CSS-variablerna
3. **Lägg till nya stilar** - Utöka med dina egna CSS-regler
4. **Aktivera** - Välj i admin-panelen

### Exempel: Skapa ditt eget tema
```css
/* theme-min-brand.css */
:root {
    --primary-gradient: linear-gradient(135deg, #FF6B6B 0%, #FFE66D 100%);
    --dark-bg: #1a1a1a;
    /* ... resten av variablerna ... */
}

/* Sedan i admin: Välj "min-brand" från dropdown */
```

---

## Tema-Struktur

```
PagefyCMS/
├── wwwroot/css/
│   ├── site.css                    (Framtidsdesign - Standard)
│   ├── theme-neon-cyberpunk.css    (Neon-tema)
│   ├── theme-professional-midnight.css
│   ├── theme-news-hub.css          (Nyhetssida)
│   ├── theme-gaming-edge.css       (Gaming/Esports)
│   ├── theme-minimalist-clean.css  (Minimalist)
│   └── theme-aurora-gradient.css   (Aurora)
├── Pages/Shared/
│   └── _Layout.cshtml              (Laddar tema dynamiskt)
└── Pages/Admin/_AdminLayout.cshtml (Admin-tema)
```

---

## Tekniska Detaljer

### Dynamisk Tema-Loading
Temana laddas dynamiskt från databasen via Razor:
```csharp
// I _Layout.cshtml
@{
    var activeTheme = Context.Settings
        .FirstOrDefault(s => s.Key == "ActiveTheme")?.Value ?? "site";
    var themeFile = activeTheme == "site" 
        ? "~/css/site.css" 
        : $"~/css/theme-{activeTheme}.css";
}
<link rel="stylesheet" href="@themeFile" />
```

### Browser-kompatibilitet
- Alla teman stöder moderna webbläsare
- CSS-gradienter, CSS-variabler, Flexbox
- Fallback-värden för äldre browser

### Performance
- Tema-fil cacheas av browser
- Ingen JavaScript-overhead
- Optimal rendering

---

## Tips för Tema-Design

### Färgval
- Använd CSS-variabler för konsistens
- Minst 4.5:1 kontrast för accessibility
- Test på mobilenheter

### Typografi
- Base font-size: 16px
- Line-height: 1.6 för body-text
- Responsive font-sizing

### Animationer
- Begränsa transitions till 0.3s
- Använd ease-in-out för smooth motion
- Testa performance på slow devices

---

## Support & Anpassning

För att anpassa eller skapa nya teman:
1. Utgå från `site.css` som bas
2. Använd CSS-variabler systemet
3. Testa på alla Bootstrap-breakpoints
4. Validera färgkontraster
5. Verifiera alla komponenter visas korrekt

Happy theming! 🎨✨
