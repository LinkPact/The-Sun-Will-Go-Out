using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    public class YellowBullet : PlayerBullet
    {
        public YellowBullet(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //Egenskaper
            Speed = 0.8f;
            IsKilled = false;
            Damage = 15;
            ObjectClass = "bullet";
            Duration = 180;

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(6, 27, 4, 4)));

            Bounding = new Rectangle((int)PositionX, (int)PositionY, anim.CurrentFrame.Width, anim.CurrentFrame.Height);

            BoundingSpace = 0;

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI / 2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);

            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI / 2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }
    }
}