using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class AmbushSpot: MissionObject
    {
        public AmbushSpot(Game1 Game, Sprite SpriteSheet) :
            base(Game, SpriteSheet)
        {
        }

        public override void Initialize()
        {
            name = "Ambush spot";

            sprite = spriteSheet.GetSubSprite(new Rectangle(73, 2, 58, 39));
            scale = 1f;
            position = new Vector2(92500, 105000);
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