using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class ResourceMeteoriteGoldSmall : ResourceMeteorite
    {

        public ResourceMeteoriteGoldSmall(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        { }

        public override void Initialize()
        {
            base.Initialize();

            HP = 250;
            Damage = 25;
            Speed = 0.25f;

            Rotation = (float)random.NextDouble() * (float)Math.PI / 25 - (float)Math.PI / 50;
            rotationDir = 0;

            ObjectName = "ResourceMeteoriteGoldSmall";

            anim.LoopTime = 1;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(57, 137, 16, 16)));

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
