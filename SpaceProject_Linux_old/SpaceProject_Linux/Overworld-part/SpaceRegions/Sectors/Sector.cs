using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    public class Sector : SpaceRegion
    {
        public ShipSpawner shipSpawner;

        protected Sector(Game1 game) :
            base(game)
        { }

        public override void Initialize()
        {
            base.Initialize();

            shipSpawner = new ShipSpawner(game, this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            shipSpawner.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
