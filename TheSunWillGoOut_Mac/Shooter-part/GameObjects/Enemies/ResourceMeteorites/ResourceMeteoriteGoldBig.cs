using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class ResourceMeteoriteGoldBig : ResourceMeteorite
    {

        public ResourceMeteoriteGoldBig(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        { }

        public override void Initialize()
        {
            base.Initialize();

            HP = 1000;
            Damage = 100;
            Speed = 0.2f;

            Rotation = (float)random.NextDouble() * (float)Math.PI / 40 - (float)Math.PI / 80;
            rotationDir = 0;

            ObjectName = "ResourceMeteoriteGoldBig";

            anim.LoopTime = 1;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(0, 137, 30, 30)));

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
