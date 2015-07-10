using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    //Object which represents one of the players primary weapons
    public class FlameShot : PlayerBullet
    {
        public FlameShot(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        { }

        public override void Initialize()
        {
            base.Initialize();

            Speed = 0.8f;
            IsKilled = false;
            Damage = 20;
            ObjectClass = "bullet";
            Duration = 500;

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(42, 24, 5, 9)));

            Bounding = new Rectangle(42, 24, 5, 9);

            BoundingSpace = 0;

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI/2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }
    }
}
