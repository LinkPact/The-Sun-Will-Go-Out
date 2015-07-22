using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class HomingMissile : PlayerBullet
    {

        float ignoreHomingTime;

        public HomingMissile(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //ignoreHomingTime = 0;

            DegreeChange = 4;
            SightRange = 1000;
            FollowObject = null;

            //Egenskaper
            Speed = 0.35f;
            IsKilled = false;
            Damage = 150;
            ObjectClass = "bullet";
            Duration = 2000;

            ignoreHomingTime = 0;    

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(42, 0, 7, 22)));
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(49, 0, 7, 22)));

            Bounding = new Rectangle(42, 0, 7, 22);

            BoundingSpace = 0;

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            if (FollowObject != null)
            {
                if (!Game.stateManager.shooterState.gameObjects.Contains(FollowObject))
                    FollowObject = null;
            }

            //ignoreHomingTime -= gameTime.ElapsedGameTime.Milliseconds;

            if (FollowObject == null && ignoreHomingTime <= 0)
                LocateHomingTarget();

            if (FollowObject != null)
                Homing(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI / 2), 
                CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }

        private void LocateHomingTarget()
        {
            GameObjectVertical tempTarget = null;
            float tempDistance = SightRange;

            if (Game.stateManager.shooterState.gameObjects != null)
            {
                foreach (GameObjectVertical obj in Game.stateManager.shooterState.gameObjects)
                {
                    if (obj.ObjectClass == "enemy" && !(obj is Meteorite) && MathFunctions.ObjectDistance(this, obj) < tempDistance
                         && MathFunctions.ObjectDistance(this, obj) < SightRange)
                    {
                        tempTarget = obj;
                        tempDistance = MathFunctions.ObjectDistance(this, tempTarget);
                    }
                }
            }

            if (tempTarget != null)
            {
                FollowObject = tempTarget;
            }
        }

        private void Homing(GameTime gameTime)
        {
            if (FollowObject != null)
            {
                Direction = MathFunctions.ChangeDirection(gameTime, Direction, Position, FollowObject.Position, DegreeChange);
                Direction = MathFunctions.ScaleDirection(Direction);
            }
        }

    }
}
