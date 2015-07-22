using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public class RebelOutpost : Outpost
    {
        private RebelBaseStation rebelStation1;
        private RebelBaseShop rebelBaseShop;
        private Beacon rebelBeacon;

        public RebelOutpost(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            name = "Rebel Outpost";

            spaceRegionArea = new Rectangle(102000, 78000, 2000, 1500);

            rebelStation1 = new RebelBaseStation(game, spriteSheet, new Vector2(spaceRegionArea.X, spaceRegionArea.Y));
            rebelStation1.Initialize();

            rebelBeacon = new Beacon(game, spriteSheet, new Rectangle(681, 234, 100, 100), new Rectangle(580, 234, 100, 100),
                    "Rebel Base Beacon", rebelStation1.position + new Vector2(300, 250));
            rebelBeacon.Initialize();

            rebelBaseShop = new RebelBaseShop(game, spriteSheet, rebelStation1.position);
            rebelBaseShop.Initialize();

            AddGameObject(rebelStation1);
            AddGameObject(rebelBeacon);
            AddGameObject(rebelBaseShop);

            game.stateManager.overworldState.AddBeacon(rebelBeacon);
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
