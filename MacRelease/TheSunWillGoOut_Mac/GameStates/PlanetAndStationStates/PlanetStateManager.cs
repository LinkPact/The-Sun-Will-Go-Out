using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{
    public class PlanetStateManager : BaseStateManager
    {
        public PlanetStateManager(Game1 Game)
        {
            this.Game = Game;
            this.baseState = Game.stateManager.planetState;
        }
    }
}
