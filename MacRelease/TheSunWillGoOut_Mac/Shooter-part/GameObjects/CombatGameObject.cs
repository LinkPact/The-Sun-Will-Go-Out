using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    public abstract class CombatGameObject : AnimatedGameObject
    {
        protected float redLevel;
        protected readonly float redShiftTimeDamaged = 1000;
        protected readonly float redShiftTimeBadlyDamaged = 300;
        protected Boolean redToningIn;

        protected CombatGameObject(Game1 Game, Sprite spriteSheet)
            : base(Game, spriteSheet)
        { }

        public override void Initialize()
        {
            base.Initialize();

            redLevel = 0;
            redToningIn = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (HP < HPmax / 5)
            {
                UpdateRedTint(gameTime, redShiftTimeBadlyDamaged);
            }
            else if (HP < 2 * HPmax / 5)
            {
                UpdateRedTint(gameTime, redShiftTimeDamaged);
            }
        }

        protected Color GetDamageTintColor()
        {
            int shiftLevel = (int)(255 - redLevel);
            return new Color(255, shiftLevel, shiftLevel);
        }

        private void UpdateRedTint(GameTime gameTime, float redShiftTime)
        {
            float redShiftAmount = gameTime.ElapsedGameTime.Milliseconds / redShiftTime * 255;

            if (redToningIn)
            {
                redLevel += redShiftAmount;
                if (redLevel >= 255)
                {
                    redLevel = 255;
                    redToningIn = false;
                }
            }
            else
            {
                redLevel -= redShiftAmount;
                if (redLevel <= 0)
                {
                    redLevel = 0;
                    redToningIn = true;
                }
            }
        }

        public abstract void InflictDamage(GameObjectVertical obj);
    }
}
