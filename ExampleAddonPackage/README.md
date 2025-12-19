# Example Packaged Addon

En enkel exempel-addon som visar hur man paketerar och installerar addons i PagefyCMS.

## Installation

1. Zippa denna mapp: `zip -r example-addon.zip .`
2. Gå till `/Admin/Settings/InstallAddon`
3. Ladda upp example-addon.zip
4. Addonen installeras automatiskt

## Struktur

```
addon.json              - Metadata för addonen
ExamplePackagedAddon.cs - Addon-implementering
README.md              - Denna fil
```

## Innehållsfiler

Du kan inkludera:
- `*.cs` - C# kod
- `*.cshtml` - Razor views
- `*.css` - Stylesheets
- `*.js` - JavaScript
- `*.json` - Konfigurationsfiler
- Vilken fil som helst

## För utvecklare

Se `ADDONS_GUIDE.md` för komplett dokumentation.
