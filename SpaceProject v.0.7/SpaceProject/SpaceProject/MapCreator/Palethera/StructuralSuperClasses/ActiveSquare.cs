using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject.MapCreator
{
    abstract class ActiveSquare : Clickable
    {
        protected ActiveSquare(Vector2 position)
            : base(position)
        { }

    }
}
