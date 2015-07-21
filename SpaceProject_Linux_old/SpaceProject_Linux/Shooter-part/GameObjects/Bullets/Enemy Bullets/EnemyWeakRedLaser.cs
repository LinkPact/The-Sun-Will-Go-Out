using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    public class EnemyWeakRedLaser : EnemyBullet
    {
        public EnemyWeakRedLaser(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            Speed = 0.55f;
            Damage = 30;
            Duration = 1500;
            
            IsKilled = false;
            ObjectClass = "enemyBullet";
            
            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(23,25,5,9)));

            BoundingSpace = 0;

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Duration -= gameTime.ElapsedGameTime.Milliseconds;

            if (Duration <= 0)
                IsKilled = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI/2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }
    }
}
