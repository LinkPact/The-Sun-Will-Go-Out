using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class DestroyedShip: MissionObject
    {
        public DestroyedShip(Game1 Game, Sprite SpriteSheet) :
            base(Game, SpriteSheet)
        {
        }

        public override void Initialize()
        {
            name = "Destroyed Ship";

            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 3, 71, 37));
            scale = 1f;
            position = new Vector2(82550, 118100);
            color = Color.White;
            layerDepth = 0.5f;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            angle += (float)Math.PI * 0.005f / 180;
            position.X += 0.015f;
            position.Y += 0.025f;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}