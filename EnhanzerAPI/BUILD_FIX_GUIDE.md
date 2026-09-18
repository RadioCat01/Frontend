# 🔧 Build Fix Guide - EnhanzerAPI Locked Process

## Issue

The build failed with:
```
MSB3021: Unable to copy file because it is being used by another process.
The process cannot access the file 'EnhanzerAPI.exe' because it is being used by another process.
```

**Cause:** The EnhanzerAPI application is still running (PID: 21372)

---

## Solution

### Step 1: Stop the Running Process

Open PowerShell and run:

```powershell
# Stop the EnhanzerAPI process
Stop-Process -Name "EnhanzerAPI" -Force

# Or kill by PID if you know it
Stop-Process -Id 21372 -Force
```

### Step 2: Clean Build Output

```powershell
cd D:\Assignment\EnhanzerAPI

# Clean previous build
dotnet clean

# Or manually delete:
Remove-Item -Path "bin" -Recurse -Force
Remove-Item -Path "obj" -Recurse -Force
```

### Step 3: Rebuild

```powershell
dotnet build

# Or in Visual Studio:
# Right-click solution → Clean Solution
# Right-click solution → Rebuild Solution
```

---

## How to Avoid This

### ✅ Before Building:

1. **Stop the running API server**
   - If running from command line: Press `Ctrl+C`
   - If running in Visual Studio: Stop the debugger
   - If running as background process: Kill it using PowerShell

2. **Close Visual Studio** (optional but recommended)
   - This releases all file locks

3. **Then build**

### ✅ Build Command from Terminal:
```powershell
# Full clean build
cd D:\Assignment\EnhanzerAPI
dotnet clean
dotnet build
dotnet build -c Release  # For release build
```

---

## Quick Checklist

- [ ] Stop EnhanzerAPI process (`Ctrl+C` or kill process)
- [ ] Stop Frontend dev server if running (`Ctrl+C`)
- [ ] Close all instances of the file in editors
- [ ] Run `dotnet clean`
- [ ] Run `dotnet build`
- [ ] Verify: No errors in Output window

---

## Code Changes Made (No Build Issues)

✅ **Program.cs** - Added camelCase JSON serialization
```csharp
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
	});
```

✅ **PurchaseBillController.cs** - Added error handling with try-catch
```csharp
try {
	// ... validation logic
} catch (Exception ex) {
	_logger.LogError(ex, "Error validating item");
	return StatusCode(500, new { message = "Error processing item validation" });
}
```

✅ **DTOs** - Simplified (removed redundant JsonPropertyName attributes)
- PurchaseBillItemDto.cs
- LatestPurchaseOrderDto.cs
- OldestPurchaseOrderItemDto.cs
- GroupedItemDto.cs

All DTOs remain nullable-safe with `= string.Empty` defaults.

---

## After Build Succeeds

1. **Run the API:**
```powershell
dotnet run
```

2. **In another terminal, run Frontend:**
```powershell
cd D:\Assignment\Frontend
npm start
```

3. Navigate to `http://localhost:4200` and test!

---

## Expected Result

When you try to add an item now:
- ✅ Frontend sends: `{ item: "Mango", batch: "...", standardCost: 50, ... }`
- ✅ API receives and deserializes correctly (camelCase → PascalCase)
- ✅ Validation passes
- ✅ Response returns with calculated totals
- ✅ Dashboard receives data in camelCase format
- ✅ No more 500 errors!

