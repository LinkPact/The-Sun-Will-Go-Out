using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    //Represent homing enemy bullet
    public class EnemyHomingMissileBullet : EnemyBullet
    {
        private PlayerVerticalShooter player;

        public EnemyHomingMissileBullet(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet)
        {
            this.player = player;
        }

        public override void Initialize()
        {
            base.Initialize();

            //Egenskaper
            Speed = 0.5f;
            IsKilled = false;
            Damage = 60;
            ObjectClass = "enemyBullet";
            Duration = 3000;
            TurningSpeed = 2f;

            Follows = true;
            FollowObjectTypes.Add("player");
            FollowObjectTypes.Add("ally");

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(42, 0, 7, 22)));
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(49, 0, 7, 22)));

            Bounding = new Rectangle((int)PositionX, (int)PositionY, anim.CurrentFrame.Width, anim.CurrentFrame.Height);

            BoundingSpace = 0;

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

            SightRange = 200;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI / 2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }
    }
}