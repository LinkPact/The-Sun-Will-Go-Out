using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    public class ResourceItem : QuantityItem
    {
        protected ResourceItem(Game1 Game) :
            base(Game)
        {
            Kind = "Resource";
            maxQuantity = 500;
        }

        protected override String GetDescription()
        {
            return "TODO: ADD DESCRIPTION";
        }

    }
}