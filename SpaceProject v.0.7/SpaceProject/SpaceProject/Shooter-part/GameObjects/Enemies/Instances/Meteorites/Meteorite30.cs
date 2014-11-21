using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class Meteorite30 : Meteorite
    {
        public Meteorite30(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            ObjectName = "Meteroite30";
        }

        public override void Initialize()
        {
            base.Initialize();

            HP = 400;
            Damage = 120;
            Speed = 0.2f;

            anim.LoopTime = 1;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(105, 65, 30, 30)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }
    }
}
