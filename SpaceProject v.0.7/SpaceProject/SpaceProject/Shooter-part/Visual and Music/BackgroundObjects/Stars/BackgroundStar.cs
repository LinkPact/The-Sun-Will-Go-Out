using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class BackgroundStar : AnimatedGameObject
    {

        public BackgroundStar(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        { }

        public override void Initialize()
        {
            base.Initialize();

            anim.LoopTime = 1;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(6, 24, 2, 2)));

            Speed = Game.random.Next(4, 10) / 100f;
            scale = (float)(Game.random.NextDouble() * 1.5);
            baseColor = new Color(Game.random.Next(200, 255), Game.random.Next(155, 255), Game.random.Next(200, 255), 255);
            
            DrawLayer = 0.2f;
        }

        public override bool CheckOutside()
        {
            if (PositionX + anim.Width < 0 || PositionX - anim.Width > windowWidth
                || PositionY + anim.Width < 0 || PositionY - anim.Height > windowHeight)
            {
                return true;
            }
            else
                return false;
        }

        public override void OnKilled()
        { }
    }
}
