using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class OutpostX : Outpost
    {
        private Telmun planet;
        private OutpostXStation station;

        public OutpostX(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            name = "Outpost X";

            spaceRegionArea = new Rectangle(1000, 43000, 5000, 5000);

            planet = new Telmun(game, spriteSheet, new Vector2(spaceRegionArea.X, spaceRegionArea.Y));
            planet.Initialize();
            AddGameObject(planet);

            station = new OutpostXStation(game, spriteSheet, planet.position);
            station.Initialize();
            AddGameObject(station);
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
