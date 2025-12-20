# PagefyCMS - Quick Start Guide

**Get up and running with PagefyCMS in 10 minutes!**

---

## What is PagefyCMS?

A lightweight, modern content management system built with ASP.NET Core 8.0. Create pages, articles, manage media, switch themes, and manage translations - all with an intuitive admin panel.

**Features:**
- 📄 Pages and Articles management
- 🎨 7 professional themes with easy switching
- 🖼️ Media library with automatic WebP optimization
- 🔌 Addon system for extensibility
- 🌐 Language management system
- 📱 Fully responsive design
- 🎯 Professional dark theme

---

## 🚀 Installation (5 minutes)

### Prerequisites
- .NET 8.0 SDK or later
- Git
- A code editor (VS Code, Visual Studio, etc.)

### Get Started

```bash
# 1. Clone the repository
git clone https://github.com/K3NT4/PagefyCMS.git
cd PagefyCMS/PagefyCMS

# 2. Restore packages
dotnet restore

# 3. Build the project
dotnet build

# 4. Run the application
dotnet run
```

**App is ready at:** `http://localhost:5256`

---

## 📖 First Steps (5 minutes)

### 1. Login to Admin Panel

Go to: `http://localhost:5256/Admin/Dashboard`

The admin interface is fully in English with a modern dark theme.

### 2. Create Your First Page

1. **Admin → Pages → Create New Page**
2. Enter title: "My First Page"
3. Add content in the editor
4. Click "Save"
5. Visit: `http://localhost:5256/my-first-page`

### 3. Create an Article

1. **Admin → Articles → Create New Article**
2. Enter headline: "My First Article"
3. Write content
4. Click "Publish"
5. View articles at: `http://localhost:5256`

### 4. Upload an Image

1. **Admin → Media → Upload**
2. Drag or select an image
3. Add title and alt text
4. Click upload
5. Image is auto-optimized to WebP format

### 5. Switch Theme

1. **Admin → Settings**
2. Look for "Theme" section
3. Select from 7 available themes
4. Click "Save Changes"
5. Visit homepage to see new theme

### 6. Manage Languages

1. **Admin → Languages**
2. Upload a language file in JSON format
3. Download the default English template for translation
4. Upload translated language files

---

## 🎨 7 Built-in Themes

Choose the perfect look for your site:

| Theme | Style | Best For |
|-------|-------|----------|
| **Future Design** (Default) | Modern gradient | General purpose |
| **Neon Cyberpunk** | Futuristic neon | Tech, gaming |
| **Professional Midnight** | Corporate dark | Business |
| **News Hub** | Content-focused | News, blogs |
| **Gaming Edge** | High energy | Gaming, entertainment |
| **Minimalist Clean** | Simple, elegant | Portfolios, minimal |
| **Aurora Gradient** | Northern lights | Creative, inspiring |

**To switch themes:**
- Admin → Settings → Select theme → Save Changes

---

## 🌐 Language Management

### What's New?

PagefyCMS v2.0 includes a complete language management system:
- Upload custom language files
- Download translation templates
- Support for unlimited languages
- JSON-based translation format

### How to Add a Language

1. Go to **Admin → Languages**
2. Click "Download en.json Template"
3. Translate all strings to your language
4. Go back to Languages panel
5. Enter language code (e.g., "sv" for Swedish)
6. Upload your translated JSON file
7. New language is now available

### Language File Format

```json
{
  "welcome": "Your translation here",
  "dashboard": "Your translation here",
  "pages": "Your translation here"
}
```

---

## 🔌 What Are Addons?

Addons extend PagefyCMS without modifying core code. Enable/disable them from the admin panel.

**Built-in addon system features:**
- Custom functionality without code changes
- Easy enable/disable from admin
- Plugin architecture for extensibility

**Go to:** Admin → Settings → Addons

For detailed addon development, see [Addons Guide](ADDONS_GUIDE.md)

---

## 📁 Project Structure Overview

```
PagefyCMS/
├── Pages/Admin/              → Admin dashboard & management
│   ├── Settings/Languages    → Language management
│   └── ...                   → Other admin pages
├── Pages/                    → Frontend pages
├── Models/                   → Data models (Page, Article, Media)
├── Services/                 → Business logic (ThemeManager, LanguageService)
├── Addons/                   → Plugin system
├── wwwroot/
│   ├── css/                 → Stylesheets (site.css + themes)
│   ├── languages/           → JSON language files
│   ├── themes/              → Theme directories
│   └── uploads/             → Media files
└── Data/                     → Database configuration
```

**Full documentation:** [Main README](README.md)

