using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    public abstract class CombatGameObject : AnimatedGameObject
    {

        protected CombatGameObject(Game1 Game, Sprite spriteSheet)
            : base(Game, spriteSheet)
        { }

        public abstract void InflictDamage(GameObjectVertical obj);
    }
}
