using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    public class AllianceFighterAlly : AlliedShip
    {
        #region Standard values
        private const float StandardHP = 2000;
        private const float StandardSightRange = 400;
        private const float StandardShootingDelay = 480;
        private const float StandardMaxSpeed = 0.72f;
        private const string StandardWeapon = "basiclaser";
        private const float StandardAvoidRadius = 150;
        #endregion

        public AllianceFighterAlly(Game1 Game, Sprite SpriteSheet, PlayerVerticalShooter player) :
            base(Game, SpriteSheet, player)
        {
            HPmax = StandardHP;
            HP = HPmax;

            SightRange = StandardSightRange;
            ShootingDelay = StandardShootingDelay;
            MaxSpeed = StandardMaxSpeed;

            Weapon = StandardWeapon;
            FormationArea = new Rectangle(-1, 0, 0, 0);
            AvoidRadius = StandardAvoidRadius;
        }

        public AllianceFighterAlly(Game1 Game, Sprite SpriteSheet, PlayerVerticalShooter player, Rectangle FormationArea) :
            base(Game, SpriteSheet, player)
        {
            this.FormationArea = FormationArea;

            HPmax = StandardHP;
            HP = HPmax;

            SightRange = StandardSightRange;
            ShootingDelay = StandardShootingDelay;
            MaxSpeed = StandardMaxSpeed;

            Weapon = StandardWeapon;
            AvoidRadius = StandardAvoidRadius;
        }

        public AllianceFighterAlly(Game1 Game, Sprite SpriteSheet, PlayerVerticalShooter player, Rectangle? FormationArea, string Weapon, float AvoidRadius, float HP,
            float SightRange, float ShootingDelay, float MaxSpeed) :
            base(Game, SpriteSheet, player)
        {
            if (FormationArea != null) { this.FormationArea = (Rectangle)FormationArea; }
            else { this.FormationArea = new Rectangle(-1, 0, 0, 0); }

            this.Weapon = Weapon;
            this.AvoidRadius = AvoidRadius;

            HPmax = HP;
            this.HP = HPmax;

            this.SightRange = SightRange;
            this.ShootingDelay = ShootingDelay;
            this.MaxSpeed = MaxSpeed;
        }

        public override void Initialize()
        {
            base.Initialize();

            Damage = 20;

            ObjectName = "FighterAlly";
            movement = Movement.AI;

            Acceleration = 0.024f;
            Decceleration = 0.016f;

            DirectionX = 1;
            DirectionY = 1;

            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(49, 201, 23, 27)));
            CenterPoint = new Vector2(anim.CurrentFrame.SourceRectangle.Value.Width / 2,
                                      anim.CurrentFrame.SourceRectangle.Value.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (aI.Target != null && aI.Behaviour.AIAction == AIAction.Attack)
                spriteBatch.Draw(spriteSheet.Texture, new Vector2(aI.Target.PositionX - 2, aI.Target.BoundingY - 3),
                    new Rectangle(50,24,5,3), Color.Red, 0.0f, Vector2.Zero, 1f,
                    SpriteEffects.None, 1f);

            base.Draw(spriteBatch);
        }

        public override void OnKilled()
        {
            if (aI.Target != null)
            {
                if (Behaviour.IgnoreList.Contains(aI.Target))
                {
                    Behaviour.GarbageIgnoreList.Add(aI.Target);
                    Behaviour.UpdateIgnoreList();
                }

                aI.Target = null;
            }
        }

        public override void Shoot()
        {
            if (LastTimeShot > ShootingDelay)
            {
                BasicLaser laser1 = new BasicLaser(Game, spriteSheet);
                laser1.PositionX = PositionX - 4;
                laser1.PositionY = PositionY;
                laser1.Direction = new Vector2(0, -1.0f);
                laser1.Initialize();
                laser1.Tracker = Tracker.Ally;
                laser1.Duration = 500;

                BasicLaser laser2 = new BasicLaser(Game, spriteSheet);
                laser2.PositionX = PositionX + 4;
                laser2.PositionY = PositionY;
                laser2.Direction = new Vector2(0, -1.0f);
                laser2.Initialize();
                laser2.Tracker = Tracker.Ally;
                laser2.Duration = 500;

                Game.stateManager.shooterState.gameObjects.Add(laser1);
                Game.stateManager.shooterState.gameObjects.Add(laser2);

                LastTimeShot = 0;
            }
        }

        public override void CreateAI(AIBehaviour behaviour)
        {
            base.CreateAI(behaviour);
        }
    }
}
