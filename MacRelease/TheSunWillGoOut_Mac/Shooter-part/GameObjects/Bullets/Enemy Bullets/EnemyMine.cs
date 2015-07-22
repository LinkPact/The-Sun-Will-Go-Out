using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class EnemyMine : EnemyShip
    {
        public float blastRadius;
        private float blastDamage;

        private Boolean delayIsActivated = false;
        private Boolean isActivated = false;
        private int remainingTimeBeforeExplosion;

        public EnemyMine(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        { }

        public override void Initialize()
        {
            base.Initialize();

            Speed = 0.02f;
            IsKilled = false;
            Damage = 0;
            ObjectClass = "enemy";
            ObjectName = "Mine";
            Duration = 50000;
            HP = 100;

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(80, 100, 15, 15)));

            Bounding = new Rectangle(149, 60, 9, 26);
            BoundingSpace = 0;
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
            useDeathAnim = true;

            blastRadius = 70;
            blastDamage = 70;

            ShootObjectTypes.Add("player");
            ShootObjectTypes.Add("ally");
            SightRange = 100;

            lootValue = LootValue.veryLow;
        }

        public void SetActivationTime(int activationTimeMilliseconds)
        {
            delayIsActivated = true;
            remainingTimeBeforeExplosion = activationTimeMilliseconds;
        }

        public override void Update(GameTime gameTime)
        {
            if (Duration <= 0)
                Explode();
            else
                CheckIfTargetIsClose();

            if (isActivated)
                DelayLogic(gameTime);

            base.Update(gameTime);

        }

        private void CheckIfTargetIsClose()
        {
            GameObjectVertical obj = FindAimObject();
            if (obj != null)
            {
                if (delayIsActivated)
                {
                    isActivated = true;
                }
                else
                { 
                    Explode();
                }
            }
        }

        private void DelayLogic(GameTime gameTime)
        {
            remainingTimeBeforeExplosion -= gameTime.ElapsedGameTime.Milliseconds;
            if (remainingTimeBeforeExplosion <= 0)
            {
                Explode();
            }
        }

        private void Explode()
        {
            IsKilled = true;

            Explosion expl = ExplosionGenerator.GenerateBombExplosion(Game, spriteSheet, this);

            CircularAreaDamage areaExpl = new CircularAreaDamage(Game, AreaDamageType.enemy, this.Position, blastDamage, blastRadius);
            areaExpl.Initialize();

            Game.stateManager.shooterState.backgroundObjects.Add(expl);
            Game.stateManager.shooterState.gameObjects.Add(areaExpl);
        }

        public override void OnKilled()
        {
            Explode();
            base.OnKilled();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color drawColor = Color.White;
            if (isActivated)
                drawColor = Color.Red;

            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, drawColor, (float)(Radians + Math.PI / 2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }

    }
}
