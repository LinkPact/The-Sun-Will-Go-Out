using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class AllianceFleet: MissionObject
    {
        public AllianceFleet(Game1 Game, Sprite SpriteSheet) :
            base(Game, SpriteSheet)
        {
        }

        public override void Initialize()
        {
            name = "Alliance Fleet";

            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 3, 71, 37));
            scale = 1f;
            position = new Vector2(45000, 43000);
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