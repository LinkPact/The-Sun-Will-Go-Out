using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    /**
     * Slow low-end ship without weapons
     * Travels against the player and tries to collide with player, as well as covering
     * its friends by getting in the way of player fire
     */

    class AllianceDrone : EnemyShip
    {
        public AllianceDrone(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceDrone(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
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

            lootValue = LootValue.low;

            //Egenskaper
            SightRange = 400;
            HP = 400.0f;
            Damage = (float)CollisionDamage.low;
            Speed = 0.06f;
            TurningSpeed = 8f;

            movement = Movement.Following;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(470, 80, 29, 41)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }
    }
}
