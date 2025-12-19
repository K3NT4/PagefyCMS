# PagefyCMS - Modern Content Management System

PagefyCMS is a lightweight, flexible content management system built with ASP.NET Core 8.0. It provides a powerful yet simple platform for managing pages, articles, media, and themes without heavyweight frameworks or limitations.

**Perfect for:** News sites, blogs, portfolios, landing pages, and custom web projects where you need full control.

---

## ✨ Key Features

- 📄 **Page & Article Management** - Create and manage content with flexible layouts
- 🎨 **WordPress-style Theme System** - Easy theme switching with 7 pre-built themes
- 🔌 **Addon System** - Extend functionality without modifying core code
- 🖼️ **Media Library** - Automatic WebP conversion, image optimization, and multi-size generation
- 🎯 **Admin Dashboard** - Modern, intuitive control panel with dark theme
- 🚀 **Performance Optimized** - Automatic image scaling, caching, and SEO support
- 🛡️ **Database Persistence** - SQLite database with Entity Framework Core migrations
- 🎨 **7 Professional Themes** - Pre-built themes ready to use or customize
- 📱 **Responsive Design** - Mobile-friendly across all admin and frontend pages
- 🔐 **Admin Panel** - Secure administration area with session management

---

## 📋 Table of Contents

1. [Quick Start](#quick-start)
2. [Project Structure](#project-structure)
3. [Core Features](#core-features)
4. [Themes System](#themes-system)
5. [Addon System](#addon-system)
6. [Media Library](#media-library)
7. [Database & Models](#database--models)
8. [Future Development](#future-development)
9. [Contributing](#contributing)
10. [License](#license)

---

## 🚀 Quick Start

### Prerequisites

- .NET 8.0 SDK
- Git

### Installation

```bash
# Clone the repository
git clone https://github.com/K3NT4/PagefyCMS.git
cd PagefyCMS/PagefyCMS

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

The application will start at `http://localhost:5256`

### Default Admin Access

- **URL:** `http://localhost:5256/Admin/Login`
- **Note:** Set up your admin user as needed

---

## 📁 Project Structure

```
PagefyCMS/
├── Controllers/              # API controllers for media handling
├── Models/                   # Data models (Page, Article, MediaItem, CmsSetting)
├── Data/                     # Database context and configuration
├── Pages/                    # Razor Pages for frontend and admin
│   ├── Admin/               # Admin dashboard and management pages
│   │   ├── Articles/        # Article CRUD operations
│   │   ├── Media/           # Media library management
│   │   ├── Pages/           # Page management
│   │   ├── Settings/        # System settings, themes, addons
│   │   └── Dashboard.cshtml # Admin dashboard
│   ├── Shared/              # Shared layouts and components
│   └── [Index,Article etc]  # Frontend pages
├── Services/                # Business logic layer
│   └── ThemeManager.cs      # Theme management service
├── Addons/                  # Addon system infrastructure
│   ├── IAddon.cs            # Addon interface
│   ├── BaseAddon.cs         # Base addon class
│   ├── AddonManager.cs      # Addon loading and management
│   └── DynamicAddon.cs      # Dynamic addon support
├── Migrations/              # EF Core database migrations
├── wwwroot/
│   ├── css/
│   │   ├── site.css         # Main stylesheet (dark theme)
│   │   └── theme-*.css      # Individual theme stylesheets
│   ├── js/                  # JavaScript utilities
│   ├── themes/              # Theme directories with theme.json
│   └── uploads/             # User-uploaded media
├── appsettings.json         # Application settings
└── Program.cs               # Application startup configuration
```

---

## 🎯 Core Features

### Pages & Articles

**Create Content:**
- Admin Panel → Pages/Articles → Create
- Support for rich text editing with TinyMCE or Toast UI Editor
- Publish/Draft status management
- SEO-friendly slug generation

**View Content:**
- Frontend pages are dynamically rendered
- Articles displayed with publication dates
- Category/tag support (extensible)

### Admin Dashboard

**Features:**
- Real-time statistics (pages, articles, media count)
- Quick access to all management sections
- User-friendly dark theme interface
- Responsive design for all screen sizes

### Settings & Configuration

Access via: Admin → Settings

- **General Settings** - Site title, editor selection
- **Theme Selection** - Switch between available themes
- **Addon Management** - Enable/disable addons
- **Site Customization** - Fonts, colors, layouts

---

## 🎨 Themes System

### Overview

PagefyCMS features a **WordPress-style theme system** that allows easy switching and customization without code changes.

### Available Themes

1. **Framtidsdesign** (Standard) - Modern gradient design with blue/purple colors
2. **Neon Cyberpunk** - Futuristic neon aesthetic
3. **Professional Midnight** - Classic corporate design
4. **News Hub** - Optimized for news and content-focused sites
5. **Gaming Edge** - High-energy gaming/entertainment theme
6. **Minimalist Clean** - Simple, elegant, distraction-free design
7. **Aurora Gradient** - Northern lights-inspired color palette

### How Themes Work

**Theme Directory Structure:**
```
wwwroot/themes/{theme-id}/
├── theme.json           # Metadata (name, colors, features)
├── style.css            # Theme-specific styles
└── preview.jpg          # Preview image for browser
```

**theme.json Format:**
```json
{
  "id": "theme-id",
  "name": "Theme Name",
  "description": "Theme description",
  "version": "1.0.0",
  "author": "Author Name",
  "colors": {
    "primary": "#667eea",
    "secondary": "#764ba2",
    "accent": "#f5576c"
  },
  "features": {
    "darkMode": true,
    "animations": true,
    "glowEffects": true,
    "responsiveDesign": true,
    "customFont": true
  }
}
```

### Using Themes

**To Switch Themes:**
1. Go to Admin → Settings
2. Select theme from "Tema" dropdown
3. Click "Spara ändringar" (Save changes)
4. Frontend will immediately reflect the new theme
5. Admin panel remains on standard theme

**Admin Theme Separation:**
- Admin always uses `site.css` (standard theme)
- Frontend dynamically loads selected theme
- This prevents admin disruption from theme changes

### Creating Custom Themes

See [Themes Guide](PagefyCMS/THEMES_GUIDE.md) for detailed instructions on creating custom themes.

---

## 🔌 Addon System

### Overview

The addon system allows extending PagefyCMS functionality without modifying core code. Create custom features like:
- Hook systems for custom logic
- Custom pages and components
- Database integrations
- Third-party service connections

### Addon Structure

**Required Files:**
```
Addons/MyAddon/
├── MyAddon.cs               # Addon class implementing IAddon
└── addon.json               # Addon metadata
```

**addon.json Format:**
```json
{
  "id": "my-addon",
  "name": "My Custom Addon",
  "version": "1.0.0",
  "author": "Your Name",
  "description": "What this addon does",
  "enabled": false
}
```

### Creating an Addon

**Step 1: Create Addon Class**
```csharp
using PagefyCMS.Addons;

public class MyAddon : BaseAddon
{
    public MyAddon() : base("my-addon", "My Addon", "1.0.0", "Your Name")
    {
    }

    public override void OnInit()
    {
        // Initialize addon
    }

    public override void OnPostPageCreate(dynamic page)
    {
        // Hook for after page creation
    }
}
```

**Step 2: Create addon.json**
```json
{
  "id": "my-addon",
  "name": "My Addon",
  "version": "1.0.0",
  "author": "Your Name",
  "description": "My custom addon"
}
```

**Step 3: Place in Addons Folder**
```
PagefyCMS/Addons/MyAddon/MyAddon.cs
PagefyCMS/Addons/MyAddon/addon.json
```

**Step 4: Enable from Admin**
1. Go to Admin → Settings → Addons
2. Find your addon in the list
3. Click "Enable" button
4. Addon is now active

### Available Hooks

- `OnInit()` - When addon is loaded
- `OnPostPageCreate(page)` - After page is created
- `OnPostPageUpdate(page)` - After page is updated
- `OnPostArticleCreate(article)` - After article is created
- `OnPostArticleUpdate(article)` - After article is updated
- `OnMediaUpload(media)` - After media is uploaded

### Example Addons

- See `ExampleAddons/` directory for complete examples
- SEO Addon - Manages SEO metadata
- Activity Log Addon - Tracks user actions
- See [Addons Guide](ADDONS_GUIDE.md) for more details

---

## 🖼️ Media Library

### Features

- **Automatic WebP Conversion** - All images converted to modern WebP format
- **Multi-Size Optimization** - Generates 3 sizes: small, medium, large
- **Original Preservation** - Keeps original for backup/reference
- **SEO Support** - Alt text and title fields for every image
- **Gallery Groups** - Organize media into galleries
- **Metadata** - Title, description, slug for each item

### Supported Formats

- JPEG / JPG
- PNG
- GIF (converted to WebP)
- BMP
- TIFF

### File Organization

```
wwwroot/uploads/
├── originals/               # Original uploaded files
├── webp/
│   ├── small/              # Mobile-optimized (max 400px)
│   ├── medium/             # Tablet-optimized (max 800px)
│   └── large/              # Desktop-optimized (max 1600px)
└── [image files]/
```

### Using Media in Pages

1. Admin → Media → Upload Image
2. Set title, alt text, and metadata
3. In page editor, reference the image
4. Frontend automatically serves optimized WebP version
5. Browser automatically falls back to original if needed

---

## 💾 Database & Models

### Database

**Engine:** SQLite  
**Location:** `pagefy.db`  
**ORM:** Entity Framework Core 8.0

### Data Models

**Page**
```csharp
public class Page
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool ShowInMenu { get; set; }
    public string? GalleryGroup { get; set; }
}
```

**Article**
```csharp
public class Article
{
    public int Id { get; set; }
    public string Headline { get; set; }
    public string Slug { get; set; }
    public string Content { get; set; }
    public DateTime PublishedAt { get; set; }
}
```

**MediaItem**
```csharp
public class MediaItem
{
    public int Id { get; set; }
    public string Filename { get; set; }
    public string Title { get; set; }
    public string AltText { get; set; }
    public DateTime UploadedAt { get; set; }
    public string? WebpSmall { get; set; }
    public string? WebpMedium { get; set; }
    public string? WebpLarge { get; set; }
    public string? GalleryGroup { get; set; }
}
```

**CmsSetting**
```csharp
public class CmsSetting
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public string? ActiveTheme { get; set; }
}
```

### Database Migrations

Migrations are managed with EF Core. New migrations automatically run on application startup.

---

## 📚 Documentation

- **[Themes Guide](PagefyCMS/THEMES_GUIDE.md)** - Detailed theme system documentation
- **[Themes Quick Start](PagefyCMS/THEMES_QUICKSTART.md)** - Get started with themes quickly
- **[Addons Guide](ADDONS_GUIDE.md)** - Complete addon development guide
- **[Addon System Summary](ADDON_SYSTEM_SUMMARY.md)** - Overview of addon architecture
- **[Quick Reference](QUICKFIX_REFERENCE.md)** - Common fixes and solutions
- **[Bugfixes & Improvements](BUGFIXES_AND_IMPROVEMENTS.md)** - Recent fixes and improvements

---

## 🔄 Recent Updates

### v1.0.0 (Current)
- ✅ WordPress-style theme system with 7 themes
- ✅ Addon system with hooks and plugin architecture
- ✅ Fixed NOT NULL constraint on ActiveTheme field
- ✅ Enhanced form visibility in dark theme
- ✅ Consistent admin UI styling
- ✅ Admin/frontend theme separation
- ✅ Media library with WebP optimization
- ✅ SEO support for pages and media

---

## 🚧 Future Development

- [ ] User roles and permissions system
- [ ] Comment/discussion system for articles
- [ ] Advanced search functionality
- [ ] Backup and export features
- [ ] Custom domain support
- [ ] CDN integration for media
- [ ] Email notification system
- [ ] Advanced analytics
- [ ] API for third-party integrations

---

## 🤝 Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Bug Reports

Found a bug? Please open an issue with:
- Description of the bug
- Steps to reproduce
- Expected vs actual behavior
- Screenshots if applicable

---

## 📄 License

This project is licensed under the MIT License - see [LICENSE](LICENSE) file for details.

---

## 🌐 Contact & Support

- **GitHub:** [K3NT4/PagefyCMS](https://github.com/K3NT4/PagefyCMS)
- **Issues:** [GitHub Issues](https://github.com/K3NT4/PagefyCMS/issues)
- **Discussions:** [GitHub Discussions](https://github.com/K3NT4/PagefyCMS/discussions)

---

## 🎓 Learning Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Razor Pages](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/)
- [CSS Best Practices](https://developer.mozilla.org/en-US/docs/Web/CSS)

---

**Made with ❤️ by the PagefyCMS Community**
