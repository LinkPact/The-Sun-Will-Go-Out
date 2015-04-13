using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class ResourceMeteoriteCopper : ResourceMeteorite
    {
        Animation small, medium, large;
        int mediumLimit, largeLimit;

        public ResourceMeteoriteCopper(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            small = new Animation();
            small.LoopTime = 1;
            small.AddFrame(spriteSheet.GetSubSprite(new Rectangle(57, 169, 16, 16)));

            medium = new Animation();
            medium.LoopTime = 1;
            medium.AddFrame(spriteSheet.GetSubSprite(new Rectangle(31, 169, 24, 24)));

            large = new Animation();
            large.LoopTime = 1;
            large.AddFrame(spriteSheet.GetSubSprite(new Rectangle(0, 169, 30, 30)));
        }

        public override void Initialize()
        {
            base.Initialize();

            HPmax = 300;
            HP = HPmax;
            Damage = 0;
            Speed = 0.2f;

            Rotation = (float)random.NextDouble() * (float)Math.PI / 40 - (float)Math.PI / 80;
            rotationDir = 0;

            ObjectClass = "enemy";
            ObjectSubClass = "resource";
            ObjectName = "ResourceMeteoriteCopper";

            CurrentAnim = large;

            mediumLimit = 101;
            largeLimit = 201;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (HP >= largeLimit)
                CurrentAnim = large;
            else if (HP < largeLimit && HP >= mediumLimit)
                CurrentAnim = medium;
            else
                CurrentAnim = small;

            rotationDir += Rotation;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsKilled) base.Draw(spriteBatch);
        }
    }
}
