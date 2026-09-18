# Dashboard Component - Visual Guide & Features

## 📊 Dashboard Layout Map

```
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃                         HEADER / TOPBAR                              ┃
┃  [Dashboard ▼]     [⚙️] [🏢] [Org▼] [?] [🔔]¹ [👤]                  ┃
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
┏━━━━━━━━┓┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃ SIDEBAR ┃┣━━━━━━━━┳━━━━━━━━┳━━━━━━━━┓                              ┃
┃         ┃┃ ORDERS ┃ ITEMS  ┃ CHART  ┃                              ┃
┃ 📊•••  ┃┃ TABLE  ┃ LIST   ┃ WIDGET ┃                              ┃
┃ 📦•••  ┃┣━━━━━━━╋━━━━━━━━╋━━━━━━━━┫                              ┃
┃ ⚙️•••  ┃┃ Order █┃ Card █ ┃ Donut█ ┃                              ┃
┃ 📋•••  ┃┃ Order █┃ Card █ ┃ Chart  ┃                              ┃
┃ 🏪•••  ┃┃ Order █┃ Card █ ┃        ┃                              ┃
┃ 🧩•••  ┃┃ Order █┃ Card █ ┃ Legend ┃                              ┃
┃ 📈•••  ┃┃        ┃ (scroll)┃ Items  ┃                              ┃
┃ 👤•••  ┃┃        ┃        ┃        ┃                              ┃
┃ ⚡•••  ┃┗━━━━━━━┻━━━━━━━━┻━━━━━━━━┛                              ┃
┃        ┃                                                            ┃
┃v0.9.2  ┃  ┌──────────────────────────────────────────────────┐   ┃
┃Build   ┃  │ [+ New Purchase Order]     [Sign out]            │   ┃
┃        ┃  └──────────────────────────────────────────────────┘   ┃
┗━━━━━━━━┛┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
```

---

## 🎨 Widget Details

### Widget 1: Latest Purchase Orders (LEFT)
**Purpose:** Display 5 most recent purchase orders

**Data Shown:**
- Order ID (formatted: ORD-2024-{id})
- Net Amount (currency, 2 decimals)
- Number of Items (quantity badge)

**Structure:**
```
┌─ WIDGET HEADER ─────────────────────┐
│ Title: "Latest Purchase Orders"     │
│                     [Today ▼]       │
├─ INFO SECTION (Blue BG) ───────────┤
│ {N} orders added                    │
├─ TABLE (Scrollable) ───────────────┤
│ ID        Amount      Items         │
│ ORD-1     Rs. 50,000  3 items      │
│ ORD-2     Rs. 45,000  2 items      │
│ ORD-3     Rs. 60,000  5 items      │
│ ...                                 │
└─────────────────────────────────────┘
```

**Interactions:**
- Hover: Row highlights with light background
- Filter: Today dropdown (expandable)
- Empty: Shows "No purchase orders yet"
- Loading: Spinner animation

---

### Widget 2: Item Details (MIDDLE)
**Purpose:** Display first 10 items added to system

**Data Shown:**
- Item Icon (📦)
- Item Name (bold, dark)
- Purchase Order ID (small, light)
- Quantity Badge (quantity qty)

**Structure:**
```
┌─ WIDGET HEADER ─────────────────────┐
│ Title: "Item Details"               │
│                     [Today ▼]       │
├─ ITEMS LIST (Scrollable) ──────────┤
│ ┌─ ITEM CARD ────────────────────┐ │
│ │ 📦 Mango                       │ │
│ │    PO #1                       │ │
│ │              5 qty             │ │
│ └────────────────────────────────┘ │
│ ┌─ ITEM CARD ────────────────────┐ │
│ │ 📦 Apple                       │ │
│ │    PO #2                       │ │
│ │              3 qty             │ │
│ └────────────────────────────────┘ │
│ ... (more cards)                   │
└─────────────────────────────────────┘
```

**Interactions:**
- Hover: Card expands shadow, background changes
- Scroll: Items list scrollable vertically
- Empty: Shows "No items added yet"
- Loading: Spinner animation

---

### Widget 3: Items Summary (RIGHT)
**Purpose:** Visualize item distribution with chart

**Data Shown:**
- Total Quantity (large number at top)
- Donut Chart (visual breakdown)
- Legend Items (each item type with count)

