using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class Meteorite25 : Meteorite
    {
        public Meteorite25(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            ObjectName = "Meteroite25";
        }

        public override void Initialize()
        {
            base.Initialize();

            HP = 300;
            Damage = 100;
            Speed = 0.25f;

            anim.LoopTime = 1;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(80, 65, 25, 25)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }
    }
}
