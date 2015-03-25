using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    class ResourceMeteoriteGoldMedium : ResourceMeteorite
    {

        public ResourceMeteoriteGoldMedium(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        { }

        public override void Initialize()
        {
            base.Initialize();

            HP = 500;
            Damage = 50;
            Speed = 0.225f;

            Rotation = (float)random.NextDouble() * (float)Math.PI / 30 - (float)Math.PI / 60;
            rotationDir = 0;

            ObjectName = "ResourceMeteoriteGoldMedium";

            anim.LoopTime = 1;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(31, 137, 24, 24)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
