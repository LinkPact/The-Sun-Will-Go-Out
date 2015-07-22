using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class AlliedShip : VerticalShooterShip
    {
        protected AI aI;
        public AI AI { get { return aI; } set { aI = value; } }

        protected float Acceleration;
        protected float Decceleration;
        protected float MaxSpeed;

        public static bool ShowSightRange;
        private Sprite SightRangeSprite;
        private float SightRangeScale;

        protected float LastTimeShot;
        protected float ShootingDelay;

        protected string Weapon;
        protected Rectangle FormationArea;
        protected float AvoidRadius;

        private SoundEffects shootSound = SoundEffects.SmallLaser;

        protected AlliedShip(Game1 Game, Sprite SpriteSheet, PlayerVerticalShooter player):
            base(Game, SpriteSheet, player)
        { 
        }

        public override void Initialize()
        {
            base.Initialize();

            ObjectClass = "ally";

            LastTimeShot = ShootingDelay;

            SightRangeSprite = new Sprite(spriteSheet.Texture, new Rectangle(2, 272, 100, 100));
            SightRangeScale = SightRange / (SightRangeSprite.SourceRectangle.Value.Width / 2);

            angle = (float)(Math.PI / 180) * 180;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsKilled)
            {
                LastTimeShot += gameTime.ElapsedGameTime.Milliseconds;

                aI.Process(gameTime);
                UpdateMovement(gameTime);

                base.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsKilled)
            {
                if(ShowSightRange)
                    spriteBatch.Draw(SightRangeSprite.Texture, Position, SightRangeSprite.SourceRectangle, Color.Green,
                        0.0f, new Vector2(SightRangeSprite.SourceRectangle.Value.Width / 2, SightRangeSprite.SourceRectangle.Value.Height / 2),
                        SightRangeScale, SpriteEffects.None, 1f);

                base.Draw(spriteBatch);
            }
        }

        public virtual void Shoot()
        {
            Game.soundEffectsManager.PlaySoundEffect(shootSound);
        }

        public override bool CheckOutside()
        {
            return false;
        }

        private void CheckCollisionsEdges()
        {
            //Kollision mot kanterna
            if (PositionX - anim.Width / 2 <= relativeOrigin)
                PositionX = relativeOrigin + anim.Width / 2;

            if (PositionX + anim.Width / 2 > relativeOrigin + LevelWidth)
                PositionX = relativeOrigin + LevelWidth - anim.Width / 2;

            if (PositionY - anim.Height / 2 <= 0)
                PositionY = anim.Height / 2;

            if (PositionY + anim.Height / 2 > windowHeight)
                PositionY = windowHeight - anim.Height / 2;

        }

        public virtual void CreateAI(AIBehaviour behaviour)
        {
            aI = new AI(Game, spriteSheet, this, behaviour, Weapon, AvoidRadius, FormationArea);
        }

        public void SetFormationArea(Rectangle formationArea)
        {
            this.FormationArea = formationArea;
        }

        #region Movement

        public void Move(Vector2 dir)
        {
            if (dir != this.Direction)
            {
                aI.Deccelerate = true;

                if (Speed < 0.05f && Speed > -0.05f)
                    this.Direction = dir;
            }

            else
                aI.Accelerate = true;
        }

        public void Stop()
        {
            if (this.Direction != Vector2.Zero)
            {
                aI.Deccelerate = true;

                if (Speed < 0.05f && Speed > -0.05f)
                    this.Direction = Vector2.Zero;
            }
        }

        public void Stop(string xOrY)
        {
            switch (xOrY.ToLower())
            {
                case "x":
                    {
                        if (this.DirectionX != 0)
                        {
                            aI.Deccelerate = true;

                            if (Speed < 0.05f && Speed > -0.05f)
                                this.DirectionX = 0;
                        }

                        break;
                    }

                case "y":
                    {
                        if (this.DirectionY != 0)
                        {
                            aI.Deccelerate = true;

                            if (Speed < 0.05f && Speed > -0.05f)
                                this.DirectionY = 0;
                        }

                        break;
                    }
            }
        }

        public void MoveToArea(Rectangle Area)
        {
            if (BoundingX > 20 &&
                BoundingX > Area.X + Area.Width)
                Move(new Vector2(-1, 0));

            else if (BoundingX + BoundingWidth < Game1.ScreenSize.X - 20 &&
                BoundingX + BoundingWidth < Area.X)
                Move(new Vector2(1, 0));

            if (BoundingX + BoundingWidth > Area.X &&
                BoundingX < Area.X + Area.Width)
            {
                if (BoundingY > 20 &&
                    BoundingY > Area.Y + Area.Height)
                    Move(new Vector2(0, -1));

                else if (BoundingY + BoundingHeight < Game1.ScreenSize.Y - 20 &&
                    BoundingY + BoundingHeight < Area.Y)
                    Move(new Vector2(0, 1));
            }
        }

        public void UpdateMovement(GameTime gameTime)
        {
            if (aI.Accelerate)
            {
                Speed += Acceleration * MathFunctions.FPSSyncFactor(gameTime);
                aI.Accelerate = false;
            }

            else if (aI.Deccelerate)
            {
                Speed -= Acceleration * MathFunctions.FPSSyncFactor(gameTime);
                aI.Deccelerate = false;
            }

            if (Speed > 0)
                Speed -= Decceleration * MathFunctions.FPSSyncFactor(gameTime);

            else if (Speed < 0)
                Speed += Decceleration * MathFunctions.FPSSyncFactor(gameTime);

            if (Speed > MaxSpeed)
                Speed = MaxSpeed;

            else if (Speed < -MaxSpeed)
                Speed = -MaxSpeed;

            if (PositionX + CenterPointX > Game1.ScreenSize.X)
            {
                Speed = 0;
                PositionX = (Game1.ScreenSize.X - CenterPointX) - 1;
            }

            else if (PositionX - CenterPointX < 0)
            {
                Speed = 0;
                PositionX = CenterPointX + 1;
            }

            if (PositionY + CenterPointY > Game1.ScreenSize.Y)
            {
                Speed = 0;
                PositionY = (Game1.ScreenSize.Y - CenterPointY) - 1;
            }

            else if (PositionY - CenterPointY < 0)
            {
                Speed = 0;
                PositionY = CenterPointY + 1;
            }

            CheckCollisionsEdges();
        }
        #endregion
    }
}
