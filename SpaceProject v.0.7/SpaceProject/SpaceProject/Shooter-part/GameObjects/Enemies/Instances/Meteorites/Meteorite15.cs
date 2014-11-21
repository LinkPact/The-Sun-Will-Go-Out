using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class Meteorite15 : Meteorite
    {
        public Meteorite15(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            ObjectName = "Meteroite15";
        }

        public override void Initialize()
        {
            base.Initialize();

            HP = 100;
            Damage = 60;
            Speed = 0.4f;

            anim.LoopTime = 1;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(45, 65, 15, 15)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }
    }
}
