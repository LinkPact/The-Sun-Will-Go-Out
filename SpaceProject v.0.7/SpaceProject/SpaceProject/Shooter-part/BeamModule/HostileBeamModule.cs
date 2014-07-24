using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    /**
     * Hostile subclass to BeamModule.
     * Used for beams targeting friendly objects.
     * Check BeamModule for more information.
     */
    class HostileBeamModule : BeamModule
    {
        public HostileBeamModule(Game1 game, Sprite spriteSheet, float damage)
            : base(game, spriteSheet, false, damage)
        {
            viableTargetTypes.Add("player");
            viableTargetTypes.Add("ally");

            color = Color.GreenYellow;
        }
    }
}
