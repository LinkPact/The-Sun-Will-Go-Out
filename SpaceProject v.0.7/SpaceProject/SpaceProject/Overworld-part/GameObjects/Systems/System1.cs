using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class System1: GameObjectOverworld
    {
        public System1(Game1 Game, Sprite spriteSheet):
            base(Game, spriteSheet)
        {
            this.spriteSheet = spriteSheet;
        }

        public override void Initialize()
        {
            Class = "System";
            name = "system1";
            layerDepth = 0.4f;
            position = new Vector2(300, 500);
            scale = 1.0f;
            color = Color.White;
            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 10, 479, 358));
            centerPoint = new Vector2(sprite.SourceRectangle.Value.Width / 2, sprite.SourceRectangle.Value.Height / 2);
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsUsed == true)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
