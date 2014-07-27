using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class StoppingModule : MovementModule
    {
        private Boolean isFullstop;

        protected float stopY;
        protected float stopYVariation;
        protected int stopTime;
        protected int currentStopCount;

        public StoppingModule(Game1 game, Boolean isFullStop)
            : base(game)
        {
            this.isFullstop = isFullStop;
        }

        public override void Setup(GameObjectVertical obj)
        {
            stopY = 250;
            stopYVariation = 200;
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
