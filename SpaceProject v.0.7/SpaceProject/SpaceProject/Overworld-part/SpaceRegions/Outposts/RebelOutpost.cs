using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class RebelOutpost : Outpost
    {
        private RebelStation1 rebelStation1;
        private RebelStation2 rebelStation2;
        private RebelStation3 rebelStation3;

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

            rebelStation1 = new RebelStation1(game, spriteSheet, new Vector2(spaceRegionArea.X, spaceRegionArea.Y));
            rebelStation1.Initialize();
            AddGameObject(rebelStation1);

            rebelStation2 = new RebelStation2(game, spriteSheet, new Vector2(spaceRegionArea.X, spaceRegionArea.Y));
            rebelStation2.Initialize();
            AddGameObject(rebelStation2);

            rebelStation3 = new RebelStation3(game, spriteSheet, new Vector2(spaceRegionArea.X, spaceRegionArea.Y));
            rebelStation3.Initialize();
            AddGameObject(rebelStation3);

            rebelBeacon = new Beacon(game, spriteSheet, new Rectangle(681, 234, 100, 100), new Rectangle(580, 234, 100, 100),
                "Rebel Base Beacon", rebelStation2.position + new Vector2(300, 250));
            rebelBeacon.Initialize();
            game.stateManager.overworldState.AddBeacon(rebelBeacon);
            AddGameObject(rebelBeacon);
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
