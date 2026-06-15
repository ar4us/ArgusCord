# ArgusCord Client Modification

ArgusCord is a fast, lightweight, next-generation client modification for Discord designed for performance, customization, and privacy.

---

## Features

*   **Fast & Lightweight:** Optimized codebase that runs smoothly alongside Discord.
*   **100+ Built-in Plugins:** Toggle utility plugins instantly (Spotify Controls, deleted message logs, volume booster, and more).
*   **Theme Support:** Built-in CSS editor with support for custom CSS themes and layouts.
*   **Privacy-Friendly:** Blocks Discord's internal tracking, telemetry, and crash reports.
*   **Local Settings Sync:** Save and load your configurations easily.

---

## Development

ArgusCord uses `pnpm` for package management.

### Prerequisites

*   [Node.js](https://nodejs.org/) (>= 18)
*   [pnpm](https://pnpm.io/)

### Installation

Install dependencies:
```bash
pnpm install
```

### Build

Compile the client code:
```bash
pnpm build
```

The compiled output will be available in the `dist/` directory.

### Local Injection

To inject ArgusCord into your local Discord client:
```bash
pnpm inject
```

To uninject:
```bash
pnpm uninject
```

---

## License

ArgusCord is open-source software licensed under the GPL-3.0 License.
