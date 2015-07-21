using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    class RegularBomb : PlayerBullet
    {
        private float blastRadius;
        private float blastDamage;

        public RegularBomb(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //Egenskaper
            Speed = 0.20f;
            IsKilled = false;
            Damage = 0;
            ObjectClass = "bullet";
            Duration = 600;
            TempInvincibility = Duration;

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(80, 100, 20, 20)));

            Bounding = new Rectangle(80, 100, 20, 20);
            BoundingSpace = 0;
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

            blastRadius = 100;
            blastDamage = 100;
        }

        public override void Update(GameTime gameTime)
        {
            if (Duration <= 0)
                Explode();
            
            base.Update(gameTime);
        }

        private void Explode()
        {
            Explosion expl = ExplosionGenerator.GenerateBombExplosion(Game, spriteSheet, this);

            CircularAreaDamage areaExpl = new CircularAreaDamage(Game, AreaDamageType.player, this.Position, blastDamage, blastRadius);
            //areaExpl.Damage = blastDamage;

            Game.stateManager.shooterState.backgroundObjects.Add(expl);
            Game.stateManager.shooterState.gameObjects.Add(areaExpl);

        }

        public override void OnKilled()
        {
            base.OnKilled();
            Explode();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI / 2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }

    }
}