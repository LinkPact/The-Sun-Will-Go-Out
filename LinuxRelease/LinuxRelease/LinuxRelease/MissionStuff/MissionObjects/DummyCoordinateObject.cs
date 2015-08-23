using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public class DummyCoordinateObject : MissionObject
    {
        public DummyCoordinateObject(Game1 Game, Sprite SpriteSheet) :
            base(Game, SpriteSheet)
        {
        }

        public override void Initialize()
        {
            name = "DummyCoordinateObject";

            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 0, 1, 1));
            scale = 1f;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
        }
    }
}
