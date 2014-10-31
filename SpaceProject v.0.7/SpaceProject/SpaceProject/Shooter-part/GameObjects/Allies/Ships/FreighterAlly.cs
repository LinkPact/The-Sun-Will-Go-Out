using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class FreighterAlly : AlliedShip
    {
        #region Standard values
        private readonly float STANDARD_HP = 2000;
        private readonly float STANDARD_SIGHTRANGE = 200;
        private readonly float STANDARD_SHOOTINGDELAY = 0;
        private readonly float STANDARD_MAXSPEED = 0.36f;
        private readonly string STANDARD_WEAPON = "none";
        private readonly float STANDARD_AVOID_RADIUS = 200;
        #endregion

        public FreighterAlly(Game1 Game, Sprite SpriteSheet, PlayerVerticalShooter player):
            base(Game,SpriteSheet, player)
        {
            HPmax = STANDARD_HP;
            HP = HPmax;

            SightRange = STANDARD_SIGHTRANGE;
            MaxSpeed = STANDARD_MAXSPEED;

            FormationArea = new Rectangle(-1, 0, 0, 0);
            Weapon = STANDARD_WEAPON;
            ShootingDelay = STANDARD_SHOOTINGDELAY;
            AvoidRadius = STANDARD_AVOID_RADIUS;
        }

        public FreighterAlly(Game1 Game, Sprite SpriteSheet, PlayerVerticalShooter player, Rectangle FormationArea) :
            base(Game, SpriteSheet, player)
        {
            HPmax = STANDARD_HP;
            HP = HPmax;

            SightRange = STANDARD_SIGHTRANGE;
            MaxSpeed = STANDARD_MAXSPEED;

            this.FormationArea = FormationArea;
            Weapon = STANDARD_WEAPON;
            ShootingDelay = STANDARD_SHOOTINGDELAY;
            AvoidRadius = STANDARD_AVOID_RADIUS;
        }

        public FreighterAlly(Game1 Game, Sprite SpriteSheet, PlayerVerticalShooter player, Rectangle? FormationArea, string Weapon, float AvoidRadius,
            float HP, float SightRange, float ShootingDelay, float MaxSpeed) :
            base(Game, SpriteSheet, player)
        {
            if (FormationArea != null) { this.FormationArea = (Rectangle)FormationArea; }
            else { this.FormationArea = new Rectangle(-1, 0, 0, 0); }

            this.Weapon = Weapon;
            this.ShootingDelay = ShootingDelay;
            this.AvoidRadius = AvoidRadius;

            HPmax = HP;
            this.HP = HPmax;

            this.SightRange = SightRange;
            this.MaxSpeed = MaxSpeed;
        }

        public override void Initialize()
        {
            base.Initialize();

            Damage = 2000;

            ObjectName = "FreighterAlly";
            movement = Movement.AI;

            //PositionX = 300;
            //PositionY = 500;

            Acceleration = 0.008f;
            Decceleration = 0.004f;

            DirectionX = 1;
            DirectionY = 0;

            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(2, 201, 43, 68)));
            CenterPoint = new Vector2(anim.CurrentFrame.SourceRectangle.Value.Width / 2,
                                      anim.CurrentFrame.SourceRectangle.Value.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void OnKilled()
        {
        }

        public override void CreateAI(AIBehaviour behaviour)
        {
            base.CreateAI(behaviour);
        }
    }
}
