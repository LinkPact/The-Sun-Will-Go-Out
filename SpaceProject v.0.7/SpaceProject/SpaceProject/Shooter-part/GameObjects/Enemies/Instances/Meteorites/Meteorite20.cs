using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class Meteorite20 : Meteorite
    {
        public Meteorite20(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            ObjectName = "Meteroite20";
        }

        public override void Initialize()
        {
            base.Initialize();

            HP = 200;
            Damage = 80;
            Speed = 0.3f;

            lootValue = LootValue.none;

            anim.LoopTime = 1;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(60, 65, 20, 20)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }
    }
}
