using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{

    //TODO: UNFINISHED CLASS
    class HomingMissileCampingEnemy : ShootingEnemyShip
    {
        public HomingMissileCampingEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            this.player = player;
            ObjectName = "HomingMissileCamperEnemy";
        }

        public HomingMissileCampingEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
            Movement movement) :
            base(Game, spriteSheet, player)
        {
            this.player = player;
            this.movement = movement;
            ObjectName = "HomingMissileCamperEnemy";
        }

        public override void Initialize()
        {
            base.Initialize();

            AddPrimaryModule(2000, ShootingMode.Regular);
            primaryModule.SetRandomCharge(random);

            //Egenskaper
            SightRange = 400;
            HP = 100.0f;
            Damage = 20;
            Speed = 0.15f;

            movement = Movement.Line;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(410, 0, 28, 35)));
            
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsKilled)
            {
                base.Draw(spriteBatch);
            }
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            EnemyHomingMissileBullet bullet = new EnemyHomingMissileBullet(Game, spriteSheet, player);
            bullet.Position = Position;
            bullet.Direction = new Vector2(0, 1);
            bullet.Initialize();
            
            Game.stateManager.shooterState.gameObjects.Add(bullet);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
