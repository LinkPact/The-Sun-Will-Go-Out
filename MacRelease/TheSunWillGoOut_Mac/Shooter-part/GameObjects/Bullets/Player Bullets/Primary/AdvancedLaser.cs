using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpaceProject_Mac
{
    public class AdvancedLaser : PlayerBullet
    {
        public AdvancedLaser(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        { }

        public override void Initialize()
        {
            base.Initialize();

            IsKilled = false;
            ObjectClass = "bullet";

            Damage = 70;
            Duration = 300;
            Speed = 1.1f;

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(35, 25, 5, 9)));

            Bounding = new Rectangle(29, 25, 5, 9);
            BoundingSpace = 0;
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI / 2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }
    }
}
