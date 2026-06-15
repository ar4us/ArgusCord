/*
 * ArgusCord, a modification for Discord's desktop app
 * Copyright (c) 2023 Vendicated and contributors
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

export const enum IpcEvents {
    INIT_FILE_WATCHERS = "ArgusCordInitFileWatchers",

    OPEN_QUICKCSS = "ArgusCordOpenQuickCss",
    GET_QUICK_CSS = "ArgusCordGetQuickCss",
    SET_QUICK_CSS = "ArgusCordSetQuickCss",
    QUICK_CSS_UPDATE = "ArgusCordQuickCssUpdate",

    GET_SETTINGS = "ArgusCordGetSettings",
    SET_SETTINGS = "ArgusCordSetSettings",

    GET_THEMES_LIST = "ArgusCordGetThemesList",
    GET_THEME_DATA = "ArgusCordGetThemeData",
    GET_THEME_SYSTEM_VALUES = "ArgusCordGetThemeSystemValues",
    THEME_UPDATE = "ArgusCordThemeUpdate",

    OPEN_EXTERNAL = "ArgusCordOpenExternal",
    OPEN_THEMES_FOLDER = "ArgusCordOpenThemesFolder",
    OPEN_SETTINGS_FOLDER = "ArgusCordOpenSettingsFolder",

    GET_UPDATES = "ArgusCordGetUpdates",
    GET_REPO = "ArgusCordGetRepo",
    UPDATE = "ArgusCordUpdate",
    BUILD = "ArgusCordBuild",

    OPEN_MONACO_EDITOR = "ArgusCordOpenMonacoEditor",
    GET_MONACO_THEME = "ArgusCordGetMonacoTheme",

    GET_PLUGIN_IPC_METHOD_MAP = "ArgusCordGetPluginIpcMethodMap",

    CSP_IS_DOMAIN_ALLOWED = "ArgusCordCspIsDomainAllowed",
    CSP_REMOVE_OVERRIDE = "ArgusCordCspRemoveOverride",
    CSP_REQUEST_ADD_OVERRIDE = "ArgusCordCspRequestAddOverride",

    GET_RENDERER_CSS = "ArgusCordGetRendererCss",
    RENDERER_CSS_UPDATE = "ArgusCordRendererCssUpdate",
    PRELOAD_GET_RENDERER_JS = "ArgusCordPreloadGetRendererJs",

    SUPPORTS_WINDOWS_MATERIAL = "ArgusCordSupportsWindowsMaterial",
}
