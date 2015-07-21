using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    class Meteorite : EnemyShip
    {
        float rotationDir;

        public Meteorite(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            fraction = Fraction.other;
        }

        public override void Initialize()
        {
            base.Initialize();

            Rotation = (float)random.NextDouble() * (float)Math.PI / 20 - (float)Math.PI / 40;
            rotationDir = 0;

            ObjectSubClass = "meteorite";
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
