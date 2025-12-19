# Themes Guide - Complete Theme System Documentation

**Language:** English  
**For overview:** See [Main README](README_EN.md)

---

## Table of Contents

1. [Overview](#overview)
2. [Theme Architecture](#theme-architecture)
3. [Available Themes](#available-themes)
4. [Using Themes](#using-themes)
5. [Creating Custom Themes](#creating-custom-themes)
6. [Theme Files Reference](#theme-files-reference)
7. [Customization](#customization)
8. [Troubleshooting](#troubleshooting)

---

## Overview

### What is a Theme?

A theme is a collection of CSS stylesheets and metadata that control the visual appearance of your PagefyCMS site. The theme system allows you to:
- Switch visual styles without code changes
- Manage multiple theme variants
- Customize colors and layouts
- Enable/disable features per theme
- Maintain frontend and admin separation

### WordPress-Style System

PagefyCMS uses a **WordPress-inspired theme structure** where:
- Each theme is a self-contained directory
- Theme metadata lives in `theme.json`
- CSS files define all styling
- Themes are activated from admin panel
- Admin panel always uses standard theme

### Admin/Frontend Separation

**Key Benefit:** Your admin panel is never affected by frontend theme changes.

- Admin Panel → Always uses `site.css` (standard theme)
- Frontend → Dynamically loads selected theme CSS
- No CSS conflicts or unexpected styling
- Safe to test themes without affecting admin functionality

---

## Theme Architecture

### Directory Structure

```
wwwroot/themes/
├── framtidsdesign/
│   ├── theme.json           # Metadata file
│   ├── style.css            # Theme-specific styles
│   └── preview.jpg          # Preview image (optional)
├── neon-cyberpunk/
│   ├── theme.json
│   ├── style.css
│   └── preview.jpg
├── professional-midnight/
│   ├── theme.json
│   ├── style.css
│   └── preview.jpg
└── [other themes]/
```

### Core CSS Files

```
wwwroot/css/
├── site.css                 # Admin theme (always loaded)
├── theme-*.css              # Individual theme stylesheets
└── [other CSS files]/
```

---

## Available Themes

### 1. Framtidsdesign (Standard)

**Characteristics:**
- Modern gradient design
- Blue/purple color scheme
- Smooth animations
- Professional appearance
- Default theme for new installations

**Colors:**
- Primary: `#667eea` (Blue)
- Secondary: `#764ba2` (Purple)
- Accent: `#f5576c` (Pink)

**Features:**
- Dark mode
- Animations
- Glow effects
- Responsive design
- Custom fonts

### 2. Neon Cyberpunk

**Characteristics:**
- Futuristic aesthetic
- High contrast neon colors
- Bold typography
- Energetic feel
- Tech-forward design

**Colors:**
- Primary: `#00d9ff` (Cyan)
- Secondary: `#ff006e` (Hot Pink)
- Accent: `#ffbe0b` (Yellow)

**Features:**
- Intense animations
- Glowing text effects
- Cyber-style borders
- High visibility
- Gaming-oriented

### 3. Professional Midnight

**Characteristics:**
- Corporate appearance
- Dark background
- Minimalist design
- Business-focused
- Classic style

**Colors:**
- Primary: `#2563eb` (Blue)
- Secondary: `#1e293b` (Dark Gray)
- Accent: `#60a5fa` (Light Blue)

**Features:**
- Professional layout
- Readability focus
- Standard animations
- Responsive design
- Traditional styling

### 4. News Hub

**Characteristics:**
- Content-optimized layout
- News/blog focused
- Article-friendly typography
- Clear content hierarchy
- Fast content reading

**Colors:**
- Primary: `#dc2626` (Red)
- Secondary: `#1f2937` (Dark Gray)
- Accent: `#fbbf24` (Amber)

**Features:**
- Large typography
- Content emphasis
- Quick scanning
- Publication dates
- Category support

### 5. Gaming Edge

**Characteristics:**
- Gaming/entertainment theme
- High energy
- Bold colors
- Attention-grabbing
- Modern edge design

**Colors:**
- Primary: `#9333ea` (Purple)
- Secondary: `#1e1b4b` (Deep Blue)
- Accent: `#22d3ee` (Cyan)

**Features:**
- Dynamic animations
- Bold typography
- Neon accents
- Hover effects
- Gaming aesthetics

### 6. Minimalist Clean

**Characteristics:**
- Simplistic design
- Maximum readability
- Whitespace emphasis
- Distraction-free
- Elegant simplicity

**Colors:**
- Primary: `#000000` (Black)
- Secondary: `#ffffff` (White)
- Accent: `#6b7280` (Gray)

**Features:**
- Minimal animations
- Clean typography
- Whitespace focus
- Fast loading
- Focus on content

### 7. Aurora Gradient

**Characteristics:**
- Northern lights inspired
- Gradient transitions
- Serene appearance
- Calming colors
- Nature-inspired

**Colors:**
- Primary: `#10b981` (Green)
- Secondary: `#06b6d4` (Cyan)
- Accent: `#8b5cf6` (Purple)

**Features:**
- Smooth gradients
- Calming animations
- Nature colors
- Elegant transitions
- Peaceful design

---

## Using Themes

### Switching Themes in Admin Panel

1. **Navigate to Settings**
   - Go to: `Admin → Settings` (URL: `/Admin/Settings`)
   - Look for "Tema" section

2. **Select Theme**
   - Click the dropdown menu
   - Choose from available themes
   - See preview if available

3. **Save Changes**
   - Click "Spara ändringar" (Save changes) button
   - Changes apply immediately

4. **View Frontend**
   - Visit your site homepage
   - Theme CSS is loaded dynamically
   - Admin remains unchanged

### Verifying Theme Change

**Check In Browser:**
1. Open DevTools (F12)
2. Go to Network tab
3. Look for `theme-*.css` being loaded
4. Inspect element styling to verify theme colors

**Check In Admin:**
1. Admin panel styling unchanged
2. Admin uses `site.css` always
3. No visual difference in admin

---

## Creating Custom Themes

### Step 1: Create Theme Directory

```bash
cd wwwroot/themes
mkdir my-custom-theme
cd my-custom-theme
```

### Step 2: Create theme.json

```json
{
  "id": "my-custom-theme",
  "name": "My Custom Theme",
  "description": "A beautiful custom theme",
  "version": "1.0.0",
  "author": "Your Name",
  "license": "MIT",
  "screenshot": "preview.jpg",
  "colors": {
    "primary": "#667eea",
    "secondary": "#764ba2",
    "accent": "#f5576c"
  },
  "features": {
    "darkMode": true,
    "animations": true,
    "glowEffects": false,
    "responsiveDesign": true,
    "customFont": false
  }
}
```

**Field Descriptions:**
- `id` - Unique identifier (lowercase, no spaces)
- `name` - Display name in admin
- `description` - Brief theme description
- `version` - Semantic version (major.minor.patch)
- `author` - Creator name/organization
- `license` - License type (MIT, GPL, etc.)
- `screenshot` - Preview image filename
- `colors` - Color palette object
- `features` - Boolean feature flags

### Step 3: Create style.css

**Basic Template:**

```css
/* ============================================================================
   MY CUSTOM THEME - Global Styles
   ============================================================================ */

:root {
    /* Colors */
    --primary: #667eea;
    --secondary: #764ba2;
    --accent: #f5576c;
    --bg-dark: #0a0e27;
    --bg-light: #1a1f3a;
    --text-light: #e0e6ff;
    --text-muted: #a0a9c9;
    --border: #2d3561;
    
    /* Spacing */
    --spacing-xs: 0.25rem;
    --spacing-sm: 0.5rem;
    --spacing-md: 1rem;
    --spacing-lg: 1.5rem;
    --spacing-xl: 2rem;
}

/* ============================================================================
   BODY & BASE STYLES
   ============================================================================ */

body {
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
    background: linear-gradient(135deg, var(--bg-dark) 0%, var(--bg-light) 100%);
    color: var(--text-light);
    line-height: 1.6;
    margin: 0;
    padding: 0;
}

/* ============================================================================
   TYPOGRAPHY
   ============================================================================ */

h1, h2, h3, h4, h5, h6 {
    color: var(--primary);
    font-weight: 600;
}

h1 { font-size: 2.5rem; }
h2 { font-size: 2rem; }
h3 { font-size: 1.5rem; }

p {
    color: var(--text-light);
    margin-bottom: 1rem;
}

a {
    color: var(--accent);
    text-decoration: none;
    transition: color 0.3s ease;
}

a:hover {
    color: var(--secondary);
}

/* ============================================================================
   CONTAINERS & CARDS
   ============================================================================ */

.container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 var(--spacing-lg);
}

.card {
    background: rgba(26, 31, 58, 0.6);
    border: 1px solid var(--border);
    border-radius: 8px;
    padding: var(--spacing-lg);
    margin-bottom: var(--spacing-lg);
}

/* ============================================================================
   BUTTONS
   ============================================================================ */

.btn {
    padding: var(--spacing-sm) var(--spacing-lg);
    border: none;
    border-radius: 6px;
    cursor: pointer;
    font-weight: 600;
    transition: all 0.3s ease;
}

.btn-primary {
    background: var(--primary);
    color: white;
}

.btn-primary:hover {
    background: var(--secondary);
    transform: translateY(-2px);
}

/* ============================================================================
   FORM ELEMENTS
   ============================================================================ */

input, textarea, select {
    background: rgba(45, 53, 97, 0.5);
    border: 1px solid var(--border);
    color: var(--text-light);
    padding: var(--spacing-sm) var(--spacing-md);
    border-radius: 4px;
    font-family: inherit;
}

input:focus, textarea:focus, select:focus {
    outline: none;
    border-color: var(--accent);
    box-shadow: 0 0 0 3px rgba(245, 87, 108, 0.2);
}

/* ============================================================================
   RESPONSIVE DESIGN
   ============================================================================ */

@media (max-width: 768px) {
    h1 { font-size: 2rem; }
    h2 { font-size: 1.5rem; }
    
    .container {
        padding: 0 var(--spacing-md);
    }
}

/* ============================================================================
   ANIMATIONS
   ============================================================================ */

@keyframes fadeIn {
    from {
        opacity: 0;
        transform: translateY(10px);
    }
    to {
        opacity: 1;
        transform: translateY(0);
    }
}

.fade-in {
    animation: fadeIn 0.3s ease-out;
}
```

### Step 4: Add Preview Image (Optional)

- Create a screenshot of your theme
- Save as `preview.jpg` or `preview.png`
- Recommended size: 800x600 or 1200x800 pixels
- Used in theme browser

### Step 5: Test Your Theme

1. Copy theme folder to `wwwroot/themes/`
2. Start application
3. Go to Admin → Settings
4. Select your theme from dropdown
5. Click save
6. Check frontend for styling

---

## Theme Files Reference

### theme.json Format

**Complete Example:**

```json
{
  "id": "example-theme",
  "name": "Example Theme",
  "description": "A comprehensive example theme for PagefyCMS",
  "version": "1.0.0",
  "author": "Theme Developer",
  "license": "MIT",
  "homepage": "https://example.com",
  "repository": "https://github.com/example/theme",
  "screenshot": "preview.jpg",
  "colors": {
    "primary": "#667eea",
    "secondary": "#764ba2",
    "accent": "#f5576c",
    "background": "#0a0e27",
    "text": "#e0e6ff",
    "success": "#10b981",
    "warning": "#f59e0b",
    "danger": "#ef4444"
  },
  "fonts": {
    "heading": "Roboto, sans-serif",
    "body": "-apple-system, BlinkMacSystemFont, Segoe UI, sans-serif"
  },
  "features": {
    "darkMode": true,
    "animations": true,
    "glowEffects": true,
    "responsiveDesign": true,
    "customFont": true
  },
  "settings": {
    "primaryColor": "#667eea",
    "accentColor": "#f5576c",
    "contentWidth": "1200px",
    "animationSpeed": "0.3s"
  }
}
```

### CSS Best Practices

**Use CSS Variables:**
```css
:root {
    --primary: #667eea;
    --secondary: #764ba2;
    --text: #e0e6ff;
}

button {
    background: var(--primary);
}
```

**Mobile-First Approach:**
```css
/* Mobile styles first */
body {
    font-size: 14px;
}

/* Desktop styles */
@media (min-width: 768px) {
    body {
        font-size: 16px;
    }
}
```

**Optimize Performance:**
```css
/* Good - Minimal repaints */
.card {
    transition: transform 0.3s ease;
}

.card:hover {
    transform: translateY(-4px);
}

/* Avoid - Heavy animations */
.card:hover {
    box-shadow: 0 0 10px 0 rgba(0,0,0,0.5);
    transform: scale(1.05) rotate(1deg);
}
```

---

## Customization

### Extending Existing Themes

**Create a variant:**

1. Copy existing theme folder
2. Rename to `original-theme-custom`
3. Update `theme.json` with new name
4. Modify `style.css` colors/styles
5. Test in admin panel

**Example - Cyberpunk Dark Variant:**

```bash
cp -r neon-cyberpunk neon-cyberpunk-dark
cd neon-cyberpunk-dark
```

Then update `theme.json`:
```json
{
  "id": "neon-cyberpunk-dark",
  "name": "Neon Cyberpunk - Dark",
  ...
}
```

And modify colors in `style.css`:
```css
:root {
    --bg-dark: #0a0000;
    --primary: #00ffff;
    --accent: #ff0080;
}
```

### Overriding Theme Styles

**In your page HTML:**
```html
<style>
    /* Override theme variable */
    :root {
        --primary: #your-custom-color;
    }
</style>
```

**Create a theme-specific page:**
```html
<link rel="stylesheet" href="/css/theme-custom-override.css">
```

---

## Troubleshooting

### Theme Not Appearing

1. **Check Directory**
   - Verify folder is in `wwwroot/themes/`
   - Check folder name matches theme `id`

2. **Check theme.json**
   - Validate JSON syntax
   - Required fields: `id`, `name`, `version`, `author`
   - Use JSON validator: [jsonlint.com](https://jsonlint.com)

3. **Restart Application**
   ```bash
   # Stop running app (Ctrl+C)
   # Restart
   dotnet run
   ```

### Theme CSS Not Loading

1. **Check Browser Network Tab**
   - Open DevTools (F12)
   - Go to Network tab
   - Look for `theme-*.css` file
   - Check response status (should be 200)

2. **Verify File Path**
   - CSS should be at: `wwwroot/css/theme-{id}.css`
   - Or in theme folder: `wwwroot/themes/{id}/style.css`

3. **Clear Browser Cache**
   - Hard refresh: Ctrl+Shift+R (Windows) or Cmd+Shift+R (Mac)
   - Clear cache in DevTools settings

### Styles Conflicting with Admin

1. **Verify Admin Isolation**
   - Admin panel should use `/Admin/` URL
   - Admin panel should load `site.css` only
   - Frontend pages should load theme CSS

2. **Check CSS Specificity**
   - Avoid `!important` overrides
   - Use appropriate selectors
   - Test in browser DevTools

### Colors Not Matching

1. **Verify Color Format**
   - Use hex: `#667eea`
   - Use RGB: `rgb(102, 126, 234)`
   - Use HSL: `hsl(251, 74%, 66%)`

2. **Check Browser Color Space**
   - Some browsers display colors differently
   - Use color picker tools
   - Test on multiple browsers

---

## Best Practices

### Do's ✅

- ✅ Use CSS variables for colors
- ✅ Mobile-first responsive design
- ✅ Optimize images for web
- ✅ Test on multiple browsers
- ✅ Document custom features
- ✅ Follow semantic HTML
- ✅ Keep CSS organized in sections
- ✅ Use meaningful class names

### Don'ts ❌

- ❌ Don't use `!important` excessively
- ❌ Don't hardcode colors
- ❌ Don't use deprecated CSS
- ❌ Don't forget responsive design
- ❌ Don't load huge fonts/images
- ❌ Don't modify admin CSS
- ❌ Don't use inline styles
- ❌ Don't break accessibility

---

## Performance Tips

### Image Optimization

- Use WebP format (automatic in PagefyCMS)
- Compress images before uploading
- Use appropriate image sizes
- Lazy load below-fold images

### CSS Optimization

- Minimize CSS file size
- Remove unused styles
- Use CSS variables
- Combine media queries

### Animation Optimization

- Use `transform` and `opacity`
- Avoid `top`, `left`, `width` changes
- Keep animations under 0.5s
- Use `will-change` sparingly

---

## Additional Resources

- [Main README](README_EN.md)
- [Addons Guide](ADDONS_GUIDE_EN.md)
- [CSS Tricks](https://css-tricks.com/)
- [MDN CSS Docs](https://developer.mozilla.org/en-US/docs/Web/CSS)
- [Color Picker](https://htmlcolorcodes.com/)

---

**Last Updated:** December 2025  
**Maintained by:** PagefyCMS Community
