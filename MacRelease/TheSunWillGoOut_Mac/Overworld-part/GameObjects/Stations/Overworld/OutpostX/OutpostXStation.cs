using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class OutpostXStation : Station
    {
        public OutpostXStation(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {
            
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 523, 93, 93));

            base.Initialize();

            StationCodeName = "OX_OutpostX_Station";

            LoadStationData(StationCodeName);
        }
    }
}
