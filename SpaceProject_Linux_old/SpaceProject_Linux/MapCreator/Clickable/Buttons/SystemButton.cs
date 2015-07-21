using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    abstract class SystemButton : MapCreatorButton
    {
        public SystemButton(Sprite spriteSheet, Vector2 position)
            : base(spriteSheet, position)
        { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
    }
}
