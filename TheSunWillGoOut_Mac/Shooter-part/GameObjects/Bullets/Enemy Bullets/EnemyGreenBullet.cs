using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    //Objekt som representerar en av spelarens magier
    public class EnemyGreenBullet : EnemyBullet
    {

        //public int duration;

        public EnemyGreenBullet(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //Egenskaper
            Speed = 0.4f;
            IsKilled = false;
            Damage = 40;
            ObjectClass = "enemyBullet";
            Duration = 1000;

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(11, 24, 5, 5)));

            //Bounding = new Rectangle(0, 24, 2, 8);
            Bounding = new Rectangle((int)PositionX, (int)PositionY, anim.CurrentFrame.Width, anim.CurrentFrame.Height);

            BoundingSpace = 0;

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI / 2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }
    }
}