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

        public int duration;
        private PlayerVerticalShooter player;

        //private float turningSpeed;

        public EnemyHomingMissileBullet(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet)
        {
            this.player = player;
        }

        public override void Initialize()
        {
            base.Initialize();

            //Egenskaper
            Speed = 0.6f;
            IsKilled = false;
            Damage = 40;
            ObjectClass = "enemyBullet";
            duration = 3000;
            TurningSpeed = 2f;

            Follows = true;
            FollowObjectTypes.Add("player");
            FollowObjectTypes.Add("ally");

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(42, 0, 7, 22)));
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(49, 0, 7, 22)));

            //Bounding = new Rectangle(0, 24, 2, 8);
            Bounding = new Rectangle((int)PositionX, (int)PositionY, anim.CurrentFrame.Width, anim.CurrentFrame.Height);

            BoundingSpace = 0;

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

            SightRange = 200;
            //FollowObject = player;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            duration -= gameTime.ElapsedGameTime.Milliseconds;

            if (duration <= 0)
                IsKilled = true;
        }

        //private void UpdateFollowObject()
        //{
        //    Direction = ChangeDirection(Direction, Position, player.Position, turningSpeed);
        //    Direction = GlobalFunctions.ScaleDirection(Direction);
        //}

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI / 2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }
    }
}