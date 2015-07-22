using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    public enum Fraction
    {
        rebel,
        alliance, 
        pirate,
        other
    }

    public enum CollisionDamage
    {
        veryLow = 75,
        low = 125,
        medium = 175,
        high = 250,
        extreme = 1000
    }

    public class EnemyShip : VerticalShooterShip
    {
        protected Fraction fraction;
        public Fraction GetFraction { get { return fraction; } }

        protected EnemyShip(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player)
            : base(Game, spriteSheet, player)
        {
            ObjectClass = "enemy";
        }

        protected EnemyShip(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player, Movement movement) :
            base(Game, spriteSheet, player, movement)
        {
            ObjectClass = "enemy";
        }
    }
}
