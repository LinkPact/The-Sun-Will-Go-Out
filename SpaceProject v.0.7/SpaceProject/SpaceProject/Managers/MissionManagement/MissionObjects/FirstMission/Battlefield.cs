using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class Battlefield: MissionObject
    {
        public Battlefield(Game1 Game, Sprite SpriteSheet) :
            base(Game, SpriteSheet)
        {
        }

        public override void Initialize()
        {
            name = "Battlefield";

            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 42, 301, 198));
            scale = 1f;
            position = new Vector2(56350, 20600);
            color = Color.White;
            layerDepth = 0.5f;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}