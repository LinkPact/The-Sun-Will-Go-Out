using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public enum AreaDamageType
    {
        player,
        enemy
    }

    public abstract class AreaDamage : GameObjectVertical
    {
        private AreaDamageType type;
        public AreaDamageType Type { get { return type; } private set { } }

        protected Vector2 position;

        public AreaDamage(Game1 game, AreaDamageType type, Vector2 position)
            : base(game)
        {
            this.type = type;
            this.position = position;

            ObjectClass = "areadamage";
            ObjectName = "areadamage";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            IsKilled = true;
        }

        public abstract Boolean IsOverlapping(AnimatedGameObject obj);
    }
}
