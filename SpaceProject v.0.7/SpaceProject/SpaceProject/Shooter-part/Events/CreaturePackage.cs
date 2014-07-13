using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class CreaturePackage
    {
        Random random = new Random();

        private Game1 Game;
        private Sprite spriteSheet;

        private VerticalShooterShip creature;
        public VerticalShooterShip Creature { get { return creature; } }

        private float initHP;
        private Vector2 initPos;

        private float timing;

        public CreaturePackage(Game1 Game, Sprite spriteSheet, 
            VerticalShooterShip creature, float timing)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;

            this.creature = creature;
            this.timing = timing;

            this.initPos = creature.Position;
            this.initHP = creature.HP;
        }
        
        public VerticalShooterShip RetrieveCreature()
        {
            return creature;
        }

        public bool StartPassed(float currentTime)
        {
            if (currentTime >= timing) return true;
            else return false;
        }
        
        public void CreateCreature()
        {
            if (creature != null)
            {
                creature.HP = initHP;
                creature.IsOutside = false;
                creature.IsKilled = false;
                
                if (initPos.X == -1.0f) 
                {
                    creature.PositionX = (float)(random.NextDouble() * 800);
                    creature.PositionY = 0;
                }
                else 
                    creature.Position = initPos;
                
                Game.stateManager.shooterState.gameObjects.Add(creature);
            }
        }
    }
}
