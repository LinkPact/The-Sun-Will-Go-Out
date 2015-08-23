using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    //Objekt som representerar en av spelarens magier
    public class GreenBullet : PlayerBullet
    {
        public GreenBullet(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //Egenskaper
            Speed = 0.7f;
            IsKilled = false;
            Damage = 30;
            ObjectClass = "bullet";
            Duration = 350;

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(11, 24, 5, 5)));

            //Bounding = new Rectangle(0, 24, 2, 8);
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
        }
    }
}