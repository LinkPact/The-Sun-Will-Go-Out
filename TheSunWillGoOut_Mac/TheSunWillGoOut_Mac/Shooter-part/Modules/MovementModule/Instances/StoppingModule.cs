using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    class StoppingModule : MovementModule
    {
        private Boolean isFullstop;

        // Temporary, for debugging, will probably be needed soon again / 140914 Jakob
        private float internalVar;
        protected float stopY { 
            get 
            { 
                return internalVar; 
            } 
            set 
            { 
                internalVar = value; 
            }
        }
        protected float stopYVariation;
        protected int stopTime;
        protected int currentStopCount;

        public StoppingModule(Game1 game, Boolean isFullStop, float stopY = 250, float stopYVariation = 200)
            : base(game)
        {
            this.isFullstop = isFullStop;
            this.stopY = stopY;
            this.stopYVariation = stopYVariation;
        }

        public override void Setup(GameObjectVertical obj)
        {
            stopY += random.Next((int)stopYVariation) - stopYVariation / 2;

            if (!isFullstop)
                stopTime = 5000;
            else
                stopTime = 999999999;

            currentStopCount = 0;
        }

        public override void Update(GameTime gameTime, GameObjectVertical obj)
        {
            if (obj.PositionY >= stopY && stopTime > 0)
            {
                stopTime -= gameTime.ElapsedGameTime.Milliseconds;
                obj.DirectionY = 0;
            }
            else
            {
                obj.DirectionY = 1;
            }
        }
    }
}
