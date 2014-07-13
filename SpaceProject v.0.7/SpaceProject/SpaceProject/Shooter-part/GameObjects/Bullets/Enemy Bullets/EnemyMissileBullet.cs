using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    //Represent homing enemy bullet
    public class EnemyMissileBullet : EnemyBullet
    {
        public EnemyMissileBullet(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //Egenskaper
            Speed = 0.5f;
            IsKilled = false;
            Damage = 60;
            ObjectClass = "enemyBullet";
            duration = 3000;
            //TurningSpeed = 2f;

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(42, 0, 7, 22)));
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(49, 0, 7, 22)));

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