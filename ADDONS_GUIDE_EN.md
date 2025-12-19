# Addons Guide - Complete Developer Documentation

**Language:** English  
**For overview:** See [Main README](README_EN.md)

---

## Table of Contents

1. [Getting Started](#getting-started)
2. [Addon Architecture](#addon-architecture)
3. [Creating Your First Addon](#creating-your-first-addon)
4. [Addon Hooks](#addon-hooks)
5. [Advanced Features](#advanced-features)
6. [Examples](#examples)
7. [Troubleshooting](#troubleshooting)

---

## Getting Started

### What is an Addon?

An addon is a self-contained module that extends PagefyCMS functionality without modifying core code. Addons can:
- Hook into system events
- Add custom pages and routes
- Integrate with external services
- Modify data before/after operations
- Run background tasks

### Why Use Addons?

- **Safe** - Core system remains unchanged
- **Maintainable** - Updates don't break addons
- **Reusable** - Share addons across projects
- **Easy to Enable/Disable** - No code changes required
- **Isolated** - Each addon is independent

---

## Addon Architecture

### Folder Structure

```
PagefyCMS/
└── Addons/
    ├── MyAddon/
    │   ├── MyAddon.cs           # Main addon class
    │   └── addon.json           # Metadata file
    ├── AnotherAddon/
    │   ├── AnotherAddon.cs
    │   └── addon.json
    └── ExampleAddons/           # Pre-built examples
        ├── ActivityLogAddon.cs
        ├── SeoAddon.cs
        └── CompleteExampleAddons.cs
```

### Addon Metadata (addon.json)

```json
{
  "id": "unique-addon-id",
  "name": "Display Name",
  "version": "1.0.0",
  "author": "Your Name",
  "description": "What this addon does",
  "enabled": false
}
```

**Fields:**
- `id` - Unique identifier (lowercase, no spaces)
- `name` - Display name in admin panel
- `version` - Semantic versioning (major.minor.patch)
- `author` - Author name or organization
- `description` - Brief description of functionality
- `enabled` - Initial enabled state (false by default)

---

## Creating Your First Addon

### Step 1: Create Addon Directory

```bash
cd PagefyCMS/Addons
mkdir MyFirstAddon
cd MyFirstAddon
```

### Step 2: Create addon.json

```json
{
  "id": "my-first-addon",
  "name": "My First Addon",
  "version": "1.0.0",
  "author": "Your Name",
  "description": "My first addon for PagefyCMS",
  "enabled": false
}
```

### Step 3: Create Addon Class

**File: MyFirstAddon.cs**

```csharp
using PagefyCMS.Addons;
using System;

namespace PagefyCMS.Addons.MyFirstAddon
{
    public class MyFirstAddon : BaseAddon
    {
        public MyFirstAddon() 
            : base(
                "my-first-addon",
                "My First Addon",
                "1.0.0",
                "Your Name"
            )
        {
        }

        public override void OnInit()
        {
            Console.WriteLine("My First Addon initialized!");
        }

        public override void OnPostPageCreate(dynamic page)
        {
            Console.WriteLine($"New page created: {page.Title}");
        }
    }
}
```

### Step 4: Enable in Admin Panel

1. Go to `Admin → Settings → Addons`
2. Find "My First Addon" in the list
3. Click the "Enable" button
4. Check console output to verify initialization

---

## Addon Hooks

Hooks are methods that execute at specific points in the application lifecycle.

### Available Hooks

#### 1. **OnInit()**
Executes when the addon is loaded.

```csharp
public override void OnInit()
{
    // Initialize addon resources
    // Load configuration
    // Register event listeners
    Logger.Log("Addon initialized");
}
```

#### 2. **OnPostPageCreate(dynamic page)**
Executes after a new page is created.

```csharp
public override void OnPostPageCreate(dynamic page)
{
    // Log page creation
    // Send notifications
    // Generate SEO metadata
    var pageTitle = page.Title;
    var pageSlug = page.Slug;
}
```

**Page Object Properties:**
- `Id` - Page ID
- `Title` - Page title
- `Slug` - URL-friendly slug
- `Content` - Page content HTML
- `CreatedAt` - Creation timestamp
- `ShowInMenu` - Display in navigation

#### 3. **OnPostPageUpdate(dynamic page)**
Executes after a page is updated.

```csharp
public override void OnPostPageUpdate(dynamic page)
{
    // Log page changes
    // Invalidate cache
    // Update search index
}
```

#### 4. **OnPostArticleCreate(dynamic article)**
Executes after a new article is published.

```csharp
public override void OnPostArticleCreate(dynamic article)
{
    // Send publication notifications
    // Schedule social media posts
    // Update feeds
    var headline = article.Headline;
    var publishedAt = article.PublishedAt;
}
```

**Article Object Properties:**
- `Id` - Article ID
- `Headline` - Article title
- `Slug` - URL-friendly slug
- `Content` - Article content HTML
- `PublishedAt` - Publication date

#### 5. **OnPostArticleUpdate(dynamic article)**
Executes after an article is updated.

```csharp
public override void OnPostArticleUpdate(dynamic article)
{
    // Log updates
    // Notify subscribers of changes
}
```

#### 6. **OnMediaUpload(dynamic media)**
Executes after media is uploaded.

```csharp
public override void OnMediaUpload(dynamic media)
{
    // Scan for viruses
    // Extract metadata
    // Generate thumbnails
    // Optimize images
    var filename = media.Filename;
    var title = media.Title;
}
```

**Media Object Properties:**
- `Id` - Media ID
- `Filename` - Original filename
- `Title` - Display title
- `AltText` - Alt text for accessibility
- `UploadedAt` - Upload timestamp
- `WebpSmall` - Small optimized version
- `WebpMedium` - Medium optimized version
- `WebpLarge` - Large optimized version
- `GalleryGroup` - Gallery category

---

## Advanced Features

### Using Dependency Injection

Access system services in your addon:

```csharp
using PagefyCMS.Data;
using Microsoft.AspNetCore.Hosting;

public class AdvancedAddon : BaseAddon
{
    private readonly PagefyDbContext _context;
    private readonly IWebHostEnvironment _env;

    public AdvancedAddon(PagefyDbContext context, IWebHostEnvironment env)
        : base("advanced-addon", "Advanced Addon", "1.0.0", "Author")
    {
        _context = context;
        _env = env;
    }

    public override void OnPostPageCreate(dynamic page)
    {
        // Access database
        var pageCount = _context.Pages.Count();
        
        // Get environment info
        var basePath = _env.WebRootPath;
    }
}
```

### Accessing the Database

```csharp
using PagefyCMS.Models;
using PagefyCMS.Data;

public override void OnPostPageCreate(dynamic page)
{
    // Create custom settings for this page
    var setting = new CmsSetting
    {
        Key = $"page-{page.Id}-views",
        Value = "0",
        ActiveTheme = null
    };
    
    _context.Settings.Add(setting);
    _context.SaveChanges();
}
```

### Logging

```csharp
using System;

public override void OnInit()
{
    Console.WriteLine("✓ Addon Started");
}

public override void OnPostPageCreate(dynamic page)
{
    Console.WriteLine($"📄 New page: {page.Title}");
}

public override void OnMediaUpload(dynamic media)
{
    Console.WriteLine($"🖼️ Media uploaded: {media.Filename}");
}
```

### Conditional Execution

```csharp
public override void OnPostArticleCreate(dynamic article)
{
    // Only notify if article contains "important" tag
    if (article.Content.Contains("important"))
    {
        SendNotification($"Important article published: {article.Headline}");
    }
}

private void SendNotification(string message)
{
    // Email, Slack, webhook, etc.
    Console.WriteLine($"NOTIFICATION: {message}");
}
```

---

## Examples

### Example 1: SEO Addon

Automatically generates SEO metadata:

```csharp
public class SeoAddon : BaseAddon
{
    public SeoAddon() : base("seo-addon", "SEO Addon", "1.0.0", "Team")
    {
    }

    public override void OnPostPageCreate(dynamic page)
    {
        var setting = new CmsSetting
        {
            Key = $"seo-page-{page.Id}",
            Value = GenerateMetaDescription(page.Content),
            ActiveTheme = null
        };
        // Save to database
    }

    private string GenerateMetaDescription(string content)
    {
        return content.Length > 155 
            ? content.Substring(0, 155) + "..." 
            : content;
    }
}
```

### Example 2: Activity Logger

Logs all actions:

```csharp
public class ActivityLogAddon : BaseAddon
{
    private readonly PagefyDbContext _context;

    public ActivityLogAddon(PagefyDbContext context)
        : base("activity-log", "Activity Logger", "1.0.0", "Team")
    {
        _context = context;
    }

    public override void OnPostPageCreate(dynamic page)
    {
        LogActivity("PAGE_CREATED", $"Page '{page.Title}' created");
    }

    public override void OnPostArticleCreate(dynamic article)
    {
        LogActivity("ARTICLE_PUBLISHED", $"Article '{article.Headline}' published");
    }

    private void LogActivity(string action, string description)
    {
        var log = new CmsSetting
        {
            Key = $"log-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}",
            Value = $"{action}: {description}",
            ActiveTheme = null
        };
        _context.Settings.Add(log);
        _context.SaveChanges();
    }
}
```

### Example 3: Email Notifier

Sends email on article creation:

```csharp
using System.Net.Mail;

public class EmailNotifierAddon : BaseAddon
{
    public EmailNotifierAddon()
        : base("email-notifier", "Email Notifier", "1.0.0", "Team")
    {
    }

    public override void OnPostArticleCreate(dynamic article)
    {
        SendEmail(
            "admin@example.com",
            $"New Article: {article.Headline}",
            $"A new article was published: {article.Headline}"
        );
    }

    private void SendEmail(string to, string subject, string body)
    {
        try
        {
            using (var client = new SmtpClient("localhost"))
            {
                var message = new MailMessage("noreply@pagefycms.com", to)
                {
                    Subject = subject,
                    Body = body
                };
                client.Send(message);
                Console.WriteLine($"✉️ Email sent to {to}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Email error: {ex.Message}");
        }
    }
}
```

---

## Troubleshooting

### Addon Not Appearing in Admin Panel

1. Verify `addon.json` exists in addon folder
2. Check JSON syntax (use JSON validator)
3. Ensure folder is in `PagefyCMS/Addons/`
4. Restart application
5. Clear browser cache

### Addon Not Initializing

1. Check console for error messages
2. Verify class inherits from `BaseAddon`
3. Check constructor parameters match available services
4. Verify `OnInit()` doesn't throw exceptions

### Hook Not Firing

1. Verify addon is enabled in admin panel
2. Check hook method name is correct
3. Verify parameters match expected types
4. Add logging to confirm execution

### Database Errors

1. Check `_context` is injected correctly
2. Verify table/model exists
3. Check database migrations are up-to-date
4. Review Entity Framework Core documentation

---

## Best Practices

### Do's ✅

- ✅ Use try-catch for error handling
- ✅ Log important actions
- ✅ Keep addons focused and single-purpose
- ✅ Follow C# naming conventions
- ✅ Document your addon's behavior
- ✅ Test thoroughly before deploying

### Don'ts ❌

- ❌ Don't modify core system files
- ❌ Don't make synchronous HTTP calls
- ❌ Don't access private/internal properties
- ❌ Don't ignore exceptions silently
- ❌ Don't create infinite loops
- ❌ Don't hardcode sensitive data

---

## Additional Resources

- [PagefyCMS Main Documentation](README_EN.md)
- [Themes Guide](PagefyCMS/THEMES_GUIDE.md)
- [ASP.NET Core Dependency Injection](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

---

**Last Updated:** December 2025  
**Maintained by:** PagefyCMS Community
