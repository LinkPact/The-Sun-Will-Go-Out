using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    //Represent homing enemy bullet
    class AllianceHomingBulletEnemy : EnemyShip
    {
        public int duration;
     
        public AllianceHomingBulletEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        private void Setup()
        {
            fraction = Fraction.alliance;
        }

        public override void Initialize()
        {
            base.Initialize();

            //Egenskaper
            HP = 600;
            Speed = 0.2f;
            IsKilled = false;
            Damage = 90;
            ObjectClass = "enemyBullet";
            duration = 3000;
            
            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(11, 24, 5, 5)));

            //Bounding = new Rectangle(0, 24, 2, 8);
            Bounding = new Rectangle((int)PositionX, (int)PositionY, anim.CurrentFrame.Width, anim.CurrentFrame.Height);

            BoundingSpace = 0;

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

            SightRange = 10000;
            FollowObject = player;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (FollowObject != null)
            {
                UpdateFollowObject();
            }

            duration -= gameTime.ElapsedGameTime.Milliseconds;

            if (duration <= 0)
                IsKilled = true;
        }

        private void UpdateFollowObject()
        {
            Direction = ChangeDirection(Direction, Position, player.Position, 1);
            Direction = GlobalFunctions.ScaleDirection(Direction);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI / 2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }
    }
}