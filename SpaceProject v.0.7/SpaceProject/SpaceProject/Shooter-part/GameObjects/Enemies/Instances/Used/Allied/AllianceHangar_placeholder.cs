using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class AllianceHangar_placeholder : EnemyShip
    {
        public AllianceHangar_placeholder(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceHangar_placeholder(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
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

            lootValue = LootValue.veryHigh;

            //Egenskaper
            SightRange = 400;
            HP = 1.0f;
            Damage = 0;
            Speed = 0.1f;

            movement = Movement.Line;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(420, 340, 58, 58)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }
    }
}
