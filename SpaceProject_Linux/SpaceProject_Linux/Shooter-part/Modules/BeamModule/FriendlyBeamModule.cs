using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    /**
    * Friendly subclass to BeamModule.
    * Used by beam targeting hostile objects.
    * Check BeamModule for more information.
    */
    class FriendlyBeamModule : BeamModule
    {
        public FriendlyBeamModule(Game1 game, Sprite spriteSheet, float damage, Color color)
            : base(game, spriteSheet, true, damage)
        {
            viableTargetTypes.Add("enemy");
            this.color = color;
        }
    }
}
