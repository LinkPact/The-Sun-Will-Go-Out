using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class Meteorite15 : EnemyShip
    {
        float rotationDir;

        public Meteorite15(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            this.player = player;
            ObjectName = "Meteroite15";
            fraction = Fraction.other;
        }

        public override void Initialize()
        {
            base.Initialize();

            HP = 100;
            Damage = 60;
            Speed = 0.4f;

            Rotation = (float)random.NextDouble() * (float)Math.PI / 20 - (float)Math.PI / 40;
            rotationDir = 0;

            ObjectSubClass = "meteorite";
            
            anim.LoopTime = 1;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(45, 65, 15, 15)));

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
