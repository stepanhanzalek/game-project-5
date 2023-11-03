﻿using System;
using Microsoft.Xna.Framework;

namespace WrongHole.Screens
{
    // Custom event argument which includes the index of the player who
    // triggered the event. This is used by the MenuEntry.Selected event.
    public class PlayerIndexEventArgs : EventArgs
    {
        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }

        public PlayerIndex PlayerIndex { get; }
    }
}
