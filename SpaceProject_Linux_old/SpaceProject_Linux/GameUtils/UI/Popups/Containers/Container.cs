using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    abstract class Container
    {
        // variables
        protected Vector2 offset;
        protected Vector2 canvasPosition;
        protected Rectangle canvasRectangle;
        protected Vector2 position;

        protected Container(Vector2 canvasPosition, Rectangle canvasRectangle)
        {
            this.canvasPosition = canvasPosition;
            this.canvasRectangle = canvasRectangle;
        }

        public virtual void Update(GameTime gameTime, Vector2 canvasPosition)
        {
            this.canvasPosition = canvasPosition;
            SetPosition();
        }

        public abstract void SetDefaultPosition(Type type);

        protected void SetPosition()
        {
            position = new Vector2(canvasPosition.X - canvasRectangle.Width / 2 + offset.X,
                                   canvasPosition.Y - canvasRectangle.Height / 2 + offset.Y);
        }
    }
}
