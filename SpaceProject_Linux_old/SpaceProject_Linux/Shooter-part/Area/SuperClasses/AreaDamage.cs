using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    public enum AreaDamageType
    {
        player,
        enemy
    }

    public abstract class AreaDamage : AreaObject
    {
        private AreaDamageType type;
        public AreaDamageType Type { get { return type; } private set { } }

        public AreaDamage(Game1 game, AreaDamageType type, Vector2 position, float damage)
            : base(game, position)
        {
            this.type = type;

            ObjectClass = "areadamage";
            ObjectName = "areadamage";

            this.Damage = damage;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            IsKilled = true;
        }

        public override void OnKilled()
        { }
    }
}