**Structure:**
```
┌─ WIDGET HEADER ─────────────────────┐
│ Title: "Items Summary"              │
│                     [Today ▼]       │
├─ TOTAL DISPLAY (Blue BG) ──────────┤
│                   128               │
│           Total Items               │
├─ CHART AREA ───────────────────────┤
│          ╱─────╲                   │
│        ╱         ╲                 │
│       │  DONUT   │                 │
│       │  CHART   │                 │
│       ╲         ╱                  │
│         ╲─────╱                    │
├─ LEGEND ITEMS ─────────────────────┤
│ Mango........................45     │
│ Apple........................32     │
│ Banana........................28     │
│ Orange........................15     │
│ Grapes........................8      │
└─────────────────────────────────────┘
```

**Interactions:**
- Chart: Interactive donut/pie chart
- Legend: Color-coded item counts
- Empty: Shows "No data available"
- Loading: Spinner animation

---

## 🎯 User Interactions

### Navigation Flow
```
User Login
    ↓
Dashboard (Landing Page)
    ├── View Orders
    ├── View Items
    ├── View Summary
    ├── [+ New Purchase Order] → Purchase Bill Page
    └── [Sign out] → Login Page
```

### Data Refresh
- **Auto-Refresh:** Every 30 seconds
- **On Load:** Initial data fetch
- **Error Handling:** Graceful fallback messages
- **Loading States:** Visual spinner indicators

### Button Actions
```
Primary Button: "+ New Purchase Order"
├─ Click → Navigate to /purchase-bill
└─ Style → Blue background, hover lift effect

Secondary Button: "Sign out"
├─ Click → Logout & Navigate to /login
└─ Style → White bg, red text on hover
```

---

## 🔄 Data Flow

```
Dashboard Component
    │
    ├─→ onInit()
    │   ├─→ loadDashboardData()
    │   │   ├─→ loadLatestOrders()        → API: /api/PurchaseBill/latest
    │   │   ├─→ loadOldestItems()         → API: /api/PurchaseBill/oldest-items
    │   │   └─→ loadGroupedItems()        → API: /api/PurchaseBill/grouped-items
    │   │
    │   └─→ Set interval(30s) → Refresh data
    │
    ├─→ PurchaseBillService
    │   ├─→ getLatestPurchaseOrders()     → Returns LatestPurchaseOrder[]
    │   ├─→ getOldestPurchaseOrderItems() → Returns OldestPurchaseOrderItem[]
    │   └─→ getGroupedItems()             → Returns GroupedItem[]
    │
    └─→ Template Rendering
        ├─→ Widget 1: Display latestOrders in table
        ├─→ Widget 2: Display oldestItems in cards
        └─→ Widget 3: Display groupedItems in chart
```

---

## 🎨 CSS Styling Highlights

### Key Classes
```scss
.dashboard-page      → Main container, flex layout
.sidebar            → Fixed left navigation
.main-content       → Flexible content area
.topbar             → Header bar
.widget             → Card container (reusable)
.widget-header      → Header section of cards
.orders-widget      → Table widget specific
.items-widget       → List widget specific
.chart-widget       → Chart widget specific
```

### Responsive Behavior
```
Desktop (1400px+):
  ✅ 3-column grid visible
  ✅ Sidebar expanded

Tablet (900px-1400px):
  ✅ 2-column grid
  ✅ Sidebar visible

Mobile (<900px):
  ✅ 1-column stacked
  ✅ Sidebar collapsible
```

### Animation Keyframes
```css
@keyframes spin {
  to { transform: rotate(360deg); }
}
```

**Used for:** Loading spinners (0.6s linear infinite)

---

## 📱 Mobile Experience

**Touch-Friendly Features:**
- Larger tap targets (40px minimum)
- Increased padding on interactive elements
- Swipe-scrollable card lists
- Full-width buttons on small screens
- Readable font sizes (minimum 12px)

**Performance:**
- Lazy-loaded chart
- Optimized animations (60fps)
- Debounced auto-refresh
- Efficient CSS (minimal repaints)

---

## ✨ Special Features

### 1. Real-Time Updates
- Auto-refresh every 30s without page reload
- Smooth data transitions
- Error recovery

### 2. Accessibility
- Color contrast: WCAG AA compliant
- Semantic HTML structure
- ARIA labels on icons
- Keyboard navigation support

### 3. Performance
- CSS Grid/Flexbox (no floats)
- Minimal JavaScript calculations
- Optimized re-renders
- Efficient event handling

### 4. User Experience
- Clear loading states
- Helpful empty states
- Hover feedback
- Smooth transitions
- Professional polish

---

## 🛠 Development Notes

**Framework:** Angular 18+
**Styling:** SCSS with variables
**Charts:** ngx-charts (PieChart component)
**Icons:** Unicode emoji (scalable, no font files)
**Layout:** CSS Grid + Flexbox

**Browser Support:**
- Chrome/Edge 90+
- Firefox 88+
- Safari 14+
- Mobile browsers (iOS 14+, Android 10+)

