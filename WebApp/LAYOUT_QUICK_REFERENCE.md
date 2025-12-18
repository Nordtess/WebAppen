# Layout Behavior Quick Reference

## Breakpoint Rule
```css
/* ONLY ONE BREAKPOINT IN ENTIRE PROJECT */
@media (max-width: 800px) { 
    /* Mobile styles */
}
```

## Desktop (> 800px)

### Visual Layout
```
???????????????????????????????????????????????????
?  [Logo]                    ?? 3 msgs  email     ?  ? Header (70px)
???????????????????????????????????????????????????
?          ?                                      ?
?  ?? Home ?                                      ?
?  ?? Sök  ?          MAIN CONTENT                ?
?  ?? Proj ?        (scrolls here)                ?
?  ?? CV   ?                                      ?
?  ?? Msgs ?                                      ?
?  ?? Login?                                      ?
?  ? Ny   ?                                      ?
?          ?                                      ?
???????????????????????????????????????????????????
?         © 2025 - Privacy                        ?  ? Footer (56px)
???????????????????????????????????????????????????
   ? 240px        ? Flex 1fr
   Sidebar        Main
```

### Key Features
- ? Hamburger: **HIDDEN** (display: none)
- ? Sidebar: **ALWAYS VISIBLE** (240px wide)
- ? Scrolling: **ONLY MAIN** (body overflow hidden)
- ? Header/Sidebar: **STAY IN PLACE** while scrolling
- ? User area: Icon + Text + Email (full)

## Mobile (<= 800px)

### Default State (Sidebar Closed)
```
??????????????????????????????
? [?] [Logo]  ?? 3 msgs     ?  ? Header (60px)
??????????????????????????????
?                            ?
?                            ?
?      MAIN CONTENT          ?
?      (scrollable)          ?
?                            ?
?                            ?
??????????????????????????????
?    © 2025 - Privacy        ?  ? Footer (50px)
??????????????????????????????
```

### Sidebar Open State
```
??????????????????????????????
?              ?[?] [Logo]???
?  ?? Home     ???????????????
?  ?? Sök CV   ?             ?
?  ?? Projekt  ?   (DIMMED)  ?
?  ?? Mitt CV  ?             ?
?  ?? Meddelan ?   OVERLAY   ?
?  ?? Logga in ?             ?
?  ? Bli medl ?             ?
?              ???????????????
?              ?   Footer    ?
??????????????????????????????
   ? 280px        ? Dimmed
   Off-canvas     (clickable to close)
```

### Key Features
- ? Hamburger: **VISIBLE** (top-left corner)
- ? Sidebar: **OFF-CANVAS** (hidden by default)
- ? Opens: Click hamburger
- ? Closes: Overlay click, ESC key, link click
- ? User area: Icon + Text only (email hidden)

## Scrolling Behavior

### Desktop
```css
body {
    overflow: hidden;        /* Body doesn't scroll */
}

.app-main {
    overflow-y: auto;        /* Only main scrolls */
    height: 100%;
}
```

### Result
- Header: Fixed at top
- Sidebar: Fixed on left
- Main: Scrolls independently
- Footer: Fixed at bottom

## Header User Area (Authenticated)

### Code Structure
```razor
@if (User?.Identity?.IsAuthenticated == true)
{
    <div class="header-user-area">
        <img src="~/images/svg/mail.svg" class="icon-mail" />
        <span class="unread-text">Du har <strong>3</strong> olästa meddelanden</span>
        <span class="user-email">jamie@example.se</span>
    </div>
}
```

### Desktop Display
```
[??] Du har 3 olästa meddelanden jamie@example.se
```

### Mobile Display
```
[??] Du har 3 olästa meddelanden
```

## Sidebar Links (All Modes)

```
?? Home
?? Sök CV
?? Alla projekt
?? Mitt CV
?? Meddelanden
?? Logga in
? Bli medlem
```

## Off-Canvas Sidebar Behavior

### Opening
1. User clicks **hamburger** (?)
2. Sidebar **slides in** from left (0.3s)
3. **Overlay appears** (fade-in 0.3s)
4. **Background scroll disabled**
5. ARIA: aria-expanded="true"

### Closing (3 ways)
1. **Click overlay** (dimmed area)
2. **Press ESC** key
3. **Click any link** (navigates + closes)

## CSS Classes Reference

### Layout Structure
- `.app-shell` - Grid container
- `.app-header` - Header area
- `.app-sidebar` - Sidebar navigation
- `.app-main` - Main content (scrollable)
- `.app-footer` - Footer area

### Header Components
- `.header-content` - Header flex wrapper
- `.header-left` - Left side (hamburger + logo)
- `.header-logo` - Logo container
- `.logo-img` - Logo image
- `.hamburger-btn` - Mobile menu button
- `.hamburger-line` - Burger lines (3x)
- `.header-user-area` - Authenticated user info
- `.icon-mail` - Mail SVG icon (20x20px)
- `.unread-text` - Unread message text
- `.user-email` - User's email address

### Sidebar Components
- `.sidebar-nav` - Nav links container
- `.sidebar-link` - Individual link
- `.sidebar-link.active` - Current page
- `.link-icon` - Icon (emoji or SVG)
- `.link-text` - Link label
- `.sidebar-overlay` - Mobile dimmed overlay

### States
- `.open` - Sidebar open (mobile)
- `.active` - Sidebar overlay visible
- `[aria-expanded="true"]` - Hamburger open state

## Typography Sizes

### Desktop
```css
h1: 1.75rem   (28px at 16px base)
h2: 1.4rem    (22.4px)
h3: 1.15rem   (18.4px)
p:  0.95rem   (15.2px)
body: 15px
```

### Mobile
```css
h1: 1.5rem    (24px at 16px base)
h2: 1.25rem   (20px)
h3: 1.05rem   (16.8px)
p:  0.9rem    (14.4px)
body: 15px
```

## Border & Spacing

```css
Border: 1px solid rgba(0, 0, 0, 0.25)  /* Darker, more visible */
Border radius: 14px
Gap between areas: 16px (desktop), 12px (mobile)
Shell padding: 16px (desktop), 12px (mobile)
```

## Colors

```css
Page background: #e8e8ed       /* Light gray */
Section background: #fff       /* White */
Text: #111                     /* Near black */
Borders: rgba(0, 0, 0, 0.25)  /* Dark gray */
Hover: rgba(0, 0, 0, 0.05)    /* Very light gray */
Active: rgba(0, 0, 0, 0.1)    /* Light gray */
Overlay: rgba(0, 0, 0, 0.5)   /* 50% black */
```

## JavaScript Events

```javascript
// Hamburger click
hamburger.click() ? toggleSidebar()

// Overlay click
overlay.click() ? closeSidebar()

// ESC key
document.keydown(ESC) ? closeSidebar()

// Sidebar link click
sidebarLink.click() ? closeSidebar() + navigate

// Window resize > 800px
window.resize() ? closeSidebar() (if open)
```

## Customization Examples

### Change breakpoint
```css
/* Change 800px to your preferred value */
@media (max-width: 900px) { ... }
```

### Resize mail icon
```css
.icon-mail {
    width: 24px;   /* Increase from 20px */
    height: 24px;
}
```

### Adjust sidebar width
```css
:root {
    --sidebar-width: 260px;  /* Desktop */
}

@media (max-width: 800px) {
    .app-sidebar {
        width: 300px;  /* Mobile off-canvas */
    }
}
```

### Change border strength
```css
:root {
    --border-color: rgba(0, 0, 0, 0.3);  /* Even darker */
}
```
