using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class MiningOutpost : Outpost
    {
        private MiningStation station;
        private MiningAsteroids asteroids;

        public MiningOutpost(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            name = "Mining Outpost";

            spaceRegionArea = new Rectangle(123500, 92000, 2000, 2000);

            station = new MiningStation(game, spriteSheet, new Vector2(spaceRegionArea.X, spaceRegionArea.Y));
            station.Initialize();
            AddGameObject(station);

            asteroids = new MiningAsteroids(game, spriteSheet);
            asteroids.Initialize();
            AddGameObject(asteroids);
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
