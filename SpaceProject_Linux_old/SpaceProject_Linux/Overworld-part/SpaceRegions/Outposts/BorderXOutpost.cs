using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    public class BorderXOutpost : Outpost
    {
        private BorderStation station;
        private TrainingArea trainingArea;

        public BorderXOutpost(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            name = "Sector X Border Outpost";

            spaceRegionArea = new Rectangle(117000, 97000, 3000, 3000);

            station = new BorderStation(game, spriteSheet, new Vector2(spaceRegionArea.X, spaceRegionArea.Y));
            station.Initialize();

            AddGameObject(station);

            trainingArea = new TrainingArea(game, spriteSheet, new Vector2(spaceRegionArea.X, spaceRegionArea.Y));
            trainingArea.Initialize();

            AddGameObject(trainingArea);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
