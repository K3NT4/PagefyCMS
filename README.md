
# Pagefy CMS

Pagefy CMS är ett lättviktigt, flexibelt innehållshanteringssystem utvecklat för maximal designfrihet, prestanda och enkelhet. Perfekt för nyhetssidor, artiklar, landningssidor och andra webbprojekt där du själv vill ha full kontroll utan att fastna i tunga mallar eller begränsade byggare.

Byggt med fokus på enkel kodstruktur, utbyggnadsmöjligheter och modern webbteknik.

---

## ✨ Funktioner

✅ Sidbyggare med full layoutkontroll  
✅ Plugin-system för utbyggnad och anpassningar  
✅ Mediabibliotek med WebP-konvertering och originalhantering  
✅ Automatisk skalning till olika bildstorlekar (small, medium, large)  
✅ SEO-stöd via namn och alt-taggar på bilder  
✅ Optimerad för prestanda och modern webbstandard  
✅ Tydlig mappstruktur för enkel vidareutveckling  
✅ Öppen källkod - anpassa och vidareutveckla efter dina behov  

---

## � Projektstruktur

```text
controller/                    Kontroller för sidhantering, artiklar och media
data/                          Konfiguration, databashantering och inställningar
models/                        Datamodeller (sidor, bilder, artiklar)
pages/                         Framsidan och publika sidor
pages/admin                    Administrationspanel
pages/admin/articles           Hantering av artiklar och nyheter
pages/admin/media              Mediabibliotek för uppladdning och bildhantering
pages/admin/pages              Skapande och redigering av sidor
pages/admin/settings           Inställningar för CMS och webbplats
pages/shared                   Återanvändbara komponenter och mallar
pages/shared/components        Mindre UI-komponenter (knappar, fält etc.)
pages/viewpage                 Dynamisk sidvisning för användarskapta sidor

uploads/                       Uppladdade filer och bilder
uploads/orginals               Originalbilder (för backup eller radering)
uploads/webp                   Optimerade WebP-bilder
uploads/webp/large             Stora bilder för desktop
uploads/webp/medium            Medium-bilder för tablet
uploads/webp/small             Små bilder för mobil

README.md                      Dokumentation för projektet
.gitignore                     Lista över filer och mappar som ignoreras av Git
LICENSE                        Licensfil (MIT)
```

---

## 🚀 Kom igång

1. Klona projektet:
   ```bash
   git clone https://github.com/dittkonto/pagefy-cms.git
   cd pagefy-cms
   ```

2. Installera beroenden (om tillämpligt, ex. vid Node-projekt):
   ```bash
   npm install
   ```

3. Starta utvecklingsservern:
   ```bash
   npm run dev
   ```

4. Öppna i webbläsaren:
   ```
   http://localhost:3000
   ```

---

## 🖼️ Mediabibliotek

- Bilder konverteras automatiskt till WebP vid uppladdning  
- Originalbilder sparas separat för manuell borttagning eller backup  
- Automatisk skalning i tre nivåer (small, medium, large)  
- SEO-vänliga alt-taggar och bildnamn kan sättas direkt i gränssnittet  
- Endast WebP används på webbplatsen för optimal prestanda  

---

## �️ Framtida Utveckling

- Fler färdiga plugins (t.ex. kontaktformulär, gästbok)  
- Möjlighet att importera bilder direkt från Unsplash/Pexels  
- Mobilanpassat admin-gränssnitt  
- Avancerad rättighetshantering för olika användare  
- Fler sidmallar och startteman  

---

## ❤️ Bidra

Pull Requests, buggrapporter och förbättringsförslag är varmt välkomna!  
Du kan även bidra genom att rapportera buggar eller komma med idéer.

---

## 📄 Licens

[MIT License](LICENSE)

---

## 🌐 Länk

Officiell sida: [https://spelhubben.se](https://spelhubben.se)
