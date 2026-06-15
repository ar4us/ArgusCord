<p align="center">
  <img src="../assets/logo.png" alt="ArgusCord Logo" width="120px" />
</p>

<h1 align="center">🔮 ArgusCord Client</h1>

<p align="center">
  <strong>The client-side modification codebase for ArgusCord, written in TypeScript and React.</strong>
</p>

---

## 🚀 Development

ArgusCord uses `pnpm` for workspace package management.

### Prerequisites

* [Node.js](https://nodejs.org/) (>= 18)
* [pnpm](https://pnpm.io/)

### Installation

To install all workspace dependencies:
```bash
pnpm install
```

### Build

To compile the client files into `dist/`:
```bash
pnpm build
```

### Local Injection (For development)

To inject your local dev build of ArgusCord directly into your Discord client:
```bash
pnpm inject
```

To uninject:
```bash
pnpm uninject
```

---

## 📁 Codebase Layout

* `src/`: Core TypeScript source files.
  * `src/ArgusCord.ts`: Main entry point loading the mod, custom default theme, and plugins.
  * `src/plugins/`: 100+ toggleable user plugins.
  * `src/components/`: Settings and settings tab UI.
* `packages/`: Local workspace helper packages.
  * `packages/discord-types`: TypeScript typings for Discord's internal Webpack modules.
* `dist/`: Output folder for compiled JavaScript/CSS bundles.

---

## 📜 License

This client mod is open-source and released under the **GPL-3.0 License**.