---

## 💡 Common Tasks

### Create a Custom Theme

1. Create folder: `wwwroot/themes/my-theme/`
2. Create `theme.json` with metadata
3. Create `style.css` with styling
4. Restart application
5. Select from Admin → Settings

See [Themes Guide](PagefyCMS/THEMES_GUIDE.md) for details.

### Create an Addon



1. Create folder: `PagefyCMS/Addons/MyAddon/`
2. Create `MyAddon.cs` extending `BaseAddon`
3. Create `addon.json` with metadata
4. Enable from Admin → Settings → Addons

See [Addons Guide](ADDONS_GUIDE_EN.md) for details.

### Upload Multiple Images

- Go to **Admin → Media**
- Drag multiple images at once
- All are optimized automatically
- Can be organized into gallery groups

### Write & Publish Article

1. **Admin → Articles → Create**
2. Write headline and content
3. Content is auto-formatted
4. Click "Publish"
5. Appears on homepage and articles page

### Manage Pages & Navigation

1. Create pages in **Admin → Pages**
2. Toggle "Show in Menu" to display in navigation
3. Pages appear in top navigation automatically
4. URL slug auto-generated from title

---

## 🖼️ Media Library Features

**Automatic Processing:**
- ✅ WebP conversion for modern browsers
- ✅ Multi-size optimization (small, medium, large)
- ✅ Original file preservation
- ✅ SEO-friendly alt text
- ✅ Gallery organization

**Supported Formats:**
- JPEG / JPG
- PNG
- GIF
- BMP
- TIFF

All converted to WebP automatically!

---

## 🎯 Admin Panel Overview

### Main Navigation

```
Admin Dashboard
├── Pages         → Manage website pages
├── Articles      → Manage news & articles
├── Media         → Upload & organize images
└── Settings
    ├── General   → Site title, editor choice
    ├── Tema      → Switch themes
    ├── Addons    → Enable/disable plugins
    └── Startpage → Customize homepage
```

### Key Features

- **Dark Theme** - Easy on the eyes
- **Responsive Design** - Works on mobile
- **Real-time Updates** - Changes apply immediately
- **Intuitive UI** - Simple navigation
- **Quick Access** - Shortcuts to common tasks

---

## 🚨 Troubleshooting

### Application Won't Start

```bash
# Clear build artifacts
dotnet clean

# Restore and rebuild
dotnet restore
dotnet build

# Run again
dotnet run
```

### Theme Not Changing

1. Verify theme folder exists in `wwwroot/themes/`
2. Check `theme.json` syntax
3. Hard refresh browser (Ctrl+Shift+R)
4. Restart application

### Images Not Optimizing

1. Check `wwwroot/uploads/` folder exists
2. Verify folder permissions are writable
3. Check for console error messages
4. Restart application

### Admin Panel Styling Issues

1. Clear browser cache
2. Hard refresh (Ctrl+Shift+R)
3. Check DevTools (F12) for CSS load errors
4. Verify `wwwroot/css/site.css` exists

---

## 📚 Documentation

**Start here:**
- [Main README](README.md) - Complete overview
- [Themes Guide](THEMES_GUIDE.md) - Theme system details
- [Addons Guide](ADDONS_GUIDE.md) - Addon development

**Additional Resources:**
- [ASP.NET Core Docs](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Razor Pages](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/)

---

## 🤔 Need Help?

- **GitHub Issues:** [Report bugs](https://github.com/K3NT4/PagefyCMS/issues)
- **Discussions:** [Ask questions](https://github.com/K3NT4/PagefyCMS/discussions)
- **Documentation:** [Full guides](README.md)

---

## 🎓 What's Next?

After getting started, explore:

1. **Create Custom Theme** - Make your site unique
2. **Build Addons** - Extend functionality
3. **Customize Pages** - Create complex layouts
4. **Optimize Media** - Professional image handling
5. **SEO Setup** - Improve search visibility

---

## 📝 Quick Reference

**Common URLs:**
- Homepage: `http://localhost:5256/`
- Admin: `http://localhost:5256/Admin/Dashboard`
- Settings: `http://localhost:5256/Admin/Settings`
- Media: `http://localhost:5256/Admin/Media`

**Common Folders:**
- Themes: `wwwroot/themes/`
- CSS: `wwwroot/css/`
- Media: `wwwroot/uploads/`
- Addons: `PagefyCMS/Addons/`

**Default Database:**
- SQLite: `pagefy.db`

---

**You're ready to build amazing websites with PagefyCMS! 🚀**

For more information, see [Main README](README.md)
