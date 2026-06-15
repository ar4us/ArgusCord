/*
 * ArgusCord, a Discord client mod
 * Copyright (c) 2024 ar4us and contributors
 * SPDX-License-Identifier: GPL-3.0-or-later
 */

import { IpcMainInvokeEvent } from "electron";

export function initDevtoolsOpenEagerLoad(e: IpcMainInvokeEvent) {
    const handleDevtoolsOpened = () => e.sender.executeJavaScript("ArgusCord.Plugins.plugins.ConsoleShortcuts.eagerLoad(true)");

    if (e.sender.isDevToolsOpened())
        handleDevtoolsOpened();
    else
        e.sender.once("devtools-opened", () => handleDevtoolsOpened());
}
