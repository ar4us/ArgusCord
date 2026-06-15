/*
 * ArgusCord, a Discord client mod
 * Copyright (c) 2025 ar4us and contributors
 * SPDX-License-Identifier: GPL-3.0-or-later
 */

import { Tooltip } from "@arguscord/discord-types";

const NOOP = () => { };

/** Don't use this */
export const TooltipFallback: Tooltip = ({ children }) => {
    if (typeof children !== "function") {
        return null;
    }

    const node = children({
        onBlur: NOOP,
        onFocus: NOOP,
        onMouseEnter: NOOP,
        onMouseLeave: NOOP,
        onClick: NOOP,
        onContextMenu: NOOP
    });

    return <>{node}</>;
};

TooltipFallback.Colors = {} as any;
