using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class Meteorite20 : EnemyShip
    {
        float rotationDir;

        public Meteorite20(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            this.player = player;
            ObjectName = "Meteroite20";
            fraction = Fraction.other;
        }

        public override void Initialize()
        {
            base.Initialize();

            HP = 200;
            Damage = 80;
            Speed = 0.3f;

            Rotation = (float)random.NextDouble() * (float)Math.PI / 20 - (float)Math.PI / 40;
            rotationDir = 0;

            ObjectSubClass = "meteorite";
            
            anim.LoopTime = 1;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(60, 65, 20, 20)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            rotationDir += Rotation;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, rotationDir, CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }
    }
}
