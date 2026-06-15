/*
 * ArgusCord, a Discord client mod
 * Copyright (c) 2025 ar4us and contributors
 * SPDX-License-Identifier: GPL-3.0-or-later
 */

/// <reference types="standalone-electron-types"/>

declare module "~pluginNatives" {
    const pluginNatives: Record<string, Record<string, (event: Electron.IpcMainInvokeEvent, ...args: unknown[]) => unknown>>;
    export default pluginNatives;
}
