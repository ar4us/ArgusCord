<p align="center">
  <img src="../assets/logo.png" alt="ArgusCord Logo" width="120px" />
</p>

<h1 align="center">🖥️ ArgusCord Installer</h1>

<p align="center">
  <strong>A native Windows C# WPF GUI installer that patches your local Discord client to inject ArgusCord.</strong>
</p>

---

## ⚙️ Features

* **🎨 Glassmorphic Midnight UI:** Matches the Midnight Violet design language of the client.
* **📦 Embedded Assets:** Bundles the client source (`patcher.js`, `preload.js`, `renderer.js`, `renderer.css`) inside the executable assembly. Works 100% offline.
* **🔄 Version Selection:** Automatically scans your local appdata directories for Discord versions (Stable, PTB, Canary) and supports one-click install/uninstall.
* **🛡️ Self-Contained Binary:** Publishes to a single file with native runtime libraries included (`<IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>`), ensuring it runs on any Windows x64 system without pre-installing .NET.

---

## 🛠️ Development & Compilation

To build and compile the installer:

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* Windows OS (since it is a WPF application)

### Re-embedding Client Assets
Before building the installer, make sure to copy the compiled client assets from the `ArgusCord/dist/` directory into `ArgusCordInstaller/Resources/`:
```powershell
Copy-Item -Path "../ArgusCord/dist/patcher.js", "../ArgusCord/dist/preload.js", "../ArgusCord/dist/renderer.js", "../ArgusCord/dist/renderer.css" -Destination "Resources" -Force
```

### Build & Publish Single-File Release
Run the following command to compile and publish the final self-contained standalone `.exe`:
```bash
dotnet publish -c Release -r win-x64
```
The compiled single-file binary will be generated under:
`bin/Release/net8.0-windows/win-x64/publish/ArgusCordInstallerCore.exe`

---

## 📜 License

This installer is open-source and released under the **GPL-3.0 License**.
