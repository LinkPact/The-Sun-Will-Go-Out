using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class TrainingArea2 : Station
    {
        public TrainingArea2(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(95, 523, 177, 141));
            base.Initialize();
            StationCodeName = "BO_Training_Area2";
            LoadStationData(StationCodeName);
            name = "Training Area 2";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
