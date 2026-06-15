# ArgusCord

ArgusCord is a premium, lightweight, next-generation client modification for Discord designed to enhance performance, customization, and privacy.

This repository contains the complete source code for the ArgusCord ecosystem:
* **`/ArgusCord`**: The client-side modification code (TypeScript/React) featuring 100+ built-in plugins, a premium default dark neon-violet theme, and telemetry-blocking functionality.
* **`/ArgusCordInstaller`**: The native Windows C# WPF GUI installer which patches the local Discord app.asar to inject ArgusCord.
* **`/website`**: A bilingual (Arabic/English) landing page website ready for static hosting.

---

## Repository Structure

```
├── ArgusCord/          # Client-side modification codebase (rebranded client)
├── ArgusCordInstaller/ # Native C# WPF GUI Installer project (Windows x64)
├── website/            # Bilingual (Arabic/English) static landing page
└── .gitignore          # Repository git ignore rules
```

---

## Features

1. **Client-Side Nitro benefits:** Locally unlock high-definition screensharing, high-fps streams, custom client emojis, and animated avatars for free.
2. **100+ Built-in Plugins:** Instantly toggle premium features such as Spotify controls, block typing indicator, deleted message logs, and view hidden channels.
3. **Premium Theme Engine:** Customize your Discord client UI with beautiful dark mode, glassmorphic layouts, and a custom CSS editor.
4. **Privacy-First:** Blocks all of Discord's internal tracking, telemetry, and analytics data.
5. **Standalone GUI Installer:** A beautiful self-contained Windows C# WPF app that automatically detects and patches your Discord installation.

---

## License

ArgusCord is open-source and released under the GPL-3.0 License.
