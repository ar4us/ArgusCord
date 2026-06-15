/*
 * ArgusCord, a Discord client mod
 * Copyright (c) 2025 ar4us and contributors
 * SPDX-License-Identifier: GPL-3.0-or-later
 */

import { Command } from "@arguscord/discord-types";
export { ApplicationCommandInputType, ApplicationCommandOptionType, ApplicationCommandType } from "@arguscord/discord-types/enums";

export interface ArgusCordCommand extends Command {
    isArgusCordCommand?: boolean;
}
