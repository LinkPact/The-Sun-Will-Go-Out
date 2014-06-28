﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public enum Fraction
    {
        rebel,
        alliance, 
        pirate,
        other
    }

    public class EnemyShip : Creature
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
