using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class RebelBomber_placeholder : EnemyShip
    {
        public RebelBomber_placeholder(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public RebelBomber_placeholder(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
            Movement movement) :
            base(Game, spriteSheet, player)
        {
            Setup();
            this.movement = movement;
        }

        private void Setup()
        {
            fraction = Fraction.pirate;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootRangeMin = 1;
            lootRangeMax = 3;

            //Egenskaper
            SightRange = 400;
            HP = 1.0f;
            Damage = 0;
            Speed = 0.1f;

            movement = Movement.Line;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(380, 400, 38, 58)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }
    }
}
