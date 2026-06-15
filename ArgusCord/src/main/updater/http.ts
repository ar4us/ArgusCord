/*
 * ArgusCord, a modification for Discord's desktop app
 * Copyright (c) 2022 ar4us and contributors
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

import { fetchBuffer, fetchJson } from "@main/utils/http";
import { IpcEvents } from "@shared/IpcEvents";
import { ARGUSCORD_USER_AGENT } from "@shared/arguscordUserAgent";
import { ipcMain } from "electron";
import { writeFile } from "fs/promises";
import { join } from "path";

import gitHash from "~git-hash";
import gitRemote from "~git-remote";

import { serializeErrors } from "./common";

const API_BASE = `https://api.github.com/repos/${gitRemote}`;
let PendingUpdates = [] as [string, string][];

async function githubGet<T = any>(endpoint: string) {
    return fetchJson<T>(API_BASE + endpoint, {
        headers: {
            Accept: "application/vnd.github+json",
            "User-Agent": ARGUSCORD_USER_AGENT
        }
    });
}

async function calculateGitChanges() {
    try {
        const isOutdated = await fetchUpdates();
        if (!isOutdated) return [];

        const data = await githubGet(`/compare/${gitHash}...builds`);

        return data.commits.map((c: any) => ({
            hash: c.sha.slice(0, 7),
            author: c.author?.login || c.commit.author.name,
            message: c.commit.message.split("\n")[0]
        }));
    } catch (e) {
        console.error("[ArgusCord Updater] Failed to calculate changes:", e);
        return [];
    }
}

async function fetchUpdates() {
    try {
        const data = await githubGet("/commits/builds");
        if (!data || !data.sha) {
            console.error("[ArgusCord Updater] Failed to get latest builds commit. Response:", data);
            return false;
        }

        const latestHash = data.sha.slice(0, 7);
        if (latestHash === gitHash) {
            return false;
        }

        PendingUpdates = [
            ["patcher.js", `https://raw.githubusercontent.com/${gitRemote}/builds/patcher.js`],
            ["preload.js", `https://raw.githubusercontent.com/${gitRemote}/builds/preload.js`],
            ["renderer.js", `https://raw.githubusercontent.com/${gitRemote}/builds/renderer.js`],
            ["renderer.css", `https://raw.githubusercontent.com/${gitRemote}/builds/renderer.css`]
        ];

        return true;
    } catch (e) {
        console.error("[ArgusCord Updater] Failed to check updates:", e);
        return false;
    }
}

async function applyUpdates() {
    if (PendingUpdates.length === 0) return false;

    const fileContents = await Promise.all(PendingUpdates.map(async ([name, url]) => {
        const contents = await fetchBuffer(url);
        return [join(__dirname, name), contents] as const;
    }));

    await Promise.all(fileContents.map(async ([filename, contents]) =>
        writeFile(filename, contents))
    );

    PendingUpdates = [];
    return true;
}

ipcMain.handle(IpcEvents.GET_REPO, serializeErrors(() => `https://github.com/${gitRemote}`));
ipcMain.handle(IpcEvents.GET_UPDATES, serializeErrors(calculateGitChanges));
ipcMain.handle(IpcEvents.UPDATE, serializeErrors(fetchUpdates));
ipcMain.handle(IpcEvents.BUILD, serializeErrors(applyUpdates));
