using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class SimpleAsteroid : SubInteractiveObject
    {
        private Vector2 coordinates;

        public SimpleAsteroid(Game1 Game, Sprite spriteSheet, Vector2 coordinates, String name) :
            base(Game, spriteSheet)
        {
            this.coordinates = coordinates;
            this.name = name;
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(724, 1075, 54, 55));
            position = MathFunctions.CoordinateToPosition(coordinates);
            base.Initialize();
            SetupText("A simple asteroid floating in space.");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Interact()
        {
            base.Interact();
        }

        protected override void SetClearedText()
        {
            clearedText = "Not applicable?";
        }
    }
}
