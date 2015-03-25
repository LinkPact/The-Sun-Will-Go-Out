using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    class MediumZigZagModule : MovementModule
    {
        protected float zigzagInterval;
        protected float zigzagXdir;
        protected bool zigzagDirRight;

        public MediumZigZagModule(Game1 game)
            : base(game)
        { }

        public override void Setup(GameObjectVertical obj)
        {
            zigzagInterval = 1.0f;
            zigzagXdir = 0.0f;

            if (random.NextDouble() > 0.5) zigzagDirRight = true;
            else zigzagDirRight = false;
        }

        public override void Update(GameTime gameTime, GameObjectVertical obj)
        {
            if (zigzagDirRight)
                zigzagXdir += (zigzagInterval / 60);
            else
                zigzagXdir -= (zigzagInterval / 60);

            if (zigzagXdir > zigzagInterval)
                zigzagDirRight = false;

            if (zigzagXdir < -zigzagInterval)
                zigzagDirRight = true;

            obj.DirectionX = zigzagXdir;
        }
    }
}
